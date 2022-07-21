using FluentValidation;
using Logic.Mediated.Commands.Authentication;

// todo test
namespace Logic.Behaviors.Validators.ForAuthentication {
	public class CreateJWTTokenCommandValidator : AbstractValidator<CreateJWTTokenCommand> {
		public CreateJWTTokenCommandValidator() {
			RuleFor(cmd => cmd.Subject).NotNull().NotEmpty();
			RuleFor(cmd => cmd.Key).NotNull().NotEmpty();
			RuleFor(cmd => cmd.Issuer).NotNull().NotEmpty();
			RuleFor(cmd => cmd.Audience).NotNull().NotEmpty();
			RuleFor(cmd => cmd.TokenValidForSeconds).GreaterThan(300);

			RuleFor(cmd => cmd.ProvidedCredentials).NotNull();
			When(cmd => cmd.ProvidedCredentials != null, () => {
				RuleFor(cmd => cmd.ProvidedCredentials.Email).NotNull().NotEmpty().Length(1, 400);
				RuleFor(cmd => cmd.ProvidedCredentials.Password).NotNull().NotEmpty().Length(10, 300);
			});
		}


	}
}
