using AutoMapper;
using DAL;
using DAL.Repositories.Generics;
using Domain.Abstract;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Domain.Model.DTO.Converted;
using Domain.Model.DTO.Request;
using FluentValidation.TestHelper;
using Logic.Behaviors.Validators.ForAuthentication;
using Logic.Mediated.Commands.Authentication;
using Logic.Profiles;
using MediatR;
using Micro2Go.Model;
using Micro2Go.Parsers;
using Moq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using Xunit;

// todo na kopieren naar andere tests de ongebruikte zaken weghalen
// todo in artikel aanhalen dat de AuthTests voorzien zijn van de meeste commentaar (eerst geschreven)
namespace Testing {
	public class AuthenticationTests {
		// ----- Benodigdheden
		CancellationTokenSource _cancelToken = new CancellationTokenSource();

		private readonly IMapper _mapper;
		private readonly Mock<IMediator> _mediator;

		private readonly Mock<NetworkDbContext> _dbContext;
		private readonly Mock<GenericReadRepository<User>> _userReadRepoMockWithMockedDbContext;
		private readonly FakeDbSet<User> _fakeUserDbSet = new();

		private readonly CreateJWTTokenCommand _createJWTTokenCommand = new();
		private readonly CreateJWTTokenCommandHandler _createJWTTokenCommandHandler;

		private readonly CreateJWTTokenCommandValidator _createJWTTokenCommandValidator = new();

		// Voor result equality comparisons middels JSON.stringify ipv .Equals override te hoeven opnemen
		private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings() {
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		};

		// Herbruikbare data
		private User _user1 = new("u1", "u1login", "user@test.com", "04b7bcd583850fa5fe5ad6a9109e7a93db5d7a8330ba815041b1afdda8bf1ea3", new() { ClearanceLevel.User }, 1);
		private User _admin1 = new("u2", "u2login", "admin@test.com", "951351844714e596b29aa9c894bdb6e17f441fbcd77760c4596c59c64ac47222", new() { ClearanceLevel.User, ClearanceLevel.Management }, 2);

		private LoginRequestDTO _user1LoginRequestDTO = new() {
			Email = "user@test.com",
			Password = "1234567890poiuytrewq"
		};

		private LoginRequestDTO _admin1LoginRequestDTO = new() {
			Email = "admin@test.com",
			Password = "mnbvcxzasdfghjkl0987654321"
		};

		// Basis data
		private CreateJWTTokenCommand _baseJWTTokenCommand = new() {
			Issuer = "JWT_BVRNET",
			Audience = "JWT_BVRNET_USERS",
			Subject = "JWT_BVRNET_ACCESSTOKEN",
			Key = "Yh2k7QSu4l8CZgv5AeQX5jPETPkI2oUSP5aiMYdCQAW9OL6OD9sDUCJVr874FWs03HdxUSP5aiMYdCQAW9O",
			ProvidedCredentials =  null
		};

		// Invalid data
		private CreateJWTTokenCommand _fullyInvalidJWTTokenCommand = new() {
			Issuer = "",
			Audience = "",
			Subject = "",
			Key = "",
			ProvidedCredentials =  null
		};

		private LoginRequestDTO _fullyInvalidLoginRequestDTO = new() {
			Email = "",
			Password = ""
		};

		public AuthenticationTests() {
			// Mapper voorzien van de bestaande mapping profile
			var mockMapper = new MapperConfiguration(config => {
				config.AddProfile(new ResponseDTOMappingProfile());
				config.AddProfile(new RequestDTOMappingProfile());
			});

			// Net zoals in de extension method, de mapping profiles controleren op fouten bij uitvoer
			mockMapper.AssertConfigurationIsValid();

			// Relevante mocks aanmaken
			_mapper = mockMapper.CreateMapper();
			_mediator = new Mock<IMediator>();
			_dbContext = new Mock<NetworkDbContext>();

			// De read repository voorzien van de fake dbset en dbcontext
			// FakeDbSet entities worden voor flexibiliteit per test ingesteld & niet in de constructor
			_dbContext.SetupGet(db => db.Users).Returns(_fakeUserDbSet);
			_userReadRepoMockWithMockedDbContext = new Mock<GenericReadRepository<User>>(_fakeUserDbSet, _dbContext.Object);

			// De handler voorzien van de repository
			_createJWTTokenCommandHandler = new(_userReadRepoMockWithMockedDbContext.Object, _mapper);
		}

		// ----- Validator
		// Maakt gebruik van TestValidate en Should.. functies van FluentValidation, geen mocking vereist
		[Fact]
		public void CreateJWTToken_Validator_Valid() {
			_baseJWTTokenCommand.ProvidedCredentials = _user1LoginRequestDTO;

			var res = _createJWTTokenCommandValidator.TestValidate(_baseJWTTokenCommand);
			res.ShouldNotHaveAnyValidationErrors();
		}

