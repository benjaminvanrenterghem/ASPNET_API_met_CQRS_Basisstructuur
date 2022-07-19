using Domain.Model.DTO.Request;
using Domain.Model.Messaging;
using Domain.Static;
using Logic.Mediated.Commands.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Authentication {
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase {
        private readonly IMediator _mediator;
        public IConfiguration _configuration;
        
        public TokenController(IMediator mediator, IConfiguration configuration) {
            _mediator = mediator;
            _configuration = configuration;
        }

        // todo unit tests
        [HttpPost]
		[Authorize(ApiConfig.AuthorizedFor_Public)]
        public async Task<ActionResult> ProvideJWTToken(LoginRequestDTO providedCredentials) {
            try {
                return Ok(
                    await _mediator.Send(
                        new CreateJWTTokenCommand() {
                            ProvidedCredentials = providedCredentials,
                            Subject = _configuration[ApiConfig.JWT_Subject],
                            Key = _configuration[ApiConfig.JWT_Key],
                            Issuer = _configuration[ApiConfig.JWT_Issuer],
                            Audience = _configuration[ApiConfig.JWT_Audience]
                        }
                    )
                );
            } catch (Exception ex) {
                return BadRequest(new FallbackResponse(nameof(TokenController) + ApiConfig.ExcSeparator + ex.Message));
            }
		}


    }
}
