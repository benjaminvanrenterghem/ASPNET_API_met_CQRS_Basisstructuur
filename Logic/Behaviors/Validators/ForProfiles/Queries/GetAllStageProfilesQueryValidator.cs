using FluentValidation;
using Logic.Mediated.Queries.Profile;

// todo test
namespace Logic.Behaviors.Validators.ForProfiles.Queries {
	public class GetAllStageProfilesQueryValidator : AbstractValidator<GetAllStageProfilesQuery> {
		public GetAllStageProfilesQueryValidator() {
			RuleFor(req => req.Page).InclusiveBetween(1, int.MaxValue);
			RuleFor(req => req.PageSize).InclusiveBetween(1, int.MaxValue);
		}
	}
}
