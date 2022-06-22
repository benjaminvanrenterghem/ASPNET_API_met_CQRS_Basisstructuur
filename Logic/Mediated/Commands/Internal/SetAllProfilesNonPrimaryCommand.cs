using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Mediated.Commands.Internal {
	public class SetAllProfilesNonPrimaryCommand : IRequest<Response<bool>> { }

	public class SetAllProfilesNonPrimaryCommandHandler : IRequestHandler<SetAllProfilesNonPrimaryCommand, Response<bool>> {
		public readonly IGenericReadRepository<Domain.Model.Profile> _profileReadRepository;
		public readonly IGenericWriteRepository<Domain.Model.Profile> _profileWriteRepository;
		private readonly IMapper _mapper;

		public SetAllProfilesNonPrimaryCommandHandler(IGenericReadRepository<Domain.Model.Profile> profileReadRepository, IGenericWriteRepository<Domain.Model.Profile> profileWriteRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_profileWriteRepository = profileWriteRepository;
			_mapper = mapper;
		}

		public async Task<Response<bool>> Handle(SetAllProfilesNonPrimaryCommand request, CancellationToken cancellationToken) {

			List<Domain.Model.Profile> profiles = _profileReadRepository.GetAll().Where(profile => profile.IsPrimary).ToList();

			foreach (var profile in profiles) {
				profile.IsPrimary = false;
				_profileWriteRepository.Update(profile);
			}

			_profileWriteRepository.Save();

			return new SuccessResponse();
		}
	}
}
