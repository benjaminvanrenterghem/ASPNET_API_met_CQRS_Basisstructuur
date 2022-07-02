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
	public class GetSingleStageProfileQuery : IRequest<Response<StageProfileResponseDTO>> {
		public int Id { get; set; }
	}

	public class GetSingleProfileQueryHandler : IRequestHandler<GetSingleStageProfileQuery, Response<StageProfileResponseDTO>> {
		private readonly IGenericReadRepository<Domain.Model.StageProfile> _profileReadRepository;
		private readonly IMapper _mapper;

		public GetSingleProfileQueryHandler(IGenericReadRepository<Domain.Model.StageProfile> profileReadRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<StageProfileResponseDTO>> Handle(GetSingleStageProfileQuery request, CancellationToken cancellationToken) {

			return new Response<StageProfileResponseDTO>(
				_mapper.Map<StageProfileResponseDTO>(
					_profileReadRepository.GetById(request.Id)
				)
			);

		}
	}
}
