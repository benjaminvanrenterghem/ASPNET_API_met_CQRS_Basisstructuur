using AutoMapper;
using DAL;
using DAL.Repositories.Generics;
using Domain.Abstract;
using Domain.Interfaces.Repositories.Generics;
using Domain.Model;
using Logic.Behaviors.Validators.ForAuthentication;
using Logic.Mediated.Commands.Authentication;
using Logic.Profiles;
using MediatR;
using Micro2Go.Model;
using Moq;
using Newtonsoft.Json;
using System;
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

		// Instantieringen
		public AuthenticationTests() {
			// Mapper voorzien van de bestaande mapping profile
			var mockMapper = new MapperConfiguration(config => {
				config.AddProfile(new ResponseDTOMappingProfile());
				config.AddProfile(new RequestDTOMappingProfile());
			});

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
			throw new NotImplementedException();
		}

		[Fact]
		public void CreateJWTToken_Validator_Invalids() {
			throw new NotImplementedException();
		}


		// ----- Handler
		[Fact]
		public void CreateJWTToken_UserExists_ValidCredentials_ResultsIn_ValidToken() {
			throw new NotImplementedException();
		}

		[Fact]
		public void CreateJWTToken_UserExists_InvalidCredentials_Throws_HandlerExc() {
			throw new NotImplementedException();
			// assert throws handlerexception
		}

		[Fact]
		public void CreateJWTToken_UserDoesNotExist_NilCredentials_Throws_HandlerExc() {
			throw new NotImplementedException();

		}
		

	}
}