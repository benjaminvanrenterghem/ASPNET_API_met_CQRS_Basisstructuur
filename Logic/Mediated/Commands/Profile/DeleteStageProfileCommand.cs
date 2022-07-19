using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.Messaging;
using MediatR;
using Micro2Go.Model;

namespace Logic.Mediated.Commands.Profile {
	public class DeleteStageProfileCommand : IRequest<Response<bool>> {
		public int Id { get; set; }
		public ParsedJwtToken ParsedJwtToken { get; set; }
	}

	// todo in alle handlers nagaan of er per ongeluk in de handler public props zijn
	public class DeleteStageProfileCommandHandler : IRequestHandler<DeleteStageProfileCommand, Response<bool>> {
		private readonly IGenericReadRepository<StageProfile> _profileReadRepository;
		private readonly IGenericWriteRepository<StageProfile> _profileWriteRepository;

		public DeleteStageProfileCommandHandler(IGenericReadRepository<StageProfile> profileReadRepository, IGenericWriteRepository<StageProfile> profileWriteRepository) {
			_profileReadRepository = profileReadRepository;
			_profileWriteRepository = profileWriteRepository;
		}

		public async Task<Response<bool>> Handle(DeleteStageProfileCommand request, CancellationToken cancellationToken) {
			var id = request.Id;
			var jwt = request.ParsedJwtToken;

			var existingProfile = _profileReadRepository.GetAll(p => p.Id == id).FirstOrDefault();

			if(existingProfile == null) {
				return new Response<bool>(false).AddError("StageProfile does not exist");
			}

			// Niet opgenomen in validator aangezien er geen sprake is van een dto
			if (!jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				if(existingProfile.OwnerId != jwt.UserId) {
					return new Response<bool>(false).AddError("Unpriviliged: you can not update someone else's StageProfile");
				}
			}

			_profileWriteRepository.SoftDelete(id);
			_profileWriteRepository.Save();

			return new Response<bool>(true);
		}

	}
}
