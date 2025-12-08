
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
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<ICostCalculator, CostCalculator>();
                    services.AddTransient<IDeliveryPlanner, DeliveryPlanner>();
                    services.AddSingleton<IOfferRepository, HardcodedOfferRepository>();
                })
                .Build();
            //This code separation for easy to understand
            var app = new App(host.Services);
            app.UiLook();
           
            app.RunProblem();


            Console.Write("Type Exit for Exit\n> ");
            string? str = Console.ReadLine();
            if (str.ToLower() == "exit")
            {
                System.Environment.Exit(0);
            }

        }
    }

    public class App
    {
        private readonly ICostCalculator _costCalculator;
        private readonly IDeliveryPlanner _deliveryPlanner;
        private readonly IOfferRepository _offerRepository;

        public App(IServiceProvider serviceProvider)
        {
            _costCalculator = serviceProvider.GetRequiredService<ICostCalculator>();
            _deliveryPlanner = serviceProvider.GetRequiredService<IDeliveryPlanner>();
            _offerRepository = serviceProvider.GetRequiredService<IOfferRepository>();
        }

   

        public void RunProblem()
        {

            //Console.Write("Enter: base_delivery_cost no_of_packages\n> ");
            Console.Write("Enter: base_delivery_cost, no_of_packages (separated by spaces):\n> ");
            string? line = Console.ReadLine();
            if (line == null) return;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            double baseCost;
            int numPackages;

            if (double.TryParse(parts[0], out baseCost))
            {
                if (int.TryParse(parts[1], out numPackages))
                {

                    baseCost = double.Parse(parts[0]);
                    numPackages = int.Parse(parts[1]);

                    List<Package> packages = new();
                    for (int i = 0; i < numPackages; i++)
                    {
                        Console.Write("Enter pkg_id, weight (kg), distance (km), and offer_code (separated by spaces): ");

                        line = Console.ReadLine();
                        if (line == null) return;
                        parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        string id;
                        double weight;
                        double distance;

                        if (double.TryParse(parts[1], out weight))
                        {
                            if (double.TryParse(parts[2], out distance))
                            {
                                id = parts[0];
                                weight = double.Parse(parts[1]);
                                distance = double.Parse(parts[2]);



                                string offerCode = parts.Length > 3 ? parts[3] : null!;
                                if (offerCode == "NA") offerCode = null;
                                packages.Add(new Package { Id = id, Weight = weight, Distance = distance, OfferCode = offerCode });
                            }
                            else
                            {

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input. distance (km).");
                            }
                        }
                        else
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. weight (kg).");
                        }


                    }

                    var offers = _offerRepository.GetAllOffers();
                    _costCalculator.CalculateCosts(baseCost, packages, offers);
                    foreach (var pkg in packages)
                    {
                        Console.Write($"{pkg.Id} {(int)pkg.Discount} {(int)pkg.TotalCost}");

                        Console.WriteLine();
                    }

                    bool hasDeliveryTime = false;
                    Console.Write("Enter no_of_vehicles, max_speed, max_carriable_weight (separated by spaces): ");
                    line = Console.ReadLine();
                    if (line != null)
                    {
                        parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 3)
                        {
                            int numVehicles;
                            double speed;
                            double maxWeight;
                            if (int.TryParse(parts[0], out numVehicles))
                            {
                                if (double.TryParse(parts[1], out speed))
                                {
                                    if (double.TryParse(parts[2], out maxWeight))
                                    {
                                        numVehicles = int.Parse(parts[0]);
                                        speed = double.Parse(parts[1]);
                                        maxWeight = double.Parse(parts[2]);


                                        _deliveryPlanner.PlanDeliveries(packages, numVehicles, speed, maxWeight);
                                        hasDeliveryTime = true;
                                    }
                                    else
                                    {

                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid input. max_carriable_weight.");
                                    }
                                }
                                else
                                {

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid input. max_speed.");
                                }
                            }
                            else
                            {

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input. no_of_vehicles.");
                            }
                        }
                        else
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input");
                        }
                    }

                    foreach (var pkg in packages)
                    {
                        Console.Write($"{pkg.Id} {(int)pkg.Discount} {(int)pkg.TotalCost}");
                        if (hasDeliveryTime)
                        {
                            Console.Write($" {pkg.DeliveryTime:F2}");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. No Of Packages.");
                }
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Base Delivery Cost.");

            }

        }

        public void UiLook()
        {
            Console.Title = "Coding Challenges";
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("===============================================");
            Console.WriteLine("                KIKI  COURIER SERVICE          ");
            Console.WriteLine("===============================================");
            Console.ResetColor();           
            
        }
        
       
    }
}