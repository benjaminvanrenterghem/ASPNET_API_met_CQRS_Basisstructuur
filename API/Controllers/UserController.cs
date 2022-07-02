using Domain.Model.DTO.Request;
using Domain.Model.Messaging;
using Domain.Static;
using Logic.Mediated.Commands.Users;
using Logic.Mediated.Queries.Users;
using MediatR;
using Micro2Go.Parsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
	[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) {
            _mediator = mediator;
        }
        
        [HttpGet("admin/users")]
        [Authorize(ApiConfig.AuthorizedFor_Management)]
        public async Task<ActionResult> GetAllUsers(int? page, int? pageSize, string? searchPropertyName = null, string? searchValue = null) {
            try {
                return Ok(
					await _mediator.Send(
						new GetAllUsersQuery() {
                            Page = page ?? ApiConfig.DefaultPage,
                            PageSize = pageSize ?? ApiConfig.DefaultPageSize,
                            SearchPropertyName = searchPropertyName,
                            SearchValue = searchValue
                        }
					)
				);
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

        [HttpGet("admin/user/{id:int}")]
        [Authorize(ApiConfig.AuthorizedFor_Management)]
        public async Task<ActionResult> GetUser(int id) {
            try {
                return Ok(
					await _mediator.Send(
					    new GetSingleUserQuery() { 
                            Id = id
                        }
					)
				);
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

		[HttpPost("register")]
        [Authorize(ApiConfig.AuthorizedFor_Public)]
        public async Task<ActionResult> RegisterUser(UserRequestDTO userRequestDTO) {
            try {
                return Ok(
				    await _mediator.Send(
				        new CreateUserCommand() { 
                            UserRequestDTO = userRequestDTO
                        }
				    )
				);
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

		[HttpPut("update/{id:int}")]
        [Authorize(ApiConfig.AuthorizedFor_Shared)]
        public async Task<ActionResult> UpdateUser(int id, UserRequestDTO userRequestDTO) {
            try {
                // todo, dat is voor in de validator
    //            if(id != userRequestDTO.Id) {
    //                return BadRequest(new Response<bool>(false).AddError("Query param id & dto id mismatch"));
				//}

                return Ok(
					await _mediator.Send(
						new UpdateUserCommand() { 
                            ParsedJwtToken = JwtTokenParser.ParseRequest(Request),
                            UserRequestDTO = userRequestDTO
                        }
					)
				);
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

		[HttpDelete("admin/user/{id:int}")]
        [Authorize(ApiConfig.AuthorizedFor_Management)]
        public async Task<ActionResult> DeleteUser(int id) {
            try {
                return Ok(
				    await _mediator.Send(
				        new DeleteUserCommand() { 
                            Id = id
                        }
				    )
				);
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

    }
}
