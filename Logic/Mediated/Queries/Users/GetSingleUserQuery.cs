using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;

namespace Logic.Mediated.Queries.Users {
	public class GetSingleUserQuery : IRequest<Response<UserResponseDTO>> {
		public int Id { get; set; }
	}

	public class GetSingleUserQueryHandler : IRequestHandler<GetSingleUserQuery, Response<UserResponseDTO>> {
		private readonly IGenericReadRepository<User> _userReadRepository;
		private readonly IMapper _mapper;

		public GetSingleUserQueryHandler(IGenericReadRepository<User> userReadRepository, IMapper mapper) {
			_userReadRepository = userReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<UserResponseDTO>> Handle(GetSingleUserQuery request, CancellationToken cancellationToken) {
			var res = _userReadRepository.GetById(request.Id);

			if (res == null) {
				return new Response<UserResponseDTO>().AddError("User could not be found");
			}

			return new Response<UserResponseDTO>(
				_mapper.Map<UserResponseDTO>(
					res
				)
			);
		}


	}
}
