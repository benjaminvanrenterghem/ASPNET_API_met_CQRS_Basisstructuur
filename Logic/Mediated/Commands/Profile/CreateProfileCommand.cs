using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Request;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Mediated.Commands.Profile {
	public class CreateProfileCommand : IRequest<Response<ProfileResponseDTO>> {
		public ProfileRequestDTO ProfileRequestDTO { get; set; }
	}

	// todo revise
	public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Response<ProfileResponseDTO>> {
		public readonly IGenericReadRepository<Domain.Model.Profile> _profileReadRepository;
		public readonly IGenericWriteRepository<Domain.Model.Profile> _profileWriteRepository;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		public CreateProfileCommandHandler(IGenericReadRepository<Domain.Model.Profile> profileReadRepository, IGenericWriteRepository<Domain.Model.Profile> profileWriteRepository, IMapper mapper, IMediator mediator) {
			_profileReadRepository = profileReadRepository;
			_profileWriteRepository = profileWriteRepository;
			_mapper = mapper;
			_mediator = mediator;
		}

		public async Task<Response<ProfileResponseDTO>> Handle(CreateProfileCommand request, CancellationToken cancellationToken) {

			Domain.Model.Profile profile = _mapper.Map<Domain.Model.Profile>(request.ProfileRequestDTO);

			if (profile.IsPrimary) {
				Response<bool> res = await _mediator.Send(
					new SetAllProfilesNonPrimaryCommand()
				);

				if (!res.Success) {
					return new Response<ProfileResponseDTO>().AddError("Failed to set all existing profiles to non-primary.");
				}
			}

			_profileWriteRepository.Insert(profile);
			_profileWriteRepository.Save();

			return new Response<ProfileResponseDTO>(_mapper.Map<ProfileResponseDTO>(profile));

		}
	}


}
