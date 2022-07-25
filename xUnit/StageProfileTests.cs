using AutoMapper;
using DAL;
using Domain.Model;
using Domain.Model.DTO.Request;
using Logic.Behaviors.Validators.ForProfiles;
using Logic.Behaviors.Validators.ForProfiles.Queries;
using Logic.Mediated.Commands.Profile;
using Logic.Mediated.Queries.Profile;
using Logic.Profiles;
using MediatR;
using Micro2Go.Model;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading;
using Xunit;

namespace Testing;

public class StageProfileTests {
	// ----- Benodigdheden
	CancellationTokenSource _cancelToken = new CancellationTokenSource();

	private readonly IMapper _mapper;
	private readonly Mock<IMediator> _mediator;

	private readonly Mock<NetworkDbContext> _dbContext;
	//

	private readonly GetAllStageProfilesQuery _getAllStageProfilesQuery;
	private readonly GetSingleStageProfileQuery _getSingleStageProfileQuery;
	private readonly CreateStageProfileCommand _createStageProfileCommand;
	private readonly UpdateStageProfileCommand _updateStageProfileCommand;
	private readonly DeleteStageProfileCommand _deleteStageProfileCommand;

	private readonly GetAllStageProfilesQueryHandler _getAllStageProfilesQueryHandler;
	private readonly GetSingleStageProfileQueryHandler _getSingleStageProfileQueryHandler;
	private readonly CreateStageProfileCommandHandler _createStageProfileCommandHandler;
	private readonly UpdateStageProfileCommandHandler _updateStageProfileCommandHandler;
	private readonly DeleteStageProfileCommandHandler _deleteStageProfileCommandHandler;

	private readonly GetAllStageProfilesQueryValidator _getAllStageProfilesQueryValidator = new();
	private readonly GetSingleStageProfileQueryValidator __getSingleStageProfileQueryValidator = new();
	private readonly CreateStageProfileCommandValidator _createStageProfileCommandValidator = new();
	private readonly UpdateStageProfileCommandValidator _updateStageProfileCommandValidator = new();
	private readonly DeleteStageProfileCommandValidator _deleteStageProfileCommandValidator = new();

	private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings() {
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
	};

	// ----- Herbruikbare data
	private ParsedJwtToken _fullyInvalidParsedJwtToken = new() {
		Email = "",
		DisplayName = "",
		LoginName = "",
		UserId = 0,
		ClearanceLevels = new()
	};

	private StageProfileRequestDTO _baseInvalidStageProfileRequestDTO = new() {
		Id = null,
		OwnerUserId = 0,
		FullName = "",
		About = "",
		WebsiteURL = "",
		Deleted = true
	};

	// ----- todo cont
	private StageProfile _stageProfile1 = new("StageProfile1", 1, "SP1 About", "http://google.com");

	// -----
	private GetAllStageProfilesQuery _fullyInvalidGetAllStageProfilesQuery = new() {
		Page = 0,
		PageSize = 0,
		SearchPropertyName = null,
		SearchValue = null
	};

	private GetAllStageProfilesQuery _fullyValidGetAllStageProfilesQuery = new() {
		Page = 1,
		PageSize = 100,
		SearchPropertyName = "FullName",
		SearchValue = "StageProfile1"
	};

	private GetSingleStageProfileQuery _fullyInvalidGetSingleStageProfileQuery = new() {
		Id = 0
	};

	private CreateStageProfileCommand _fullyInvalidCreateStageProfileCommand1 = new() {
		ParsedJwtToken = null,
		ProfileRequestDTO = null
	};

	private UpdateStageProfileCommand _fullyInvalidUpdateStageProfileCommand = new() {
		ParsedJwtToken = null,
		ProfileRequestDTO = null
	};

	private DeleteStageProfileCommand _fullyInvalidDeleteStageProfileCommand = new() {
		Id = 0,
		ParsedJwtToken = null
	};

	public StageProfileTests() {
		var mockMapper = new MapperConfiguration(config => {
			config.AddProfile(new ResponseDTOMappingProfile());
			config.AddProfile(new RequestDTOMappingProfile());
		});

		mockMapper.AssertConfigurationIsValid();

		_mapper = mockMapper.CreateMapper();
		_mediator = new Mock<IMediator>();
		_dbContext = new Mock<NetworkDbContext>();

		//

		// Handlers

	}

	// ----- Validator
	[Fact]
	public void GetAllStageProfiles_Validator_Valid() {
		throw new NotImplementedException();
	}

	[Fact]
	public void GetAllStageProfiles_Validator_Invalids() {
		throw new NotImplementedException();
	}

	[Fact]
	public void GetSingleStageProfile_Validator_Valid() {
		throw new NotImplementedException();
	}

	[Fact]
	public void GetSingleStageProfile_Validator_Invalids() {
		throw new NotImplementedException();
	}

	[Fact]
	public void CreateStageProfile_Validator_Valid() {
		throw new NotImplementedException();
	}

	[Fact]
	public void CreateStageProfile_Validator_Invalids() {
		throw new NotImplementedException();
	}

	[Fact]
	public void UpdateStageProfile_Validator_Valid() {
		throw new NotImplementedException();
	}

	[Fact]
	public void UpdateStageProfile_Validator_Invalids() {
		throw new NotImplementedException();
	}

	[Fact]
	public void DeleteStageProfile_Validator_Valid() {
		throw new NotImplementedException();
	}

	[Fact]
	public void DeleteStageProfile_Validator_Invalids() {
		throw new NotImplementedException();
	}

	// ----- Handler

	// todo

}