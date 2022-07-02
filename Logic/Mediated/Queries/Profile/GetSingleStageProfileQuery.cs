using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;

namespace Logic.Mediated.Queries.Profile {
	// todo revisit
	public class GetSingleStageProfileQuery : IRequest<Response<StageProfileResponseDTO>> {
		public int Id { get; set; }
	}

	public class GetSingleStageProfileQueryHandler : IRequestHandler<GetSingleStageProfileQuery, Response<StageProfileResponseDTO>> {
		private readonly IGenericReadRepository<StageProfile> _profileReadRepository;
		private readonly IMapper _mapper;

		public GetSingleStageProfileQueryHandler(IGenericReadRepository<StageProfile> profileReadRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<StageProfileResponseDTO>> Handle(GetSingleStageProfileQuery request, CancellationToken cancellationToken) {
			var res = _profileReadRepository.GetById(request.Id);

			if(res == null) {
				return new Response<StageProfileResponseDTO>().AddError("StageProfile could not be found");
			}

			return new Response<StageProfileResponseDTO>(
				_mapper.Map<StageProfileResponseDTO>(
					res
				)
			);

		}
	}
}
