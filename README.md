# CQRS_Base

A foundational CQRS base solution to build upon.
Contains the foundational structure and 1 (one) vertically developed example, leveraging the important projects.

Based on a simple **Profile** entity from the Domain model, controller functions and commands and queries are standard CRUD operations exemplified.
Uses Entity Framework, doesn't contain a migration, create your own by:
- 1 - **Having Sql Server up and running & adding default database "BVRNET" (or change the name in appsettings.json)**
  - ![Howto](https://i.imgur.com/XJ879eS.png)
- 2 - **Opening** View → Package Manager Console
- 3 - **Executing** Add-Migration InitialCreate **and** Update-Database

## Projects
- 0: Frameworks
  - **Purpose:** A place to put frameworks you may want to make
  - Empty
- 0: xUnit
  - **Purpose:** A place to put your xunit tests
  - Empty
- 1: BL (Business layer)
  - 1a: Domain
    - **Purpose:** Defining structures on which logic is applied  
    - **Folders:** Abstract, Attributes, Exceptions, Interfaces, Model, Static
      - Model → DTO → Request/Response
      - Model → Messaging
  - 1b: Logic 
    - **Purpose:** Defining logic & behavior, uses the domain objects, CQRS pattern with FluentValidation validators
    - **Folders:** Behaviors, Extensions, Mediated, Profiles
      - Behaviors → Validators → ForProfile
      - Mediated → Commands/Queries → Internal  
- 2: DAL (Data (access) layer) 
  - **Purpose:** Repository implementations, collecting migrations, configuring dbcontext 
  - **Folders:** Extensions, Migrations, Repositories
    - Repositories → Generics
    - Repositories → Specifics 
- 3: API (Service layer 1) 
  - **Purpose:** Leverages BL & DAL, defines endpoints, uses MediatR
  - Folders: Properties, Controllers, Extensions 



## Todo
- Add validators for the scope, solution currently contains none.
- Add unit tests for the scope, solution currently contains none.
- Add a map to the Request- & ResponseDTO mapping profile
- Add an alternative implementation for the IGenericRead- & WriteRepository using a different storage technology

## Dependencies
- .NET 6
- Automapper
- Automapper.Extensions.Microsoft.DependencyInjection
- FluentValidation
- FluentValidation.AspNetCore
- FluentValidation.DependencyInjectionExtensions
- MediatR
- MediatR.Extensions.FluentValidation.AspNetCore
- MediatR.Extensions.Microsoft.DependencyInjection
- Microsoft.AspNetCore.Cors
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Moq
- Swashbuckle.AspNetCore
- Swashbuckle.AspNetCore.Swagger
- xUnit
- xUnit.runner.visualstudio
- coverlet.collector

