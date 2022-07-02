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
	public class UpdateUserCommand : IRequest<Response<UserResponseDTO>> {
		public UserRequestDTO UserRequestDTO { get; set; }
		public ParsedJwtToken ParsedJwtToken { get; set; }
	}

	// todo validators, tests
	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<UserResponseDTO>> {
		private readonly IGenericReadRepository<User> _userReadRepository;
		private readonly IGenericWriteRepository<User> _userWriteRepository;
		private readonly IMapper _mapper;

		public UpdateUserCommandHandler(IGenericReadRepository<User> userReadRepository, IGenericWriteRepository<User> userWriteRepository, IMapper mapper) {
			_userReadRepository = userReadRepository;
			_userWriteRepository = userWriteRepository;
			_mapper = mapper;
		}

		// todo validator, unit tests
		public async Task<Response<UserResponseDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
			var req = request.UserRequestDTO;
			var jwt = request.ParsedJwtToken;

			User? existingUser = _userReadRepository.GetAllWithLazyLoading(u => u.Id == (req.Id ?? -1)).FirstOrDefault();

			if (existingUser == null) {
				return new Response<UserResponseDTO>().AddError("User could not be found");
			}

			if (!jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				if (existingUser.Id != jwt.UserId) {
					return new Response<UserResponseDTO>().AddError("Unpriviliged: you can not edit someone else's account");
				}
			}

			existingUser.DisplayName = req.DisplayName;
			existingUser.LoginName = req.LoginName;
			existingUser.Email = req.Email;
			existingUser.Password = req.Password.GetSHA256String();

			// Louter management mag een User's ClearanceLevels wijzigen
			if (jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				List<ClearanceLevel> clevels = new();

				try {
					req.ClearanceLevels.ForEach(cl => {
						clevels.Add(Enum.Parse<ClearanceLevel>(cl.ToString()));
					});
				} catch {
					return new Response<UserResponseDTO>().AddError("One or more invalid ClearanceLevels were given, provide the string or numerical representation of the CL");
				}

				existingUser.ClearanceLevels = clevels;
			}

			// User's StageProfiles blijven ongewijzigd, gebruiker dient UpdateStageProfileCommand te benuttigen om deze relatie te wijzigen

			_userWriteRepository.Update(existingUser);
			_userWriteRepository.Save();

			// We wensen om veiligheidsredenen een response te returnen welke het geencrypteerde wachtwoord niet bevat
			existingUser.Password = "";

			return new Response<UserResponseDTO>(
				_mapper.Map<UserResponseDTO>(
					existingUser
				)
			);

		}
	}
}
