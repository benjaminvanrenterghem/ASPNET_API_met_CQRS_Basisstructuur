using FluentValidation;
using Logic.Mediated.Commands.Users;

namespace Logic.Behaviors.Validators.ForUsers {
	public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand> {
		// Geen ClearanceLevel validaties aangezien de endpoint afgeschermd is voor management
		public DeleteUserCommandValidator() {
			RuleFor(cmd => cmd.Id).InclusiveBetween(1, int.MaxValue);
		}
	}
}
