

using Domain.Interfaces;
using Domain.Model.Messaging;
using Domain.Static;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Wordt gebruikt als test endpoint (wordt gebruikt als test bij de initiele configuratie - zie artikel) en indiceert API service en databank liveliness

namespace API.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class DummyController : ControllerBase {
		private readonly INetworkDbContext _dbContext;

		public DummyController(INetworkDbContext dbContext) {
			_dbContext = dbContext;
		}

		[HttpGet]
		[Authorize(ApiConfig.AuthorizedFor_Public)]
		public async Task<ActionResult> PerformLivelinessTest() {
			try {
				return Ok(
					new Response<int>(_dbContext.Profiles.Count())
				);
			} catch (Exception ex) {
				return BadRequest(new FallbackResponse(nameof(DummyController) + " >> " + ex.Message));
			}
		}
	}
}
