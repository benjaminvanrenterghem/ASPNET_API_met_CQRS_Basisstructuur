using System.Text;
using System.Text.Json.Serialization;
using DAL.Extensions;
using Domain.Static;
using Logic.Extensions;
using Micro2Go.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

		private static void ConfigureAuthentication(this IServiceCollection services, string jwtKey, string jwtIssuer, string jwtAudience, string jwtAuthority, string clearanceLevelManagement, string clearanceLevelUser) {
			services.AddAuthentication(x => {
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => {
				options.RequireHttpsMetadata = true;
				options.Audience = jwtAudience;
				options.Authority = jwtAuthority;
				options.TokenValidationParameters = new() {
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidAudience = jwtAudience,
					ValidIssuer = jwtIssuer,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
				};
			});

			services.AddAuthorization(options => {
				options.AddPolicy(ApiConfig.AuthorizedFor_Management, policy => {
					policy.RequireClaim(nameof(ClearanceLevel), clearanceLevelManagement);
				});
				options.AddPolicy(ApiConfig.AuthorizedFor_Shared, policy => {
					policy.RequireClaim(nameof(ClearanceLevel), new string[] { clearanceLevelManagement, clearanceLevelUser });
				});
				options.AddPolicy(ApiConfig.AuthorizedFor_Public, policy => { });
			});

		}

		public static void ConfigureServicelayer(this IServiceCollection services, IConfiguration configuration) {

			services.AddControllers().AddJsonOptions(options => {
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			});

			services.ConfigureAuthentication(
				configuration[ApiConfig.JWT_Key],
				configuration[ApiConfig.JWT_Issuer],
				configuration[ApiConfig.JWT_Audience],
				configuration[ApiConfig.JWT_Authority],
				configuration[ApiConfig.JWT_ClearanceLevels_Management],
				configuration[ApiConfig.JWT_ClearanceLevels_User]
			);

			// Toegang tot configuraties in controllers
			services.AddSingleton<IConfiguration>(configuration);

			// Voor de CQRS handlers
			services.AddValidatedAutoMapper();

			// Toegang verlenen aan FE 
			services.ConfigureCors();

			// Mediation
			services.AddMediation();

			// DbContext 
			services.AddDataLayer(
				configuration[ApiConfig.ConnectionStrings_Main]
			);
		}


	}
}
