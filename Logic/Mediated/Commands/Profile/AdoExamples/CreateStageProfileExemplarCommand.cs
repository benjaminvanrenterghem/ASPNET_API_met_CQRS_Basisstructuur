using AutoMapper;
using Domain.Exceptions;
using Domain.Interfaces.Repositories.Generics;
using Domain.Interfaces.Repositories.Specifics;
using Domain.Model;
using Domain.Model.DTO.Request;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;
using Micro2Go.Model;

namespace Logic.Mediated.Commands.Profile {
	public class CreateStageProfileExemplarCommand : IRequest<Response<StageProfileResponseDTO>> {
		public StageProfileRequestDTO ProfileRequestDTO { get; set; }
		public ParsedJwtToken ParsedJwtToken { get; set; }
	}

	// todo tests
	public class CreateStageProfileExemplarCommandHandler : IRequestHandler<CreateStageProfileExemplarCommand, Response<StageProfileResponseDTO>> {
		private readonly IAdoExemplarStageProfileWriteRepository _profileWriteRepository;
		private readonly IMapper _mapper;

		public CreateStageProfileExemplarCommandHandler(IAdoExemplarStageProfileWriteRepository profileWriteRepository, IMapper mapper) {
			_profileWriteRepository = profileWriteRepository;
			_mapper = mapper;
		}

		public async Task<Response<StageProfileResponseDTO>> Handle(CreateStageProfileExemplarCommand request, CancellationToken cancellationToken) {
			var req = request.ProfileRequestDTO;
			var jwt = request.ParsedJwtToken;

			// Ook opgenomen in validator
			if (!jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				if (req.OwnerUserId != jwt.UserId) {
					return new Response<StageProfileResponseDTO>().AddError("Unpriviliged: JWT user id & provided dto owner id mismatch");
				}
			}

			StageProfile profile = _mapper.Map<StageProfile>(req);

			try {
				profile = _profileWriteRepository.AddStageProfile(profile);
			} catch (RepositoryException exc) {
				return new Response<StageProfileResponseDTO>().AddError("Failed to insert StageProfile")
															  .AddError(exc);
			}

			if (profile is null || profile.Id < 1) {
				return new Response<StageProfileResponseDTO>().AddError("Insert result is null");
			}

			return new Response<StageProfileResponseDTO>(
				_mapper.Map<StageProfileResponseDTO>(
					profile
				)
			);

		}
	}


}
