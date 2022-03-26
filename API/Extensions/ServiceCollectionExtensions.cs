using System.Text.Json.Serialization;
using DAL.Extensions;
using Logic.Extensions;

namespace API.Extensions {
	public static class ServiceCollectionExtensions {
		// Globaal onderstaande CORS policy gebruiken, alternatief is om een attribuut toe te voegen aan de controllers
		// [EnableCors("OpenPolicy")] https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0#enable-cors-with-attributes
		private static void ConfigureCors(this IServiceCollection services) {
			services.AddCors(options => {
				options.AddPolicy("OpenPolicy",
					builder => {
						builder.AllowAnyOrigin()
							   .AllowAnyMethod()
							   .AllowAnyHeader();
					}
				);
			});
		}

		public static void ConfigureServicelayer(this IServiceCollection services, string connectionString) {

			services.AddControllers().AddJsonOptions(options => {
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			});

			// Voor de CQRS handlers
			services.AddValidatedAutoMapper();

			// Toegang verlenen aan FE 
			services.ConfigureCors();

			// Mediation
			services.AddMediation();

			// DbContext 
			services.AddDataLayer(connectionString);
		}


	}
}
