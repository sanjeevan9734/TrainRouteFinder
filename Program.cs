using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrainRoutesFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the path to the input file:");
                string filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File not found.");
                    return;
                }

                var routes = File.ReadAllLines(filePath)
                    .Select(line => line.Split(','))
                    .Select(parts => new Route(parts[0], parts[1], int.Parse(parts[2])))
                    .ToList();

                var graph = new Graph(routes);

                // Running predefined tests
                Console.WriteLine("Test #1: The distance of the route A=>B=>C is " + graph.CalculateDistance("A", "B", "C"));
                Console.WriteLine("Test #2: The distance of the route A=>D is " + graph.CalculateDistance("A", "D"));
                Console.WriteLine("Test #3: The distance of the route A=>D=>C is " + graph.CalculateDistance("A", "D", "C"));
                Console.WriteLine("Test #4: The distance of the route A=>E=>B=>C=>D is " + graph.CalculateDistance("A", "E", "B", "C", "D"));
                Console.WriteLine("Test #5: Route A=>E=>D " + (graph.CalculateDistance("A", "E", "D") == -1 ? "Route doesn't exist" : "Route exists"));
                Console.WriteLine("Test #6: Number of trips from C to C with maximum 3 stops is " + graph.CountTripsWithMaxStops("C", "C", 3) + " ( C=>D=>C, C=>E=>B=>C ) ");
                Console.WriteLine("Test #7: Number of trips from A to C with exactly 4 stops is " + graph.CountTripsWithExactStops("A", "C", 4) + "( A=>B=>C=>D=>C, A=>D=>C=>D=>C, A=>D=>E=>B=>C )");
                Console.WriteLine("Test #8: The length of the shortest route from A to C is " + graph.ShortestRouteLength("A", "C") + "( A=>B=>C )");
                Console.WriteLine("Test #9: The length of the shortest route from B to B is " + graph.ShortestRouteLength("B", "B") + "( B=>C=>E=>B )");
                Console.WriteLine("Test #10: The number of trips from C to C with distance less than 30 is " + graph.CountTripsWithMaxDistance("C", "C", 30) + "( C=>D=>C, C=>D=>C=>E=>B=>C, C=>D=>E=>B=>C, C=>E=>B=>C, C=>E=>B=>C=>D=>C, C=>E=>B=>C=>E=>B=>C, C=>E=>B=>C=>E=>B=>C=>E=>B=>C )");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }

    public class Route
    {
        public string From { get; }
        public string To { get; }
        public int Distance { get; }

        public Route(string from, string to, int distance)
        {
            From = from;
            To = to;
            Distance = distance;
        }
    }

    public class Graph
    {
        private readonly Dictionary<string, List<Route>> _adjacencyList;

        public Graph(IEnumerable<Route> routes)
        {
            _adjacencyList = new Dictionary<string, List<Route>>();

            foreach (var route in routes)
            {
                if (!_adjacencyList.ContainsKey(route.From))
                {
                    _adjacencyList[route.From] = new List<Route>();
                }

                _adjacencyList[route.From].Add(route);
            }
        }

        public int CalculateDistance(params string[] towns)
        {
            int totalDistance = 0;

            for (int i = 0; i < towns.Length - 1; i++)
            {
                var from = towns[i];
                var to = towns[i + 1];

                var route = _adjacencyList.GetValueOrDefault(from)?.FirstOrDefault(r => r.To == to);

                if (route == null)
                {
                    return -1; // Route doesn't exist
                }

                totalDistance += route.Distance;
            }

            return totalDistance;
        }

        public int CountTripsWithMaxStops(string start, string end, int maxStops)
        {
            return CountTrips(start, end, 0, maxStops, true);
        }

        public int CountTripsWithExactStops(string start, string end, int exactStops)
        {
            return CountTrips(start, end, 0, exactStops, false);
        }

        public int ShortestRouteLength(string start, string end)
        {
            return FindShortestRoute(start, end, 0, new HashSet<string>());
        }

        public int CountTripsWithMaxDistance(string start, string end, int maxDistance)
        {
            return CountTripsByDistance(start, end, 0, maxDistance);
        }

        private int CountTrips(string current, string end, int stops, int limit, bool isMaxStops)
        {
            if (stops > limit) return 0;

            int count = 0;

            if (stops > 0 && current == end && (isMaxStops || stops == limit))
            {
                count++;
            }

            foreach (var route in _adjacencyList.GetValueOrDefault(current, new List<Route>()))
            {
                count += CountTrips(route.To, end, stops + 1, limit, isMaxStops);
            }

            return count;
        }

        private int FindShortestRoute(string current, string end, int currentDistance, HashSet<string> visited)
        {
            if (visited.Contains(current)) return int.MaxValue;

            if (current == end && currentDistance > 0) return currentDistance;

            visited.Add(current);

            int shortest = int.MaxValue;

            foreach (var route in _adjacencyList.GetValueOrDefault(current, new List<Route>()))
            {
                shortest = Math.Min(shortest, FindShortestRoute(route.To, end, currentDistance + route.Distance, visited));
            }

            visited.Remove(current);

            return shortest;
        }

        private int CountTripsByDistance(string current, string end, int currentDistance, int maxDistance)
        {
            if (currentDistance >= maxDistance) return 0;

            int count = 0;

            if (current == end && currentDistance > 0)
            {
                count++;
            }

            foreach (var route in _adjacencyList.GetValueOrDefault(current, new List<Route>()))
            {
                count += CountTripsByDistance(route.To, end, currentDistance + route.Distance, maxDistance);
            }

            return count;
        }
    }
}
