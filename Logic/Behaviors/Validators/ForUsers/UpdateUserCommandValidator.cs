using FluentValidation;
using Logic.Mediated.Commands.Users;
using Micro2Go.Model;

// todo test
namespace Logic.Behaviors.Validators.ForUsers {
	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand> {
		public UpdateUserCommandValidator() {
			RuleFor(cmd => cmd.UserRequestDTO).NotNull();
			RuleFor(cmd => cmd.ParsedJwtToken).NotNull();

			When(cmd => cmd.UserRequestDTO != null, () => {
				RuleFor(cmd => cmd.UserRequestDTO.Id).InclusiveBetween(1, int.MaxValue);
				RuleFor(cmd => cmd.UserRequestDTO.DisplayName).NotNull().NotEmpty().Length(1, 40);
				RuleFor(cmd => cmd.UserRequestDTO.LoginName).NotNull().NotEmpty().Length(1, 40);
				RuleFor(cmd => cmd.UserRequestDTO.Email).NotNull().NotEmpty().Length(1, 400);
				RuleFor(cmd => cmd.UserRequestDTO.Password).NotNull().NotEmpty().Length(10, 300);
			});

			When(cmd => cmd.ParsedJwtToken != null, () => {
				RuleFor(cmd => cmd.ParsedJwtToken.Email).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.DisplayName).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.LoginName).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.UserId).InclusiveBetween(1, int.MaxValue);
			});

			// Ook opgenomen in handler
			// Indien men niet tot management behoort
			When(cmd => cmd.UserRequestDTO != null && cmd.ParsedJwtToken != null, () => {
				When(cmd => !cmd.ParsedJwtToken.ClearanceLevels.Contains(ClearanceLevel.Management), () => {
					// Mag men louter het eigen account aanpassen
					RuleFor(cmd => cmd.ParsedJwtToken).Must((cmd, jwt) =>
						jwt.UserId == cmd.UserRequestDTO.Id
					);

					// Mag men zichzelf niet promoveren tot management
					RuleFor(cmd => cmd.UserRequestDTO).Must((cmd, userdto) =>
						!userdto.ClearanceLevels.Contains(ClearanceLevel.Management.ToString())
					);
				});
			});
		}
	
	}
}
