using FluentValidation;
using Logic.Mediated.Commands.Profile;
using Micro2Go.Model;

// todo test
namespace Logic.Behaviors.Validators.ForProfiles {
	public class UpdateStageProfileCommandValidator : AbstractValidator<UpdateStageProfileCommand> {
		public UpdateStageProfileCommandValidator() {
			RuleFor(cmd => cmd.ParsedJwtToken).NotNull();
			RuleFor(cmd => cmd.ProfileRequestDTO).NotNull();

			When(cmd => cmd.ParsedJwtToken != null, () => {
				RuleFor(cmd => cmd.ParsedJwtToken.Email).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.DisplayName).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.LoginName).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.UserId).InclusiveBetween(1, int.MaxValue);
			});

			When(cmd => cmd.ProfileRequestDTO != null, () => {
				RuleFor(cmd => cmd.ProfileRequestDTO.Id).InclusiveBetween(1, int.MaxValue);
				RuleFor(cmd => cmd.ProfileRequestDTO.OwnerUserId).InclusiveBetween(1, int.MaxValue);
				RuleFor(cmd => cmd.ProfileRequestDTO.FullName).NotNull().NotEmpty();
				RuleFor(cmd => cmd.ProfileRequestDTO.About).NotNull().NotEmpty();
				RuleFor(cmd => cmd.ProfileRequestDTO.WebsiteURL).NotNull().NotEmpty();
				RuleFor(cmd => cmd.ProfileRequestDTO.Deleted).NotEqual(true);
			});

			// Ook opgenomen in handler
			When(cmd => cmd.ProfileRequestDTO != null && cmd.ParsedJwtToken != null, () => {
				When(cmd => !cmd.ParsedJwtToken.ClearanceLevels.Contains(ClearanceLevel.Management), () => {
					RuleFor(cmd => cmd.ParsedJwtToken).Must((cmd, jwt) =>
						jwt.UserId == cmd.ProfileRequestDTO.OwnerUserId
					);
				});
			});

		}

	}
}
