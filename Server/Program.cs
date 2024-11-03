using System;
using Nancy.Hosting.Self;

namespace StudentDashboard
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:8080");
            var hostConfig = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true }
            };

            using (var host = new NancyHost(hostConfig, uri))
            {
                Console.WriteLine($"Starting Nancy on {uri}");
                host.Start();
                Console.ReadLine(); // Keeps the server running
            }
        }
    }
}
