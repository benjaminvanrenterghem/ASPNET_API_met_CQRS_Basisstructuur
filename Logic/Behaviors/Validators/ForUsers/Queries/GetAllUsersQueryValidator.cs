using FluentValidation;
using Logic.Mediated.Queries.Users;

// todo test
namespace Logic.Behaviors.Validators.ForUsers.Queries {
	public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery> {
		public GetAllUsersQueryValidator() {
			RuleFor(req => req.Page).InclusiveBetween(1, int.MaxValue);
			RuleFor(req => req.PageSize).InclusiveBetween(1, int.MaxValue);
		}
	}
}
