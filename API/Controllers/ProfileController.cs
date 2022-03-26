using Domain.Model.DTO.Request;
using Domain.Model.Messaging;
using Domain.Static;
using Logic.Mediated.Commands;
using Logic.Mediated.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase {
		private readonly IMediator _mediator;

		public ProfileController(IMediator mediator) { 
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult> GetAllProfiles(int? page, int? pageSize) {
			try {
				return Ok(
					await _mediator.Send(
						new GetAllProfilesQuery () { Page = page ?? default, PageSize = pageSize ?? default }
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(ProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}	
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult> GetSingleProfile(int id) {
			try {
				return Ok(
					await _mediator.Send(
						new GetSingleProfileQuery() { Id = id }
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(ProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		[HttpGet("/profile/primary")] // todo nazicht path ok
		public async Task<ActionResult> GetPrimaryProfile() {
			try {
				return Ok(
					await _mediator.Send(
						new GetPrimaryProfileQuery() { }
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(ProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		// bij handlers voor zorgen dat bij create & update er steeds slechts 1 profile primary kan/mag zijn
		[HttpPost]
		public async Task<ActionResult> CreateProfile(ProfileRequestDTO profile) {
			try {
				return Ok(
					await _mediator.Send(
						new CreateProfileCommand() { ProfileRequestDTO = profile }
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(ProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateProfile(ProfileRequestDTO profile) {
			try {
				return Ok(
					await _mediator.Send(
						new UpdateProfileCommand() { ProfileRequestDTO = profile }
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(ProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		

	}
}
