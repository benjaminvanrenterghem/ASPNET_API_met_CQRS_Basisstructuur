using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.Messaging;
using Logic.Mediated.Commands.Profile;
using MediatR;
using Micro2Go.Model;

namespace Logic.Mediated.Commands.Users {

	public class DeleteUserCommand : IRequest<Response<bool>> {
		public int Id { get; set; }
	}

	// todo validator, tests
	public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<bool>> {
		private readonly IGenericReadRepository<User> _userReadRepository;
		private readonly IGenericWriteRepository<User> _userWriteRepository;
		private readonly IMediator _mediator;

		public DeleteUserCommandHandler(IGenericReadRepository<User> userReadRepository, IGenericWriteRepository<User> userWriteRepository, IMediator mediator) {
			_userReadRepository = userReadRepository;
			_userWriteRepository = userWriteRepository;
			_mediator = mediator;
		}

		public async Task<Response<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken) {
			var id = request.Id;

			var existingUser = _userReadRepository.GetAllWithLazyLoading(p => p.Id == id).FirstOrDefault();

			if (existingUser == null) {
				return new Response<bool>(false).AddError("User does not exist");
			}

			// Indien de User verwijdert wordt, worden zijn StageProfiles meegenomen.
			// Om herhaling van logica te vermijden wordt gebruik gemaakt van DeleteStageProfileCommand
			foreach (var stageProfile in existingUser.Profiles) {
				await _mediator.Send(
					new DeleteStageProfileCommand() {
						Id = (int)stageProfile.Id,
						ParsedJwtToken = new ParsedJwtToken() {
							DisplayName = existingUser.DisplayName,
							LoginName = existingUser.LoginName,
							Email = existingUser.Email,
							UserId = (int)existingUser.Id,
							ClearanceLevels = new () {
								ClearanceLevel.User,
								ClearanceLevel.Management
							}
						}
					}
				);
			}

			_userWriteRepository.SoftDelete(id);
			_userWriteRepository.Save();

			return new Response<bool>(true);
		}

	}
}
