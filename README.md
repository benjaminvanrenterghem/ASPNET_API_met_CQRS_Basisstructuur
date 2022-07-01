# ASP.NET API met CQRS basisfundering

Maakt gebruik van Entity Framework. Bevat nog geen eerste migration, deze kan als volgt aangemaakt worden:
- 1 - **Sql Server dient geinstalleerd en actief te zijn, voeg vervolgens de server toe in VS met default databank naam "BVRNET" (of wijzig de naam alvorens aanvang in appsettings.json)**
  - ![Howto](https://i.imgur.com/XJ879eS.png)
- 2 - **Console openen:** View → Package Manager Console
- 3 - **Selecteer de DAL (datalaag) uit de dropdown. Vervolgens kan de eerste migration aangemaakt en de databank geupdatet worden:** Add-Migration InitialCreate **gevolgd door** Update-Database

## Projecten
- 0: Frameworks
  - **Doel:** Een mogelijke plaats voor je eigen frameworks
  - Empty
- 0: xUnit
  - **Doel:** Een project voor je xUnit testen in onder te brengen
  - Empty
- 1: BL (Business layer)
  - 1a: Domain
    - **Doel:** Structuren definieren waar met Logic mee wordt omgesprongen
    - **Folders:** Abstract, Attributes, Exceptions, Interfaces, Model, Static
      - Model → DTO → Request/Response
      - Model → Messaging
  - 1b: Logic 
    - **Doel:** Logica en gedrag definieren, maakt gebruik van de Domain klassen. Ingedeeld volgens het CQRS patroon, samen met FluentValidation valideerders
    - **Folders:** Behaviors, Extensions, Mediated, Profiles
      - Behaviors → Validators → ForProfile
      - Mediated → Commands/Queries → Internal  
- 2: DAL (Data (access) layer) 
  - **Doel:** Repository implementaties, een verzameling van de aangemaakte migrations en een configurerende DbContext
  - **Folders:** Extensions, Migrations, Repositories
    - Repositories → Generics
    - Repositories → Specifics 
- 3: API (Service layer 1) 
  - **Doel:** Maakt gebruik van het Domain, de Logic en de DAL. Definieert endpoints in een ASP.NET controller en maakt voor verwerking van een verzoek gebruik van MediatR.
  - Folders: Properties, Controllers, Extensions 


## Todo
- Add test endpoint without auth requirements
- Add authentication (including expanding the model & a dedicated controller)
- Add validators where applicable
- Add unit tests where applicable
- ~~Add a map to the Request- & ResponseDTO mapping profile~~
- Add an alternative implementation for the IGenericRead- & WriteRepository using a different storage technology
- Add one ADO.NET illustrative example
- Add article link and remove unneeded info from the readme

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
- Microsoft.AspNetCore.Mvc
- Microsoft.AspNetCore.Mvc.Core
- System.IdentityModel.Tokens.Jwt
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Proxies
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Moq
- Swashbuckle.AspNetCore
- Swashbuckle.AspNetCore.Swagger
- xUnit
- xUnit.runner.visualstudio
- coverlet.collector
