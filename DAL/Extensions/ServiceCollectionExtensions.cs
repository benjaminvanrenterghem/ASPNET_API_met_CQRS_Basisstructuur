using DAL.Repositories.Generics;
using Domain.Interfaces;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Extensions {
	public static class ServiceCollectionExtensions {
		public static void AddDataLayer(this IServiceCollection services, string connectionString) {

			services.AddDbContext<NetworkDbContext>(options => {
				options.EnableSensitiveDataLogging(true);
				options.UseLazyLoadingProxies();
				options.UseSqlServer(connectionString,
				sqlServerOptionsAction: sqlOptions => {
					sqlOptions.EnableRetryOnFailure(
							maxRetryCount: 3,
							maxRetryDelay: TimeSpan.FromSeconds(10),
							errorNumbersToAdd: null
					);
				});
			}, ServiceLifetime.Scoped);

			services.AddScoped<INetworkDbContext, NetworkDbContext>();



			// Explicitly registering dependencies can be seen as a benefit because you won’t get unwanted
			// surprises later at runtime from seemingly “magic” registrations.
			// This is especially true since your convention to register these types appears to be solely
			// based on interface naming or namespaces. I would recommend you to add some stronger
			// identifiers (e.g. common interfaces, or marker interfaces) if you want to utilize a
			// convention-based registration there. Otherwise, it may seem better to list every
			// single DI registration even if that may seem very verbose.
			//
			// Alternatief met scrutor en scan
			// https://stackoverflow.com/a/59520369/8623540

			// Generics
			services.AddScoped<IGenericReadRepository<Profile>, GenericReadRepository<Profile>>();

			services.AddScoped<IGenericWriteRepository<Profile>, GenericWriteRepository<Profile>>();

			// Specifics
		}

	}
}
