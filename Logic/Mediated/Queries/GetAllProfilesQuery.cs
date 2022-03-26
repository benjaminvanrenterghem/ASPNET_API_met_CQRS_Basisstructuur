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

namespace Logic.Mediated.Queries {

	public class GetAllProfilesQuery : PaginatedRequest<Response<IEnumerable<ProfileResponseDTO>>> { }

	public class GetAllProfilesQueryHandler : IRequestHandler<GetAllProfilesQuery, Response<IEnumerable<ProfileResponseDTO>>> {
		private readonly IGenericReadRepository<Domain.Model.Profile> _profileReadRepository;
		private readonly IMapper _mapper;

		public GetAllProfilesQueryHandler(IGenericReadRepository<Domain.Model.Profile> profileReadRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<IEnumerable<ProfileResponseDTO>>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken) {

			return new PaginatedResponse<IEnumerable<ProfileResponseDTO>>(
				_mapper.Map<List<ProfileResponseDTO>>(
					_profileReadRepository.GetAll().Skip(request.Skip).Take(request.Take).ToList()
				),
				request.Page,
				request.PageSize,
				_profileReadRepository.GetAll().Count()
			);

		}
	}


}
