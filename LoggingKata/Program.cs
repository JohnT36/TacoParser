using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            // TODO:  Find the two Taco Bells that are the furthest from one another.            

            

            var lines = File.ReadAllLines(csvPath);
            if (lines.Length == 0)
            {
                logger.LogError("Error no Input. 0 files");
            }
            if (lines.Length == 1)
            {
                logger.LogWarning("Warning. Only 1 file found.");
            }
            logger.LogInfo("Log initialized");

            logger.LogInfo($"Lines: {lines[0]}");

            var parser = new TacoParser();

            var locations = lines.Select(parser.Parse).ToArray();

            ITrackable tacoOne = null;
            ITrackable tacoTwo = null;
            var count = 0;
            var count2 = 0;
            double distance = 0;

            foreach (var locA in locations)
            {
                var coordA = new GeoCoordinate(locA.Location.Latitude, locA.Location.Longitude);
                count++;
                foreach (var locB in locations)
                {
                    var coordB = new GeoCoordinate(locB.Location.Latitude, locB.Location.Longitude);
                    var dist = coordA.GetDistanceTo(coordB);
                    count2++;
                    if (dist > distance)
                    {
                        distance = dist;
                        tacoOne = locA;
                        tacoTwo = locB;
                    }
                }
            }

            Console.WriteLine($"First Location:{tacoOne.Name}," +
                $" Long: {tacoOne.Location.Longitude}, Lat: {tacoOne.Location.Latitude}");

            Console.WriteLine($"Second Location:{tacoTwo.Name}," +
                $" Long: {tacoTwo.Location.Longitude}, Lat: {tacoTwo.Location.Latitude}");

            Console.WriteLine($"{Math.Round(distance * 0.00062)} miles");
            Console.WriteLine($"{count} loops for the first loop");
            Console.WriteLine($"{count2} loops for the second loop");
        }
    }
}
