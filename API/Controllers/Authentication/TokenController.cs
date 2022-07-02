using Domain.Interfaces;
using Domain.Model.DTO.Request;
using Domain.Model.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace API.Controllers.Authentication {
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase {
        private readonly IMediator _mediator;
        public IConfiguration _configuration;
        
        public TokenController(IMediator mediator, IConfiguration config) {
            _mediator = mediator;
            _configuration = config;
        }

        // todo open/no auth if necess.
        // todo logica in handler + validator
        [HttpPost]
        public async Task<ActionResult> ProvideJWTToken(LoginRequestDTO providedCredentials) {
            var clearanceLevels = new List<ClearanceLevel>() { ClearanceLevel.User, ClearanceLevel.Management };

            throw new NotImplementedException();

            //if (user != null) {
            //    //create claims details based on the user information
            //    var claims = new[] {
            //        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            //        new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(1).ToString()),
            //        new Claim("ClearanceLevels".ToString(), clearanceLevels.ConvertAll(cl => cl.ToString()).ToString() ?? "")
            //    };

            //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //    var token = new JwtSecurityToken(
            //        _configuration["Jwt:Issuer"],
            //        _configuration["Jwt:Audience"],
            //        claims,
            //        expires: DateTime.UtcNow.AddHours(1),
            //        signingCredentials: signIn
            //    );

            //    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            //} else {
            //    return BadRequest("Invalid credentials");
            //}
        }

    }
}
