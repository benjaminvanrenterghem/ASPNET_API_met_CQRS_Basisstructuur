using FluentValidation;
using Logic.Mediated.Commands.Profile;

// todo test
namespace Logic.Behaviors.Validators.ForProfiles {
	public class DeleteStageProfileCommandValidator : AbstractValidator<DeleteStageProfileCommand> {
		public DeleteStageProfileCommandValidator() {
			RuleFor(cmd => cmd.Id).InclusiveBetween(1, int.MaxValue);
			RuleFor(cmd => cmd.ParsedJwtToken).NotNull();

			When(cmd => cmd.ParsedJwtToken != null, () => {
				RuleFor(cmd => cmd.ParsedJwtToken.Email).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.DisplayName).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.LoginName).NotEmpty();
				RuleFor(cmd => cmd.ParsedJwtToken.UserId).InclusiveBetween(1, int.MaxValue);
			});
		}
	}
}
