using AutoMapper;
using Domain.Abstract;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using Logic.Extensions;
using MediatR;

namespace Logic.Mediated.Queries.Profile {

	// todo revisit
	public class GetAllStageProfilesQuery : PaginatedSearchRequest<Response<IEnumerable<StageProfileResponseDTO>>> { }

	public class GetAllStageProfilesQueryHandler : IRequestHandler<GetAllStageProfilesQuery, Response<IEnumerable<StageProfileResponseDTO>>> {
		private readonly IGenericReadRepository<StageProfile> _profileReadRepository;
		private readonly IMapper _mapper;

		public GetAllStageProfilesQueryHandler(IGenericReadRepository<StageProfile> profileReadRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<IEnumerable<StageProfileResponseDTO>>> Handle(GetAllStageProfilesQuery request, CancellationToken cancellationToken) {

			return new PaginatedResponse<IEnumerable<StageProfileResponseDTO>>(
				_mapper.Map<List<StageProfileResponseDTO>>(
					_profileReadRepository.GetAll().Search(request.Skip, request.Take, request.SearchPropertyName, request.SearchValue)
				),
				request.Page,
				request.PageSize,
				_profileReadRepository.GetAll().Count()
			);

		}
	}


}
