using FluentValidation;
using Logic.Mediated.Queries.Profile;

// todo test
namespace Logic.Behaviors.Validators.ForProfiles.Queries {
	public class GetSingleStageProfileQueryValidator : AbstractValidator<GetSingleStageProfileQuery> {
		public GetSingleStageProfileQueryValidator() {
			RuleFor(req => req.Id).InclusiveBetween(1, int.MaxValue);
		}
	}
}
