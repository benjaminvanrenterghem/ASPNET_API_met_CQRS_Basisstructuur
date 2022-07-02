using AutoMapper;
using Domain.Abstract;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using Logic.Extensions;
using MediatR;

namespace Logic.Mediated.Queries.Users {
	public class GetAllUsersQuery : PaginatedSearchRequest<Response<IEnumerable<UserResponseDTO>>> {}

	public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response<IEnumerable<UserResponseDTO>>> {
		private readonly IGenericReadRepository<User> _userReadRepository;
		private readonly IMapper _mapper;

		public GetAllUsersQueryHandler(IGenericReadRepository<User> userReadRepository, IMapper mapper) {
			_userReadRepository = userReadRepository;
			_mapper = mapper;
		}

		public async Task<Response<IEnumerable<UserResponseDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) {
			// todo controle clearancelevels values in resp dto
			return new PaginatedResponse<IEnumerable<UserResponseDTO>>(
					_mapper.Map<List<UserResponseDTO>>(
						_userReadRepository.GetAll().Search(request.Skip, request.Take, request.SearchPropertyName, request.SearchValue)
					)
				,
				request.Page,
				request.PageSize,
				_userReadRepository.GetAll().Count()
			);
		}


	}
}
