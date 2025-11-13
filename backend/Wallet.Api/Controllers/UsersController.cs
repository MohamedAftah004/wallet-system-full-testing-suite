using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Wallet.Api.DTOs.Users;
using Wallet.Application.Users.Commands.ActivateUser;
using Wallet.Application.Users.Commands.CloseUser;
using Wallet.Application.Users.Commands.DisableUser;
using Wallet.Application.Users.Commands.FreezeUser;
using Wallet.Application.Users.Commands.RegisterUser;
using Wallet.Application.Users.Queries.GetAllUsersInfo;
using Wallet.Application.Users.Queries.GetUserDashboard;
using Wallet.Application.Users.Queries.GetUserInfoByEmail;
using Wallet.Application.Users.Queries.GetUserInfoById;
using Wallet.Application.Users.Queries.GetUserInfoByPhone;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task <ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserRequestDto dto)
        {
            var command = new RegisterUserCommand(
                dto.FullName,
                dto.Email,
                dto.PhoneNumber,
                dto.Password
                );

            var userId = await _mediator.Send(command);

            var response = new RegisterUserResponseDto
            {
                UserId = userId,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Status = Domain.Enums.UserStatus.PendingActivation
            };

            return Ok(response);
        }


        /// <summary>
        /// Activates a user account within the system by changing its status to <c>Active</c>.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be activated.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [HttpPatch("{userId}/activate")]
        public async Task<IActionResult>Activate(Guid userId)
        {
            await _mediator.Send(new ActivateUserCommand(userId));
            return NoContent();
        }


        /// <summary>
        /// Freezing a user account within the system by changing its status to <c>Freeze</c>.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be Frozen.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [HttpPatch("{userId}/freeze")]
        public async Task<IActionResult>Freeze(Guid userId)
        {
            await _mediator.Send(new FreezeUserCommand(userId));
            return NoContent();
        }

        /// <summary>
        /// Disable a user account within the system by changing its status to <c>Disable</c>.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be Disabled.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [HttpPatch("{userId}/disable")]
        public async Task<IActionResult> Disable(Guid userId)
        {
            await _mediator.Send(new DisableUserCommand(userId));
            return NoContent();
        }


        /// <summary>
        /// Close a user account within the system by changing its status to <c>Close</c>.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be Closed.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [HttpPatch("{userId}/close")]
        public async Task<IActionResult> Close(Guid userId)
        {
            await _mediator.Send(new CloseUserCommand(userId));
            return NoContent();
        }




        /// <summary>
        /// Retrieves a paginated list of users.
        /// </summary>
        /// <param name="page">Page number (default = 1)</param>
        /// <param name="size">Number of items per page (default = 10)</param>
        /// <returns>Paged list of users</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var query = new GetAllUsersInfoQuery(page, size);

            var result = await _mediator.Send(query);

            return Ok(result);
        }


        /// <summary>
        /// Retrieves user information by ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserInfoByIdQuery(userId), cancellationToken);
            var response = new UserResponseDto
            {
                UserId = result.UserId,
                FullName = result.FullName,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                Status = result.Status,
                CreatedAt = result.CreatedAt
            };

            return Ok(response);
        }


        /// <summary>
        /// Retrieves user information by Email.
        /// </summary>
        /// <param name="email">The unique identifier of the user.</param>
        [HttpGet("by-email/{email}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserInfoByEmailQuery(email), cancellationToken);
            var response = new UserResponseDto
            {
                UserId = result.UserId,
                FullName = result.FullName,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                Status = result.Status,
                CreatedAt = result.CreatedAt
            };

            return Ok(response);
        }


        /// <summary>
        /// Retrieves user information by Phone.
        /// </summary>
        /// <param name="phone">The unique identifier of the user.</param>
        [HttpGet("by-phone/{phone}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByPhone(string phone, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserInfoByPhoneQuery(phone), cancellationToken);
            var response = new UserResponseDto
            {
                UserId = result.UserId,
                FullName = result.FullName,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                Status = result.Status,
                CreatedAt = result.CreatedAt
            };

            return Ok(response);
        }

        // GET: /api/users/{userId}/dashboard
        [HttpGet("{userId}/dashboard")]
        public async Task<IActionResult> GetUserDashboard(Guid userId, CancellationToken cancellationToken)
        {
            var query = new GetUserDashboardQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }

}
