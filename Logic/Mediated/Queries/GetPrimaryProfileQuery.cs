using AutoMapper;
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
	public class GetPrimaryProfileQuery : IRequest<Response<ProfileResponseDTO>> { }

	public class GetPrimaryProfileQueryHandler : IRequestHandler<GetPrimaryProfileQuery, Response<ProfileResponseDTO>> {
		private readonly IGenericReadRepository<Domain.Model.Profile> _profileReadRepository;
		private readonly IMapper _mapper;

		public GetPrimaryProfileQueryHandler(IGenericReadRepository<Domain.Model.Profile> profileReadRepository, IMapper mapper) {
			_profileReadRepository = profileReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<ProfileResponseDTO>> Handle(GetPrimaryProfileQuery request, CancellationToken cancellationToken) {

			var res = _profileReadRepository.GetAll(x => x.IsPrimary).FirstOrDefault();

			if (res == null) {
				return new Response<ProfileResponseDTO>(
					_mapper.Map<ProfileResponseDTO>(
						_profileReadRepository.GetAll().FirstOrDefault()
					)	
				).AddError("It's expected to find a primary profile but none was found, attempting to return a first occurrence instead as a fallback, as not to have no data to populate the UI with.");
			}

			return new Response<ProfileResponseDTO>(
				_mapper.Map<ProfileResponseDTO>(
					res
				)
			);

		}


	}


}
