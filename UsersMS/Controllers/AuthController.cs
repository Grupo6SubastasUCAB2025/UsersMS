using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Auth.Application.Commands;
using UsersMS.Auth.Infrastructure.DTOs.Login;
using UsersMS.Auth.Infrastructure.DTOs.Logout;
using UsersMS.Auth.Infrastructure.DTOs.RefreshToken;
using UsersMS.Core.Utilities;

namespace UsersMS.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginResponseDTO), 200)]
        public async Task<IActionResult> GetToken([FromBody] LoginRequestDTO request)
        {
            return await ActionExecutor.Execute(async () =>
            {
                var command = new LoginCommand(request);
                var response = await _mediator.Send(command);
                return response.Success ? Ok(response) : BadRequest(response.Message);
            }, ModelState, _logger, "Login");
        }

        [HttpPost("RefreshToken")]
        [ProducesResponseType(typeof(RefreshTokenResponseDTO), 200)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            return await ActionExecutor.Execute(async () =>
            {
                var command = new RefreshTokenCommand(request);
                var response = await _mediator.Send(command);
                return response.Success ? Ok(response) : BadRequest(response.Message);
            }, ModelState, _logger, "RefreshToken");
        }

        /*[HttpPost("CreateUser")]
        [Authorize]
        [ProducesResponseType(typeof(CreateUserResponseDTO), 200)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
        {
            return await ActionExecutor.Execute(async () =>
            {
                var command = new CreateUserCommand(request);
                var response = await _mediator.Send(command);
                return response.Success ? Ok(response) : BadRequest(response.Message);
            }, ModelState, _logger, "CreateUser");
        }*/

        [HttpPost("Logout")]
        [Authorize]
        [ProducesResponseType(typeof(LogoutResponseDTO), 200)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO request)
        {
            return await ActionExecutor.Execute(async () =>
            {
                var command = new LogoutCommand(request);
                var response = await _mediator.Send(command);
                return response.Success ? Ok(response) : BadRequest(response.Message);
            }, ModelState, _logger, "Logout");
        }
    }
}
