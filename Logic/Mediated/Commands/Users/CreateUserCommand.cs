using AutoMapper;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Request;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using MediatR;
using Micro2Go.Extensions;
using Micro2Go.Model;

namespace Logic.Mediated.Commands.Users {
	public class CreateUserCommand : IRequest<Response<UserResponseDTO>> {
		public ParsedJwtToken ParsedJwtToken { get; set; }
		public UserRequestDTO UserRequestDTO { get; set; }
	}

	// todo tests
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<UserResponseDTO>> {
		private readonly IGenericReadRepository<User> _userReadRepository;
		private readonly IGenericWriteRepository<User> _userWriteRepository;
		private readonly IMapper _mapper;

		public CreateUserCommandHandler(IGenericReadRepository<User> userReadRepository, IGenericWriteRepository<User> userWriteRepository, IMapper mapper) {
			_userReadRepository = userReadRepository;
			_userWriteRepository = userWriteRepository;
			_mapper = mapper;
		}

		public async Task<Response<UserResponseDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
			var req = request.UserRequestDTO;
			var jwt = request.ParsedJwtToken;

			var existingUser = _userReadRepository.GetAll()
												  .Where(u => u.Email == req.Email
														   || u.LoginName == req.LoginName
														   || u.DisplayName == req.DisplayName)
												  .FirstOrDefault();

			if (existingUser != null) {
				return new Response<UserResponseDTO>().AddError("Email, login name or display name are already taken");
			}

			// Indien het een create betreft door management wordt tevens gebruik gemaakt van de meegegeven
			// clearance levels
			List<ClearanceLevel> clearanceLevels = new();

			if (jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				try {
					clearanceLevels = req.ClearanceLevels.ConvertAll(cl => Enum.Parse<ClearanceLevel>(cl));
				} catch {
					return new Response<UserResponseDTO>().AddError("One or more invalid ClearanceLevels were given, provide the string or numerical representation of the CL");
				}
			}

			if (!clearanceLevels.Contains(ClearanceLevel.User)) {
				clearanceLevels.Add(ClearanceLevel.User);
			}

			var user = new User(req.DisplayName, req.LoginName, req.Email, req.Password.GetSHA256String(), DateTime.Now, clearanceLevels);

			_userWriteRepository.Insert(user);
			_userWriteRepository.Save();

			return new Response<UserResponseDTO>(
				_mapper.Map<UserResponseDTO>(
					user
				)
			);
		}


	}
}
