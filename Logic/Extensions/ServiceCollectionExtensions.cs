using AutoMapper;
using FluentValidation;
using Logic.Behaviors;
using Logic.Profiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Extensions {
	public static class ServiceCollectionExtensions {
		public static void AddValidatedAutoMapper(this IServiceCollection services) {
			new MapperConfiguration(cfg => {
				cfg.AddProfile<ResponseDTOMappingProfile>();
			}).AssertConfigurationIsValid();

			services.AddAutoMapper(typeof(ResponseDTOMappingProfile));
		}

		public static void AddMediation(this IServiceCollection services) {
			services.AddMediatR(Assembly.GetExecutingAssembly());

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Scoped);

			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
		}

	}
}
