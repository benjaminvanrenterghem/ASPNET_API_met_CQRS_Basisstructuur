using FluentValidation;
using Logic.Mediated.Queries.Users;

// todo test
namespace Logic.Behaviors.Validators.ForUsers.Queries {
	public class GetSingleUserQueryValidator : AbstractValidator<GetSingleUserQuery> {
		public GetSingleUserQueryValidator() {
			RuleFor(req => req.Id).InclusiveBetween(1, int.MaxValue);
		}
	}
}
