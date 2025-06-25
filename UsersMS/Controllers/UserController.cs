using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using UsersMS.Application.Commands.RecordUserData;
using UsersMS.Application.Commands.UpdateUser;
using System.Security.Claims;
using UsersMS.Core.Utilities;
using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;
using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs;
using UsersMS.Application.Queries.GetAdministratorById;
using UsersMS.Application.Queries.GetAdministratorByName;
using UsersMS.Application.Queries.GetAuctioneerById;
using UsersMS.Application.Queries.GetAuctioneerByName;
using UsersMS.Application.Queries.GetBidderById;
using UsersMS.Application.Queries.GetBidderByName;
using UsersMS.Application.Queries.GetTechnicalSupportById;
using UsersMS.Application.Queries.GetTechnicalSupportByName;
using UsersMS.Infrastructure.DTOs.UpdateUser;

namespace UsersMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID is missing from the token.");
            return Guid.Parse(userId);
        }

        [HttpGet("GetAdministratorById/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(GetAdministratorByIdResponseDTO), 200)]
        public async Task<IActionResult> GetAdministratorById(Guid id) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetAdministratorByIdQuery(GetUserId(), id);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetAdministratorById");

        [HttpGet("GetAdministratorsByName/{name}")]
        [Authorize]
        [ProducesResponseType(typeof(GetAdministratorByNameResponseDTO), 200)]
        public async Task<IActionResult> GetAdministratorsByName(string name) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetAdministratorByNameQuery(GetUserId(), name);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetAdministratorsByName");

        [HttpGet("GetAuctioneerById/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(GetAuctioneerByIdResponseDTO), 200)]
        public async Task<IActionResult> GetAuctioneerById(Guid id) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetAuctioneerByIdQuery(GetUserId(), id);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetAuctioneerById");

        [HttpGet("GetAuctioneersByName/{name}")]
        [Authorize]
        [ProducesResponseType(typeof(GetAuctioneerByNameResponseDTO), 200)]
        public async Task<IActionResult> GetAuctioneersByName(string name) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetAuctioneerByNameQuery(GetUserId(), name);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetAuctioneersByName");

        [HttpGet("GetBidderById/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(GetBidderByIdResponseDTO), 200)]
        public async Task<IActionResult> GetBidderById(Guid id) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetBidderByIdQuery(GetUserId(), id);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetBidderById");

        [HttpGet("GetBiddersByName/{name}")]
        [Authorize]
        [ProducesResponseType(typeof(GetBidderByNameResponseDTO), 200)]
        public async Task<IActionResult> GetBiddersByName(string name) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetBidderByNameQuery(GetUserId(), name);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetBiddersByName");

        [HttpGet("GetTechnicalSupportById/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(GetTechnicalSupportByIdResponseDTO), 200)]
        public async Task<IActionResult> GetTechnicalSupportById(Guid id) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetTechnicalSupportByIdQuery(GetUserId(), id);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetTechnicalSupportById");

        [HttpGet("GetTechnicalSupportsByName/{name}")]
        [Authorize]
        [ProducesResponseType(typeof(GetTechnicalSupportByNameResponseDTO), 200)]
        public async Task<IActionResult> GetTechnicalSupportsByName(string name) =>
            await ActionExecutor.Execute(async () =>
            {
                var query = new GetTechnicalSupportByNameQuery(GetUserId(), name);
                return Ok(await _mediator.Send(query));
            }, ModelState, _logger, "GetTechnicalSupportsByName");

        [HttpPost("RecordUserData")]
        [Authorize]
        [ProducesResponseType(typeof(RecordUserDataResponseDTO), 200)]
        public async Task<IActionResult> CreateUser([FromBody] RecordUserDataRequestDTO request) =>
            await ActionExecutor.Execute(async () =>
            {
                var command = new RecordUserDataCommand(request);
                var response = await _mediator.Send(command);
                return response.Success ? Ok(response) : BadRequest(response);
            }, ModelState, _logger, "RecordUserData");

        [HttpPut("UpdateUserData")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateRecordUserDataResponseDTO), 200)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateRecordUserDataRequestDTO request) =>
            await ActionExecutor.Execute(async () =>
            {
                var command = new UpdateRecordUserDataCommand(request);
                var response = await _mediator.Send(command);
                return response.Success ? Ok(response) : BadRequest(response);
            }, ModelState, _logger, "UpdateUserData");
    }
}
