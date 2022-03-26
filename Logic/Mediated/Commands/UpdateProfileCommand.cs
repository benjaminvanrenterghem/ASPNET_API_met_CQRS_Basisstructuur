using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Request;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using Logic.Mediated.Commands.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Mediated.Commands {

	public class UpdateProfileCommand : IRequest<Response<ProfileResponseDTO>> {
		public ProfileRequestDTO ProfileRequestDTO { get; set; }
	}

	public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Response<ProfileResponseDTO>> {
		public readonly IGenericReadRepository<Domain.Model.Profile> _profileReadRepository;
		private readonly IGenericWriteRepository<Domain.Model.Profile> _profileWriteRepository;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		public UpdateProfileCommandHandler(IGenericReadRepository<Domain.Model.Profile> profileReadRepository, IGenericWriteRepository<Domain.Model.Profile> profileWriteRepository, IMapper mapper, IMediator mediator) {
			_profileReadRepository = profileReadRepository;
			_profileWriteRepository = profileWriteRepository;
			_mapper = mapper;
			_mediator = mediator;
		}

		// todo Id is not null, positive, etc in validator
		public async Task<Response<ProfileResponseDTO>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken) {

			Domain.Model.Profile profile = _mapper.Map<Domain.Model.Profile>(request.ProfileRequestDTO);
			Domain.Model.Profile? existingProfile = _profileReadRepository.GetById(profile.Id ?? -1);

			if (existingProfile == null) {
				return new Response<ProfileResponseDTO>().AddError("No profile exists with this id");
			}

			// This profile is promoted to new primary
			if (profile.IsPrimary && !existingProfile.IsPrimary) {
				Response<bool> res = await _mediator.Send(
					new SetAllProfilesNonPrimaryCommand()
				);

				if (!res.Success) {
					return new Response<ProfileResponseDTO>().AddError("Failed to set all existing profiles to non-primary.");
				}
				// This profile is demoted, but that's not allowed in this manner.
			} else if (!profile.IsPrimary && existingProfile.IsPrimary) {
				return new Response<ProfileResponseDTO>().AddError("You have set this profile to be no longer primary, this is not allowed since it would leave no primary profile active, instead, promote a profile to be the new primary profile first.");
			}

			_profileWriteRepository.Clear();
			_profileWriteRepository.Update(profile);
			_profileWriteRepository.SaveAndClear();

			return new Response<ProfileResponseDTO>(_mapper.Map<ProfileResponseDTO>(profile));

		}
	}


}
