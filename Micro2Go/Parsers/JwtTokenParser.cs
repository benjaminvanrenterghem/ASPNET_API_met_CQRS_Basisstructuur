using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Micro2Go.Model;
using Microsoft.AspNetCore.Http;

namespace Micro2Go.Parsers {
	public static class JwtTokenParser {
		private static List<Claim>? ProvideClaims(string jwtToken) {
			try {
				return new JwtSecurityTokenHandler().ReadJwtToken(jwtToken.Replace("Bearer ", ""))
													.Claims
													.ToList();
			} catch (Exception e) when(e.GetType() == typeof(ArgumentException)
									|| e.GetType() == typeof(ArgumentNullException)) {
			return null;
		}
	}

		private static ParsedJwtToken? ParseJwtToken(string jwtToken) {
			var claims = JwtTokenParser.ProvideClaims(jwtToken);

			if(claims is null) {
				return null;
			}

			return new ParsedJwtToken() {
				UserId = int.Parse(claims?.FirstOrDefault(claim => claim.Type.ToLower() == "userid")?.Value ?? "-1"),
				Email = claims?.FirstOrDefault(claim => claim.Type.ToLower() == "email")?.Value ?? "",
				DisplayName = claims?.FirstOrDefault(claim => claim.Type.ToLower() == "displayname")?.Value ?? "",
				LoginName = claims?.FirstOrDefault(claim => claim.Type.ToLower() == "loginname")?.Value ?? "",
				ClearanceLevels = claims?.Where(claim => claim.Type == nameof(ClearanceLevel))
										 .ToList()
										 .ConvertAll(clclaim => Enum.Parse<ClearanceLevel>(clclaim.Value.ToString()))
										 ?? new()

										 // todo rem:
				//ClearanceLevels = claims?.FirstOrDefault(claim => claim.Type == nameof(ClearanceLevel)+"-main")
				//						?.Value.Split(",")
				//						?.ToList()
				//						?.ConvertAll(clstr => Enum.Parse<ClearanceLevel>(clstr))
				//						?? new()
			};
		}

		public static ParsedJwtToken ParseBareToken(string jwtToken) {
			return JwtTokenParser.ParseJwtToken(jwtToken);
		}

		public static ParsedJwtToken ParseRequest(HttpRequest request) {
			return JwtTokenParser.ParseJwtToken(request.Headers["Authorization"].ToString());
		}

		// Ten behoeve van unit tests & validatie qua inhoud
		public static VerboseParsedJwtToken? ParseToVerboseJwtToken(string jwtToken) { 
			List<Claim> claims = JwtTokenParser.ProvideClaims(jwtToken);
			ParsedJwtToken standardJwt = JwtTokenParser.ParseBareToken(jwtToken);

			if(standardJwt is null){
				return null;
			}

			return new VerboseParsedJwtToken {
				UserId = standardJwt.UserId,
				Email = standardJwt.Email,
				DisplayName = standardJwt.DisplayName,
				LoginName = standardJwt.LoginName,
				ClearanceLevels = standardJwt.ClearanceLevels,

				Issuer = claims.Where(cl => cl.Type.ToLower() == "iss")
							   .FirstOrDefault()
							   ?.Value
							   ?? "",
				Audience = claims.Where(cl => cl.Type.ToLower() == "aud")
							   .FirstOrDefault()
							   ?.Value
							   ?? "",
				Subject = claims.Where(cl => cl.Type.ToLower() == "sub")
							   .FirstOrDefault()
							   ?.Value
							   ?? "",
				Expiration = DateTimeOffset.FromUnixTimeMilliseconds(
					long.Parse(
						claims.Where(cl => cl.Type.ToLower() == "exp")
								.FirstOrDefault()
								?.Value
								?? "0"
					) * 1000
				).DateTime
			};


		}
	}
}