		[Fact]
		public void CreateJWTToken_Validator_Invalids() {
			var res = _createJWTTokenCommandValidator.TestValidate(_fullyInvalidJWTTokenCommand);
			res.ShouldHaveValidationErrorFor(cmd => cmd.Subject);
			res.ShouldHaveValidationErrorFor(cmd => cmd.Key);
			res.ShouldHaveValidationErrorFor(cmd => cmd.Issuer);
			res.ShouldHaveValidationErrorFor(cmd => cmd.Audience);
			res.ShouldHaveValidationErrorFor(cmd => cmd.ProvidedCredentials);

			_fullyInvalidJWTTokenCommand.ProvidedCredentials = _fullyInvalidLoginRequestDTO;
			var res2 = _createJWTTokenCommandValidator.TestValidate(_fullyInvalidJWTTokenCommand);
			res2.ShouldHaveValidationErrorFor(cmd => cmd.ProvidedCredentials.Email);
			res2.ShouldHaveValidationErrorFor(cmd => cmd.ProvidedCredentials.Password);
		}


		// ----- Handler
		// Indien er in de handler tests gebruik gemaakt wordt van de FakeDbSet & diens gerelateerde repo is het niet vereist om repository aanroepingen te verifyen (in dat geval zou de IQueryable chain gemocked dienen te worden)
		[Fact]
		public async void CreateJWTToken_UserExists_ValidCredentials_ResultsIn_ValidToken() {
			// Correcte werking handler vaststellen
			_fakeUserDbSet.SetSingle(_user1);
			_baseJWTTokenCommand.ProvidedCredentials = _user1LoginRequestDTO;

			var handled = await _createJWTTokenCommandHandler.Handle(_baseJWTTokenCommand, _cancelToken.Token);

			Assert.True(handled.Success);
			Assert.Equal(1, handled.Content?.Count);
			Assert.StartsWith("ey", handled.Content.First());
			Assert.Empty(handled.Messages);

			// JWT token parsen & inhoud controleren
			VerboseParsedJwtToken jwtResult = JwtTokenParser.ParseToVerboseJwtToken(handled.Content.First());

			if(jwtResult is null) {
				throw new Exception();
			}

			Assert.Equal(_user1.Id, jwtResult.UserId);
			Assert.Equal(_user1.Email, jwtResult.Email);
			Assert.Equal(_user1.DisplayName, jwtResult.DisplayName);
			Assert.Equal(_user1.LoginName, jwtResult.LoginName);

			Assert.Equal(_user1.ClearanceLevels.Count, jwtResult.ClearanceLevels.Count);

			foreach (var cl in _user1.ClearanceLevels) {
				var clstr = cl.ToString();
				Assert.Contains(clstr, jwtResult.ClearanceLevels.ConvertAll(cl => cl.ToString()));
			}

			Assert.Equal(_baseJWTTokenCommand.Issuer, jwtResult.Issuer);
			Assert.Equal(_baseJWTTokenCommand.Audience, jwtResult.Audience);
			Assert.Equal(_baseJWTTokenCommand.Subject, jwtResult.Subject);

			// Ingestelde expiration date verifieren
			var now = DateTime.Now;
			var jwtExpirationTestLeniencyMS = 10 * 1000;
			var jwtLifetime = _baseJWTTokenCommand.TokenValidForSeconds;
			var jwtLifetimeLowerBounds = jwtLifetime - jwtExpirationTestLeniencyMS;
			var jwtLifetimeHigherBounds = jwtLifetime + jwtExpirationTestLeniencyMS;

			var jwtLifetimeDiff = (jwtResult.Expiration - now).TotalSeconds;

			Assert.True(jwtLifetimeDiff > jwtLifetimeLowerBounds);
			Assert.True(jwtLifetimeDiff < jwtLifetimeHigherBounds);
		}

		[Fact]
		public async void CreateJWTToken_UserExists_InvalidCredentials_ReturnsError() {
			_fakeUserDbSet.SetSingle(_user1);

			_user1LoginRequestDTO.Password = "invalidpassword----------";
			_baseJWTTokenCommand.ProvidedCredentials = _user1LoginRequestDTO;

			var handled = await _createJWTTokenCommandHandler.Handle(_baseJWTTokenCommand, _cancelToken.Token);

			Assert.False(handled.Success);
			Assert.Equal(1, handled.Messages.Count);
			Assert.Equal("Invalid credentials", handled.Messages.First().Body);
			Assert.Null(handled.Content);
		}

		[Fact]
		public async void CreateJWTToken_UserDoesNotExist_NilCredentials_ReturnsError() {
			_fakeUserDbSet.Clear();

			var handled = await _createJWTTokenCommandHandler.Handle(_baseJWTTokenCommand, _cancelToken.Token);

			Assert.False(handled.Success);
			Assert.Equal(1, handled.Messages.Count);
			Assert.Equal("Invalid credentials", handled.Messages.First().Body);
			Assert.Null(handled.Content);
		}
		

	}
}