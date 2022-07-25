using Domain.Model.DTO.Request;
using Domain.Model.Messaging;
using Domain.Static;
using Logic.Mediated.Commands.Profile;
using Logic.Mediated.Queries.Profile;
using MediatR;
using Micro2Go.Parsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// todo fallback wrapper functie
namespace API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class StageProfileController : ControllerBase {
		private readonly IMediator _mediator;

		public StageProfileController(IMediator mediator) { 
			_mediator = mediator;
		}

		[HttpGet("profiles")]
		[Authorize(ApiConfig.AuthorizedFor_Public)]
		public async Task<ActionResult> GetAllStageProfiles(int? page, int? pageSize, string? searchPropertyName = null, string? searchValue = null) {
			try {
				return Ok(
					await _mediator.Send(
						new GetAllStageProfilesQuery () { 
							Page = page ?? ApiConfig.DefaultPage, 
							PageSize = pageSize ?? ApiConfig.DefaultPageSize,
							SearchPropertyName = searchPropertyName,
							SearchValue = searchValue
						}
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(StageProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}	
		}

		[HttpGet("{id:int}")]
		[Authorize(ApiConfig.AuthorizedFor_Public)]
		public async Task<ActionResult> GetSingleStageProfile(int id) {
			try {
				return Ok(
					await _mediator.Send(
						new GetSingleStageProfileQuery() { 
							Id = id 
						}
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(StageProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		[HttpPost("create")]
		[Authorize(ApiConfig.AuthorizedFor_Shared)]
		public async Task<ActionResult> CreateStageProfile(StageProfileRequestDTO profile) {
			try {
				return Ok(
					await _mediator.Send(
						new CreateStageProfileCommand() {
							ParsedJwtToken = JwtTokenParser.ParseRequest(Request),
							ProfileRequestDTO = profile 
						}
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(StageProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		[HttpPut("update")]
		[Authorize(ApiConfig.AuthorizedFor_Shared)]
		public async Task<ActionResult> UpdateStageProfile(StageProfileRequestDTO profile) {
			try {
				return Ok(
					await _mediator.Send(
						new UpdateStageProfileCommand() {
							ParsedJwtToken = JwtTokenParser.ParseRequest(Request),
							ProfileRequestDTO = profile
						}
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(StageProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		[HttpDelete("delete/{id:int}")]
		[Authorize(ApiConfig.AuthorizedFor_Shared)]
		public async Task<ActionResult> DeleteStageProfile(int id) {
			try {
				return Ok(
					await _mediator.Send(
						new DeleteStageProfileCommand() {
							ParsedJwtToken = JwtTokenParser.ParseRequest(Request),
							Id = id
						}
					)
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(StageProfileController) + ApiConfig.ExcSeparator + ex.Message));
			}
		}

		

	}
}
