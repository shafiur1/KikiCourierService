# Kiki Courier Service Delivery Cost and Time Estimator

## Overview

This project is a command-line application built in C# using .NET 8,  It calculates the delivery cost for packages with optional discount offers and estimates delivery times based on vehicle availability, maximizing efficiency in shipments.

The application follows SOLID principles:
- **Single Responsibility Principle (SRP)**: Classes like `CostCalculator` and `DeliveryPlanner` handle specific tasks.
- **Open-Closed Principle (OCP)**: Offers are extensible via the `IOfferRepository` interface without modifying core logic.
- **Liskov Substitution Principle (LSP)**: Interfaces ensure substitutability.
- **Interface Segregation Principle (ISP)**: Small, focused interfaces.
- **Dependency Inversion Principle (DIP)**: Dependencies are injected using Microsoft.Extensions.DependencyInjection.

## Features

- Calculates delivery costs with discounts based on offer codes.
- Estimates delivery times by optimizing shipments across multiple vehicles.
- Extensible for additional offer codes via repository pattern.
- Uses backtracking to find optimal shipments (max packages, then max weight, then min time).

## Requirements

- .NET 8 SDK
- No external dependencies beyond .NET standard libraries.
- visual studio 2022, 2026 ide supports
 

## Project Structure
KikiCourierService/
├── KikiCourierService.Interfaces/
│   ├── ICostCalculator.cs
│   ├── IDeliveryPlanner.cs
│   └── IOfferRepository.cs
├── KikiCourierService.Models/
│   ├── Offer.cs
│   ├── Package.cs
│   ├── Shipment.cs (internal)
│   └── Vehicle.cs
├── KikiCourierService.Services/
│   ├── CostCalculator.cs
│   ├── DeliveryPlanner.cs
│   └── HardcodedOfferRepository.cs
├──KikiCourierService/
├── App.cs
├── Program.cs
├── CourierService.csproj
└── README.md

## Build the project

├── KikiCourierService

## Run the application

--ProjectLocation-- \KikiCourierService\KikiCourierService\bin\Debug\net8.0\KikiCourierService.exe

##OR 
Run in visual studio 2022, 2026 ide


##Test Cases  1

###Input Format (Part 1: Cost Estimation Only)
	base_delivery_cost no_of_packages
	pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1

Example:1
#Input 
100 3
PKG1 5 5 OFR001
PKG2 15 5 OFR002
PKG3 10 100 OFR003

#Output:
PKG1 0 175
PKG2 0 275
PKG3 35 665

##Test Cases  2

#Input Format (Part 2: Cost + Time Estimation)

#base_delivery_cost no_of_packages
#pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1


#no_of_vehicles max_speed max_carriable_weight

Example:2

#Input
100 5
PKG1 50 30 OFR001
PKG2 75 125 OFFR0008
PKG3 175 100 OFFR003
PKG4 110 60 OFFR002
PKG5 155 95 NA
2 70 200

#Output:
PKG1 0 750 3.98
PKG2 0 1475 1.78
PKG3 0 2350 1.42
PKG4 105 1395 0.85
PKG5 0 2125 4.19


Limitation 
This is not database connected so offered is hardcoded in HardcodedOfferRepository.cs
We segregate all details   
