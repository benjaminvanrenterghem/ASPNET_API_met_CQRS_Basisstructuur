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
	public class GetAllStageProfilesQuery : PaginatedSearchRequest<Response<IEnumerable<StageProfileResponseDTO>>> { }

	public class GetAllProfilesQueryHandler : IRequestHandler<GetAllStageProfilesQuery, Response<IEnumerable<StageProfileResponseDTO>>> {
		private readonly IGenericReadRepository<Domain.Model.StageProfile> _profileReadRepository;
		private readonly IMapper _mapper;

		public GetAllProfilesQueryHandler(IGenericReadRepository<Domain.Model.StageProfile> profileReadRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<IEnumerable<StageProfileResponseDTO>>> Handle(GetAllStageProfilesQuery request, CancellationToken cancellationToken) {

			return new PaginatedResponse<IEnumerable<StageProfileResponseDTO>>(
				_mapper.Map<List<StageProfileResponseDTO>>(
					_profileReadRepository.GetAll().Skip(request.Skip).Take(request.Take).ToList()
				),
				request.Page,
				request.PageSize,
				_profileReadRepository.GetAll().Count()
			);

		}
	}


}
