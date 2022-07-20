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

	// todo tests
	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<UserResponseDTO>> {
		private readonly IGenericReadRepository<User> _userReadRepository;
		private readonly IGenericWriteRepository<User> _userWriteRepository;
		private readonly IMapper _mapper;

		public UpdateUserCommandHandler(IGenericReadRepository<User> userReadRepository, IGenericWriteRepository<User> userWriteRepository, IMapper mapper) {
			_userReadRepository = userReadRepository;
			_userWriteRepository = userWriteRepository;
			_mapper = mapper;
		}

		// todo unit tests
		public async Task<Response<UserResponseDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
			var req = request.UserRequestDTO;
			var jwt = request.ParsedJwtToken;

			User? existingUser = _userReadRepository.GetAllWithLazyLoading(u => u.Id == (req.Id ?? -1)).FirstOrDefault();

			if (existingUser == null) {
				return new Response<UserResponseDTO>().AddError("User could not be found");
			}

			// Ook opgenomen in de validator
			if (!jwt.ClearanceLevels.Contains(ClearanceLevel.Management)) {
				if (existingUser.Id != jwt.UserId) {
					return new Response<UserResponseDTO>().AddError("Unpriviliged: you can not edit someone else's account");
				}
			}

			User updatedUser = _mapper.Map<User>(req);

			// User's StageProfiles blijven ongewijzigd, gebruiker dient UpdateStageProfileCommand te benuttigen om deze relatie te wijzigen
			updatedUser.Profiles = existingUser.Profiles;
			
			_userWriteRepository.Update(existingUser, updatedUser);
			_userWriteRepository.Save();

			// In ResponseDTOMappingProfile wordt door de mapper het geencrypteerde wachtwoord veld omgevormd naar een lege string, aangezien we dit om veiligheidsredenen niet wensen te retourneren.
			return new Response<UserResponseDTO>(
				_mapper.Map<UserResponseDTO>(
					existingUser
				)
			);

		}
	}
}
