using Domain.Model.Messaging;
using Domain.Static;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// todo toevoegen auth attribs
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) {
            _mediator = mediator;
        }
        
        // todo args voor paginated search req
        [HttpGet("admin/users")]
        public async Task<ActionResult> GetAllUsers() {
            try {
                // todo handler
                return Ok(
                    //await _mediator.Send(
                        //new GetAllProfilesQuery() { Page = page ?? default, PageSize = pageSize ?? default }
                    //)
                );
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

        // todo idem
        [HttpGet("admin/user/{id:int}")]
        public async Task<ActionResult> GetUser(int id) {
            try {
                // todo handler
                return Ok(
                //await _mediator.Send(
                //new GetAllProfilesQuery() { Page = page ?? default, PageSize = pageSize ?? default }
                //)
                );
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

		// todo
		[HttpPost("register")]
        public async Task<ActionResult> RegisterUser() {
            try {
                // todo handler
                return Ok(
                //await _mediator.Send(
                //new GetAllProfilesQuery() { Page = page ?? default, PageSize = pageSize ?? default }
                //)
                );
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

		// todo
		[HttpPut("self/update/{id:int}")]
        public async Task<ActionResult> UpdateSelf(int id /*,dto*/) {
            try {
                // todo handler
                return Ok(
                //await _mediator.Send(
                //new GetAllProfilesQuery() { Page = page ?? default, PageSize = pageSize ?? default }
                //)
                );
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

		// todo
		[HttpPut("admin/update/{id:int}")]
        public async Task<ActionResult> UpdateUser(int id /*,dto*/) {
            try {
                // todo handler
                return Ok(
                //await _mediator.Send(
                //new GetAllProfilesQuery() { Page = page ?? default, PageSize = pageSize ?? default }
                //)
                );
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

		// todo
		[HttpDelete("admin/user/{id:int}")]
        public async Task<ActionResult> DeleteUser(int id) {
            try {
                // todo handler
                return Ok(
                //await _mediator.Send(
                //new GetAllProfilesQuery() { Page = page ?? default, PageSize = pageSize ?? default }
                //)
                );
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(UserController) + ApiConfig.ExcSeparator + ex.Message));
            }
        }

    }
}
