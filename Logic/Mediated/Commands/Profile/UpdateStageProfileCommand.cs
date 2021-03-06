using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Request;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;
using Micro2Go.Model;

namespace Logic.Mediated.Commands.Profile {
	public class UpdateStageProfileCommand : IRequest<Response<StageProfileResponseDTO>> {
		public StageProfileRequestDTO ProfileRequestDTO { get; set; }
		public ParsedJwtToken ParsedJwtToken { get; set; }
	}

	public class UpdateStageProfileCommandHandler : IRequestHandler<UpdateStageProfileCommand, Response<StageProfileResponseDTO>> {
		private readonly IGenericReadRepository<User> _userReadRepository;
		private readonly IGenericReadRepository<StageProfile> _profileReadRepository;
		private readonly IGenericWriteRepository<StageProfile> _profileWriteRepository;
		private readonly IMapper _mapper;

		public UpdateStageProfileCommandHandler(IGenericReadRepository<User> userReadRepository, IGenericReadRepository<StageProfile> profileReadRepository, IGenericWriteRepository<StageProfile> profileWriteRepository, IMapper mapper) {
			_userReadRepository = userReadRepository;
			_profileReadRepository = profileReadRepository;
			_profileWriteRepository = profileWriteRepository;
			_mapper = mapper;
		}

		// todo unit tests
		public async Task<Response<StageProfileResponseDTO>> Handle(UpdateStageProfileCommand request, CancellationToken cancellationToken) {
			var req = request.ProfileRequestDTO;
			var jwt = request.ParsedJwtToken;

			StageProfile? existingProfile = _profileReadRepository.GetAllWithLazyLoading(p => p.Id == (req.Id ?? -1)).FirstOrDefault();

			if (existingProfile == null) {
				return new Response<StageProfileResponseDTO>().AddError("StageProfile could not be found");
			}

			// Clearance check ook opgenomen in validator
			if (!jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				if (existingProfile.OwnerId != jwt.UserId) {
					return new Response<StageProfileResponseDTO>().AddError("Unpriviliged: you are not the owner of this profile");
				}
			} else {
				var aspiringOwner = _userReadRepository.GetById(req.OwnerUserId);

				if(aspiringOwner == null) {
					return new Response<StageProfileResponseDTO>().AddError("Newly appointed owner does not exist");
				}
			}

			// UpdatedProfile zal niet beschikken over een Owner instantie, echter door opname van 
			// int OwnerId in StageProfile zal deze correct ingesteld worden (zie StageProfile)
			StageProfile updatedProfile = _mapper.Map<StageProfile>(req);
			_profileWriteRepository.Update(existingProfile, updatedProfile);
			_profileWriteRepository.Save();

			return new Response<StageProfileResponseDTO>(
				_mapper.Map<StageProfileResponseDTO>(
					existingProfile
				)
			);

		}
	}


}
