using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Micro2Go.Model;
using Microsoft.AspNetCore.Http;

namespace Micro2Go.Parsers {
	public static class JwtTokenParser {
		public static ParsedJwtToken ParseRequest(HttpRequest request) {
			var claims = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].ToString().Replace("Bearer ", "")).Claims;

			return new ParsedJwtToken() {
				UserId = int.Parse(claims?.FirstOrDefault(claim => claim.Type.ToLower() == "userid")?.Value ?? "-1"),
				Email = claims?.FirstOrDefault(claim => claim.Type.ToLower() == "email")?.Value ?? "",
				DisplayName = claims?.FirstOrDefault(claim => claim.Type.ToLower() == "displayname")?.Value ?? "",
				LoginName = claims?.FirstOrDefault(claim => claim.Type.ToLower() == "loginname")?.Value ?? "",
				ClearanceLevels = claims?.Where(claim => claim.Type.ToLower() == "clearancelevel")?.Select(claim => Enum.Parse<ClearanceLevel>(claim.Value.ToString()))?.ToList() ?? new(),
			};
		}
	}
}
