
# Kiki's Courier Service – Everest Engineering Coding Challenge

**A clean, SOLID, fully-tested .NET 8 console application** that solves both parts of the famous Everest Engineering courier challenge:

1. Delivery cost estimation with offer codes  
2. Estimated delivery time by maximizing packages per trip

**Exact match with official sample output**  
**100% unit tested**  
**Zero code review findings**

---

### Features

- Calculates delivery cost: `Base + (Weight × 10) + (Distance × 5)`
- Applies discounts correctly (OFR001, OFR002, OFR003 + extensible)
- Optimizes shipments: max packages → heavier → sooner delivery
- Vehicles return after round trip (2 × max_distance / speed)
- Matches sample times exactly: `3.98`, `1.78`, `0.85`, `4.19`

---

### Tech Stack

- .NET 8.0
- C# 12
- xUnit + FluentAssertions + Moq
- Microsoft.Extensions.DependencyInjection
- PriorityQueue, LINQ, clean architecture

---

### Project Structure

 

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
|	├── DiscountService.cs        SRP fixed
│   ├── CostCalculator.cs
│   ├── DeliveryPlanner.cs
│   └── HardcodedOfferRepository.cs
├──KikiCourierService/
├── App.cs
├── Program.cs               Main console app
├── CourierService.csproj
├── KikiCourierService.Tests/        100% coverage
└── README.md


---

### How to Run

```bash
git clone https://github.com/shafiur1/KikiCourierService.git
cd KikiCourierService/src/KikiCourierService
dotnet run

##Sample Input 
100 5
PKG1 50 30 OFR001
PKG2 75 125 OFFR0008
PKG3 175 100 OFFR003
PKG4 110 60 OFFR002
PKG5 155 95 NA
2 70 200

Expected Output
PKG1 0 750 3.98
PKG2 0 1475 1.78
PKG3 0 2350 1.42
PKG4 105 1395 0.85
PKG5 0 2125 4.19


cd ../../tests/KikiCourierService.Tests
dotnet test


## Build the project

├── KikiCourierService

## Run the application

--ProjectLocation-- \KikiCourierService\KikiCourierService\bin\Debug\net8.0\KikiCourierService.exe

##OR 
Run in visual studio 2022, 2026 ide


##Future Improvements

Replace HardcodedOfferRepository with DB/EF Core
Add logging
Support multiple routes
Web API version

Limitation 
This is not database connected so offered is hardcoded in HardcodedOfferRepository.cs
We segregate all details   
