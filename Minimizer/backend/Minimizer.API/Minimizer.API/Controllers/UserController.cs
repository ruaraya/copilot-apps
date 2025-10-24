using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimizer.Common.Constants;
using Minimizer.Common.Exceptions;
using Minimizer.Common.Models.Users;
using Minimizer.Services.Users;
using System.Net;

namespace Minimizer.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<UserResponse>> GetAllUsers()
        {
            return await mediator.Send(new GetAllUsersQuery());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (!Guid.TryParse(userId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(userId), string.Format(ErrorContants.InvalidGuid, nameof(userId), userId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            var user = await mediator.Send(new GetUserByUserIdQuery(guidId));

            if (user == null) 
                return NotFound();
            
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UpsertUserRequest upsertUserRequest)
        {
            bool userExist = await mediator.Send(new CheckUserByUserNameQuery(upsertUserRequest.UserName));

            if (userExist)
                return BadRequest(string.Format(ErrorContants.UserNameExists, upsertUserRequest.UserName));

            UserResponse newUser = await mediator.Send(new InsertUserCommand(upsertUserRequest));
            return Ok(newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpsertUserRequest upsertUserRequest)
        {
            if (!Guid.TryParse(userId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(userId), string.Format(ErrorContants.InvalidGuid, nameof(userId), userId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            bool userExist = await mediator.Send(new CheckUserByUserNameQuery(upsertUserRequest.UserName));

            if (!userExist) 
                return NotFound();

            UserResponse newUser = await mediator.Send(new UpdateUserCommand(guidId, upsertUserRequest));
            return Ok(newUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (!Guid.TryParse(userId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(userId), string.Format(ErrorContants.InvalidGuid, nameof(userId), userId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            await mediator.Send(new DeleteUserCommand(guidId));
            return NoContent();
        }
    }
}
