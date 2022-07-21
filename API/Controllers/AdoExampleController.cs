using Domain.Model.DTO.Request;
using Domain.Model.DTO.Response;
using Domain.Model.Messaging;
using Domain.Static;
using Logic.Mediated.Commands.Profile;
using MediatR;
using Micro2Go.Parsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdoExampleController : ControllerBase {
	private IMediator _mediator;

	public AdoExampleController(IMediator mediator) {
		_mediator = mediator;
	}

	[HttpPost("create")]
	[Authorize(ApiConfig.AuthorizedFor_Shared)]
	public async Task<ActionResult> CreateStageProfile(StageProfileRequestDTO profile) {
		Response<StageProfileResponseDTO> res;

		// todo - Zelfde conditionele HTTP status codes kunnen opgenomen worden in overige endpoints
		try {
			res = await _mediator.Send(
				new CreateStageProfileExemplarCommand() {
					ParsedJwtToken = JwtTokenParser.ParseRequest(Request),
					ProfileRequestDTO = profile
				}
			);

			if (res.Success) {
				return Ok(res);
			} else {
				return BadRequest(res);
			}
		} catch (Exception ex) {
			return BadRequest(new FallbackResponse(nameof(StageProfileController) + ApiConfig.ExcSeparator + ex.Message));
		}
	}

}


