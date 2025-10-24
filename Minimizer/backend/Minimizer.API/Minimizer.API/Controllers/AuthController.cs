using MediatR;
using Microsoft.AspNetCore.Mvc;
using Minimizer.API.Services;
using Minimizer.Common.Constants;
using Minimizer.Common.Models.Users;
using Minimizer.Services.Users;

namespace Minimizer.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly AuthService authService;

        public AuthController(IMediator mediator, AuthService authService)
        {
            this.mediator = mediator;
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await mediator.Send(new GetUserByUserNameQuery(request.Username));

            if (user.UserName == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized();

            var token = authService.GenerateJwtToken(new UserResponse() { UserName = "raraya", Role = "admin"});
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UpsertUserRequest upsertUserRequest)
        {
            var userExists = await mediator.Send(new CheckUserByUserNameQuery(upsertUserRequest.UserName));

            if (userExists)
                return BadRequest(string.Format(ErrorContants.UserNameExists, upsertUserRequest.UserName));

            return Ok();
        }
    }
}
