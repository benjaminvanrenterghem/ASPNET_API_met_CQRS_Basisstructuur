using FluentValidation;
using Logic.Mediated.Commands.Users;
using Micro2Go.Model;

// todo test
namespace Logic.Behaviors.Validators.ForUsers {
	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
		public CreateUserCommandValidator() {
			RuleFor(cmd => cmd.UserRequestDTO).NotNull();

			// JWT Parsing van de HttpRequest dient de resulteren in een null-waarde (niet ingelogd)
			// of een create door een management user
			When(cmd => cmd.ParsedJwtToken != null, () => {
				RuleFor(cmd => cmd.ParsedJwtToken.ClearanceLevels).Must((cmd, jwt) =>
					jwt.Contains(ClearanceLevel.Management)
				);
			});

			// Zie tevens update validator
			When(cmd => cmd.UserRequestDTO != null, () => {
				RuleFor(cmd => cmd.UserRequestDTO.Id).Null();
				RuleFor(cmd => cmd.UserRequestDTO.DisplayName).NotNull().NotEmpty().Length(1, 40);
				RuleFor(cmd => cmd.UserRequestDTO.LoginName).NotNull().NotEmpty().Length(1, 40);
				RuleFor(cmd => cmd.UserRequestDTO.Email).NotNull().NotEmpty().Length(1, 400);

				// Minimum wachtwoord lengte van 10
				RuleFor(cmd => cmd.UserRequestDTO.Password).NotNull().NotEmpty().Length(10, 300);
			});
		}
	}
}
