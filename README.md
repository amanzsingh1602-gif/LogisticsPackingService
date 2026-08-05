# Logistics Packing Service
 
## Overview
 
Logistics Packing Service is an ASP.NET Core Web API that calculates the number of shipping boxes required for a collection of packages using a practical packing heuristic.

The solution implements a **Shelf Packing heuristic** where packages are processed in descending order of volume and are placed into existing shipping boxes whenever possible before opening a new box.
 
Package rotation is supported to maximize the chance of fitting a package into a suitable shipping box.
 
---
 
# Solution Architecture
 
The solution follows **Clean Architecture** principles and is divided into the following projects.
 
## LogisticsPackingService.Api
 
- REST API endpoints
- Swagger / OpenAPI configuration
- Global exception handling
- Dependency Injection configuration
 
## LogisticsPackingService.Application
 
- Application services
- Packing algorithm abstraction (`IPackingAlgorithm`)
- DTOs
- FluentValidation validators
 
## LogisticsPackingService.Domain
 
- Domain entities
- Value objects
- Domain exceptions
 
## LogisticsPackingService.Infrastructure
 
- Box catalog provider
- Configuration using the Options Pattern
- Dependency Injection registrations
 
## LogisticsPackingService.Tests
 
- Packing service tests
- Packing algorithm tests
- Validator tests
 
---
 
# Technologies Used
 
- .NET 10
- ASP.NET Core Web API
- FluentValidation
- xUnit
- Moq
- FluentAssertions
- Swagger / OpenAPI
 
---
 
# Packing Heuristic
 
The solution uses a **Shelf Packing** heuristic.
 
The algorithm works as follows:
 
1. Packages are sorted in descending order of volume.
2. Existing opened boxes are checked before opening a new box.
3. For each package, all valid orientations are evaluated and the orientation that minimizes unused shelf space is selected.
4. The algorithm evaluates all existing shelves and places the package on the shelf that leaves the least remaining space (Best Fit).
5. If no shelf can accommodate the package, a new shelf is created if sufficient height remains.
6. If no existing box can accommodate the package, the smallest suitable shipping box is selected.
 
This heuristic keeps the implementation simple while demonstrating actual package packing rather than assigning every package to an independent box.
The implementation models each shipping box as a collection of shelves. Each shelf reserves a fixed width and height when created, and packages placed on that shelf consume the remaining shelf length. This provides a practical approximation of real-world packing without the complexity of full 3D bin packing.
---
 
# Assumptions
 
The implementation uses the following assumptions:
 
- Packages are processed using a First Fit Decreasing approach.
- Packages are packed using a Shelf Packing heuristic combined with Best Orientation and Best Shelf selection.
- Multiple packages may share the same shipping box.
- Packages on a shelf are arranged side-by-side along the remaining shelf length.
- Shelf width and height are fixed when a shelf is created and must accommodate all packages placed on that shelf.
- A new shelf is created only if sufficient height remains inside the box.
- Package rotation is supported by evaluating all six possible orientations.
- Weight constraints are always validated before placing a package.
- Shipping box definitions are loaded from configuration using the Options Pattern.
- If a package cannot fit into any available shipping box, a `PackageDoesNotFitException` is thrown.
 
> **Note**
>
> The objective of this implementation is to demonstrate a practical packing heuristic rather than solve the NP-hard 3D Bin Packing problem. The Shelf Packing approach provides a good balance between simplicity, maintainability and realistic packing behaviour.
 
---
 
# API Endpoint
 
## Calculate Required Boxes
 
**POST**
 
```
/api/Packing/calculate
```
 
---
 
## Sample Request
 
```json
{
  "packages": [
    {
      "id": 1,
      "width": 100,
      "height": 120,
      "length": 100,
      "weight": 500
    },
    {
      "id": 2,
      "width": 80,
      "height": 100,
      "length": 80,
      "weight": 300
    },
    {
      "id": 3,
      "width": 200,
      "height": 200,
      "length": 150,
      "weight": 1200
    }
  ]
}
```
 
---
 
## Sample Response
 
```json
{
  "boxesRequired": 2,
  "boxes": [
    {
      "boxName": "A",
      "packageIds": [
        1,
        2
      ]
    },
    {
      "boxName": "C",
      "packageIds": [
        3
      ]
    }
  ]
}
```
 
---
 
# Validation
 
The API validates:
 
- Package dimensions must be greater than zero.
- Package weight must be greater than zero.
- Package Id must be greater than zero.
- Request must contain at least one package.
 
Invalid requests automatically return **400 Bad Request** using FluentValidation.
 
---
 
# Running the Application
 
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Set **LogisticsPackingService.Api** as the startup project.
4. Run the application.
5. Open Swagger UI.
6. Test the `POST /api/Packing/calculate` endpoint.
 
---
 
# Running the Tests
 
Run all unit tests using:
 
```bash
dotnet test
```
 
or use **Visual Studio Test Explorer**.
 
---
 
# Testing
 
The solution includes tests covering:

- Packing Service orchestration
- Shelf Packing algorithm
- Multiple package packing scenarios
- Package rotation
- Smallest suitable box selection
- Weight constraint enforcement
- Invalid package scenarios
- Package validation
- Request validation
 
---
 
# Future Improvements
 
Potential future enhancements include:
 
- Replace the Shelf Packing heuristic with a complete 3D Bin Packing algorithm.
- Track exact package coordinates inside each box.
- Improve shelf optimization to minimize wasted space.
- Add integration tests.
- Persist the box catalog in a database.
- Add structured logging and telemetry.
 
---
 
# Design Decisions
 
- Clean Architecture separates business logic from infrastructure.
- The packing algorithm is abstracted behind `IPackingAlgorithm`, allowing future algorithms to be introduced without changing the application service.
- The packing algorithm combines First Fit Decreasing with Best Orientation and Best Shelf heuristics to improve box utilization while keeping the implementation simple and maintainable.
- The solution intentionally favors a clear, maintainable heuristic over a fully optimal packing algorithm, aligning with the assignment's focus on correctness, simplicity, and testability.
---
 
# Author
 
Developed as part of a technical assessment using ASP.NET Core, Clean Architecture and a Shelf Packing heuristic.
 