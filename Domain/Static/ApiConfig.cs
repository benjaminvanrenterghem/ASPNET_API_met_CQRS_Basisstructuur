using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Static {
	public static class ApiConfig {
		// Appsettings.json paths, indien er een path wijzigt dient men louter hier wijzigingen aan te brengen.
		public static string ConnectionStrings_Main => "ConnectionStrings:Main";
		public static string JWT_Key => "Jwt:Key";
		public static string JWT_Issuer => "Jwt:Issuer";
		public static string JWT_Audience => "Jwt:Audience";
		public static string JWT_Authority => "Jwt:Authority";
		public static string JWT_Subject => "Jwt:Subject";
		public static string JWT_ClearanceLevels_Management => "Jwt:ClearanceLevels:Management";
		public static string JWT_ClearanceLevels_User => "Jwt:ClearanceLevels:User";

		// Voor definiering in ConfigureAuthentication 
		// Voor gebruik in Authorize attributen, e.g. [Authorize(ApiConfig.AuthorizedFor_Management)]
		// Dient steeds een compile-time "const" te zijn om gebruik in de attributen als argument mogelijk te maken
		public const string AuthorizedFor_Management = "Management";
		public const string AuthorizedFor_Shared = "Shared";
		public const string AuthorizedFor_Public = "Public";

		// Wordt gebruikt in de controllers indien de FallbackResponse van kracht is.
		// FallbackResponse content kan indien gewenst aangepast worden, deze string separator laat eenvoudige splitting/parsing toe in de frontend.

		public static string ExcSeparator = "::||::";

		// Voor queries, indien niet meegegeven als parameter bij een GET request
		public static int DefaultPage = 1;
		public static int DefaultPageSize = 100;
	}
}
