using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Request;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;
using Micro2Go.Model;

namespace Logic.Mediated.Commands.Profile {
	public class CreateStageProfileCommand : IRequest<Response<StageProfileResponseDTO>> {
		public StageProfileRequestDTO ProfileRequestDTO { get; set; }
		public ParsedJwtToken ParsedJwtToken { get; set; }
	}

	// todo validator, tests
	public class CreateStageProfileCommandHandler : IRequestHandler<CreateStageProfileCommand, Response<StageProfileResponseDTO>> {
		private readonly IGenericWriteRepository<StageProfile> _profileWriteRepository;
		private readonly IMapper _mapper;

		public CreateStageProfileCommandHandler(IGenericWriteRepository<StageProfile> profileWriteRepository, IMapper mapper) {
			_profileWriteRepository = profileWriteRepository;
			_mapper = mapper;
		}

		public async Task<Response<StageProfileResponseDTO>> Handle(CreateStageProfileCommand request, CancellationToken cancellationToken) {
			var req = request.ProfileRequestDTO;
			var jwt = request.ParsedJwtToken;

			if (!jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				if (req.OwnerUserId != jwt.UserId) {
					return new Response<StageProfileResponseDTO>().AddError("Unpriviliged: JWT user id & provided dto owner id mismatch");
				}
			}

			StageProfile profile = new StageProfile(req.FullName, jwt.UserId, req.About, req.WebsiteURL);

			_profileWriteRepository.Insert(profile);
			_profileWriteRepository.Save();

			return new Response<StageProfileResponseDTO>(
				_mapper.Map<StageProfileResponseDTO>(
					profile
				)
			);

		}
	}


}
