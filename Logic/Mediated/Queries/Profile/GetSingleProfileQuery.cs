using AutoMapper;
using Domain.Abstract;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Mediated.Queries.Profile {
	// todo revisit
	public class GetSingleProfileQuery : IRequest<Response<ProfileResponseDTO>> {
		public int Id { get; set; }
	}

	public class GetSingleProfileQueryHandler : IRequestHandler<GetSingleProfileQuery, Response<ProfileResponseDTO>> {
		private readonly IGenericReadRepository<Domain.Model.Profile> _profileReadRepository;
		private readonly IMapper _mapper;

		public GetSingleProfileQueryHandler(IGenericReadRepository<Domain.Model.Profile> profileReadRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<ProfileResponseDTO>> Handle(GetSingleProfileQuery request, CancellationToken cancellationToken) {

			return new Response<ProfileResponseDTO>(
				_mapper.Map<ProfileResponseDTO>(
					_profileReadRepository.GetById(request.Id)
				)
			);

		}
	}
}
