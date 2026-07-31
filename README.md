# Logistics Packing Service
 
## Overview
 
Logistics Packing Service is an ASP.NET Core Web API that calculates the number of shipping boxes required for a collection of packages.
 
The application calculates the number of shipping boxes required for a collection of packages. Packages are processed in descending order of volume and each package is assigned to the smallest available shipping box that satisfies its dimension and weight constraints.
---
 
## Solution Architecture
 
The solution follows the principles of Clean Architecture and is divided into the following projects:
 
### LogisticsPackingService.Api
- API endpoints
- Swagger/OpenAPI configuration
- Global exception handling
- Dependency injection configuration
 
### LogisticsPackingService.Application
- Business logic
- DTOs
- Service interfaces
- FluentValidation validators
 
### LogisticsPackingService.Domain
- Domain entities
- Value objects
- Domain exceptions
 
### LogisticsPackingService.Infrastructure
- Box catalog provider
- Configuration using the Options Pattern
- Dependency Injection registrations
 
### LogisticsPackingService.Tests
- Unit tests for PackingService
- Validator tests
 
---
 
## Technologies Used
 
- .NET 10
- ASP.NET Core Web API
- FluentValidation
- xUnit
- Moq
- FluentAssertions
- Swagger / OpenAPI
 
---
 
## Assumptions
 
The implementation uses the following assumptions:
 
- Each package is assigned to a single shipping box.
- Packages are processed in descending order of volume.
- The smallest available box that satisfies both dimension and weight constraints is selected.
- Package rotation is supported by evaluating all possible orientations.
- If a package cannot fit into any available box, a `PackageDoesNotFitException` is thrown.
- Shipping box definitions are loaded from configuration using the Options Pattern.
 
**Note:**  
The assignment allowed a simple packing heuristic. Therefore, a one-package-per-box strategy was intentionally chosen to keep the implementation simple, maintainable, and easy to understand.
---
 
## API Endpoint
 
### Calculate Required Boxes
 
**POST**
 
```
/api/Packing/calculate
```
 
### Sample Request
 
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
      "width": 150,
      "height": 150,
      "length": 100,
      "weight": 800
    }
  ]
}
```
 
### Sample Response
 
```json
{
  "boxesRequired": 2
}
```
 
---

## Running the Application
 
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Set **LogisticsPackingService.Api** as the startup project.
4. Run the application using the **HTTPS** launch profile.
5. Open the Swagger UI using the HTTPS URL displayed by Visual Studio (for example, `https://localhost:7001/swagger`).
6. Use the `POST /api/Packing/calculate` endpoint to test the API.
 
---
 
## Running the Tests
 
Run all unit tests using:
 
```
dotnet test
```
 
or use **Visual Studio Test Explorer**.
 
---
 
## Testing
 
The solution includes unit tests covering:
 
- Packing service business logic
- Package validation
- Request validation
 
---
 
## Future Improvements
 
Potential enhancements include:
 
- Pack multiple packages into a single box.
- Implement an optimized bin-packing algorithm.
- Add integration tests.
- Persist the box catalog in a database.
- Add logging and telemetry.
 
---
 
## Author
 
Developed as part of a technical assessment using ASP.NET Core and Clean Architecture.
 