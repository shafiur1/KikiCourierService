
using KikiCourierService.Interfaces;
using KikiCourierService.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KikiCourierService.Services
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShowBranding();

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            var calculator = serviceProvider.GetRequiredService<ICostCalculator>();
            var planner = serviceProvider.GetRequiredService<IDeliveryPlanner>();
            var offerRepository = serviceProvider.GetRequiredService<IOfferRepository>();
            var offers = offerRepository.GetAllOffers();

            try
            {
                var (baseCost, packages) = ReadInput();
                calculator.CalculateCosts(baseCost, packages, offers);

                // Read vehicle info if present
                string? line = Console.ReadLine();
                if (line != null && line.Trim().Split(' ').Length == 3)
                {
                    var parts = line.Trim().Split(' ');
                    int numVehicles = int.Parse(parts[0]);
                    double maxSpeed = double.Parse(parts[1]);
                    double maxWeight = double.Parse(parts[2]);

                    planner.PlanDeliveries(packages, numVehicles, maxSpeed, maxWeight);
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n════════════════════════════════════");
                Console.WriteLine("        FINAL DELIVERY REPORT       ");
                Console.WriteLine("════════════════════════════════════");
                Console.ResetColor();

                foreach (var pkg in packages)
                {
                    Console.WriteLine(pkg);
                }

                Console.WriteLine("\nDelivered with love by Kiki, Tombo & Jiji");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void ShowBranding()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"                                                                                
   __  __     _    _    ___ _    _    ___ _  _ ___ ___  ___ ___ _  _ ___ ___ _   ___ 
  |  \/  |   | |  | |  / __| |  | |  / __| | | | _ \ _ \/ __/ __| \| | __| _ \ | | __|
  | |\/| |   | |__| | | (__| |__| | | (__| |_| |   /   \__ \__ \ .` | _||   / |__| _|
  |_|  |_|   |____|_|  \___|____|_|  \___|\___/|_|_\_|_\___|___/_|\_|___|_|_\____|___|
                                                                                
");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                    KikiCourierService v2.0 - Powered by Love & Magic");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                        Craft what you love, love what you craft");
            Console.WriteLine("                                 EVERESTENGINEERING");
            Console.ResetColor();
            Console.WriteLine("\n" + new string('═', 80));
            Console.WriteLine();
        }

        private static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ICostCalculator, CostCalculator>();
            services.AddScoped<IDeliveryPlanner, DeliveryPlanner>();
            services.AddSingleton<IOfferRepository, HardcodedOfferRepository>();
            return services;
        }

        private static (double baseCost, List<Package> packages) ReadInput()
        {
            Console.WriteLine("Enter base delivery cost and number of packages:");
            var firstLine = Console.ReadLine()!.Trim();
            var parts = firstLine.Split(' ');
            double baseCost = double.Parse(parts[0]);
            int count = int.Parse(parts[1]);

            var packages = new List<Package>();
            Console.WriteLine($"\nEnter {count} packages (format: id weight_in_kg distance_in_km offer_code):");

            for (int i = 0; i < count; i++)
            {
                var line = Console.ReadLine()!.Trim();
                var pkgParts = line.Split(' ');
                packages.Add(new Package
                {
                    Id = pkgParts[0],
                    Weight = double.Parse(pkgParts[1]),
                    Distance = double.Parse(pkgParts[2]),
                    OfferCode = pkgParts.Length > 3 ? pkgParts[3] : null
                });
            }

            return (baseCost, packages);
        }
    }
}






