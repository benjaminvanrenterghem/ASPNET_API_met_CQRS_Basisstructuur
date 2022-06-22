using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Micro2Go.Model;
using Microsoft.AspNetCore.Http;

namespace Micro2Go.Parsers {
	public static class JwtTokenParser {
		public static ParsedJwtToken ParseRequest(HttpRequest request) {
			var claims = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].ToString().Replace("Bearer ", "")).Claims;

			return new ParsedJwtToken() {
				Email = claims?.FirstOrDefault(claim => claim.Type == "email")?.Value ?? "",
				Name = claims?.FirstOrDefault(claim => claim.Type == "name")?.Value ?? "",
				Groups = claims?.Where(claim => claim.Type == "groups")?.Select(claim => claim.Value.ToString())?.ToList() ?? new(),
			};
		}
	}
}
