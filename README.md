# CQRS Foundational base solution

(English at the bottom of the readme)

# Dutch

Een CQRS basisfundering om verdere ontwikkelingen mogelijk te maken. Bevat de basisstructuur en 1 (één) verticaal ontwikkelde case, waarbij gebruik wordt gemaakt van de projecten in de solution. Gebaseerd op een eenvoudige entity uit het domeinmodel (Profile), de controllerfuncties en bijbehorende commands & query's stellen CRUD-operaties voor ter illustratie van het gebruik en de samenhang.

Maakt gebruik van Entity Framework. Bevat nog geen eerste migration, deze kan als volgt aangemaakt worden:
- 1 - **Sql Server dient geinstalleerd en actief te zijn, voeg vervolgens de server toe in VS met default databank naam "BVRNET" (of wijzig de naam alvorens aanvang in appsettings.json)**
  - ![Howto](https://i.imgur.com/XJ879eS.png)
- 2 - **Console openen:** View → Package Manager Console
- 3 - **Selecteer de DAL (datalaag) uit de dropdown. Vervolgens kan de eerste migration aangemaakt en de databank geupdatet worden:** Add-Migration InitialCreate **gevolgd door** Update-Database

## Projecten
- 0: Frameworks
  - **Purpose:** Een mogelijke plaats voor je eigen frameworks
  - Empty
- 0: xUnit
  - **Purpose:** Een project voor je xUnit testen in onder te brengen
  - Empty
- 1: BL (Business layer)
  - 1a: Domain
    - **Purpose:** Structuren definieren waar met Logic mee wordt omgesprongen
    - **Folders:** Abstract, Attributes, Exceptions, Interfaces, Model, Static
      - Model → DTO → Request/Response
      - Model → Messaging
  - 1b: Logic 
    - **Purpose:** Logica en gedrag definieren, maakt gebruik van de Domain klassen. Ingedeeld volgens het CQRS patroon, samen met FluentValidation valideerders
    - **Folders:** Behaviors, Extensions, Mediated, Profiles
      - Behaviors → Validators → ForProfile
      - Mediated → Commands/Queries → Internal  
- 2: DAL (Data (access) layer) 
  - **Purpose:** Repository implementaties, een verzameling van de aangemaakte migrations en een configurerende DbContext
  - **Folders:** Extensions, Migrations, Repositories
    - Repositories → Generics
    - Repositories → Specifics 
- 3: API (Service layer 1) 
  - **Purpose:** Maakt gebruik van het Domain, de Logic en de DAL. Definieert endpoints in een ASP.NET controller en maakt voor verwerking van een verzoek gebruik van MediatR.
  - Folders: Properties, Controllers, Extensions 

De todo lijst en het dependency overzicht zijn onderaan de pagina terug te vinden.



# English

A foundational CQRS base solution to build upon.
Contains the foundational structure and 1 (one) vertically developed example, leveraging the important projects.

Based on a simple **Profile** entity from the Domain model, controller functions & accompanying commands and queries are standard CRUD operations exemplified.
Uses Entity Framework, doesn't contain a migration, create your own by:
- 1 - **Having Sql Server up and running & adding default database "BVRNET" (or change the name in appsettings.json)**
  - ![Howto](https://i.imgur.com/XJ879eS.png)
- 2 - **Opening console** View → Package Manager Console
- 3 - **Select the DAL (datalayer) from the dropdown, then perform following commands** Add-Migration InitialCreate **followed by** Update-Database

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
- ~~Add a map to the Request- & ResponseDTO mapping profile~~
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
