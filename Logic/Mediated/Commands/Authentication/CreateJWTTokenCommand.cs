﻿using Domain.Exceptions;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Request;
using Domain.Model.Messaging;
using MediatR;
using Micro2Go.Extensions;
using Micro2Go.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logic.Mediated.Commands.Authentication {
	public class CreateJWTTokenCommand : IRequest<Response<string?>> {
		public LoginRequestDTO ProvidedCredentials { get; set; }
		public string Subject { get; set; }
		public string Key { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public int TokenValidForSeconds => 7 * 86400; // 7 dagen
	}

	public class CreateJWTTokenCommandHandler : IRequestHandler<CreateJWTTokenCommand, Response<string?>> {
		public readonly IGenericReadRepository<User> _userReadRepository;

		public CreateJWTTokenCommandHandler(IGenericReadRepository<User> userReadRepository) {
			_userReadRepository = userReadRepository;
		}

		// todo validator, tests
		public async Task<Response<string?>> Handle(CreateJWTTokenCommand request, CancellationToken cancellationToken) {
			var user = _userReadRepository.GetAllWithLazyLoading()
										  .Where(user => user.Email == request.ProvidedCredentials.Email)
										  .Where(user => user.Password == 
														 request.ProvidedCredentials.Password.GetSHA256String())
										  .ToList()
										  .FirstOrDefault();

			if(user == null) {
				throw new HandlerException("Invalid credentials");
			}

			var claims = new[] {
				new Claim("UserId", user.Id?.ToString() ?? "-1"),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("DisplayName", user.DisplayName),
				new Claim("LoginName", user.LoginName),
				new Claim(JwtRegisteredClaimNames.Sub, request.Subject),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
				new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddSeconds(request.TokenValidForSeconds).ToString())
			};

			user.ClearanceLevels.ForEach((clearanceLevel) => {
				claims.Append(
					new Claim(nameof(ClearanceLevel), clearanceLevel.ToString())
				);
			});

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.Key));
			var token = new JwtSecurityToken(
				request.Issuer,
				request.Audience,
				claims,
				expires: DateTime.UtcNow.AddSeconds(request.TokenValidForSeconds),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return new Response<string?>(new JwtSecurityTokenHandler().WriteToken(token));
		}

	}
}