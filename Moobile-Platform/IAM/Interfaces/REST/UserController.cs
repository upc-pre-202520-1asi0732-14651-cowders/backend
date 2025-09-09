using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Moobile_Platform.IAM.Domain.Model.Commands;
using Moobile_Platform.IAM.Domain.Model.Queries.UserQueries;
using Moobile_Platform.IAM.Domain.Services;
using Moobile_Platform.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;
using Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromUserResources;
using Swashbuckle.AspNetCore.Annotations;

namespace Moobile_Platform.IAM.Interfaces.REST
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("Users")]
    public class UserController(
        IUserCommandService commandService,
        IUserQueryService queryService
        ) : ControllerBase
    {
        /// <summary>
        /// Handles the user sign-up process.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpResource resource)
        {
            var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result, resource.Username, resource.Email);

            return CreatedAtAction(nameof(SignUp), userResource);
        }

        /// <summary>
        /// Handles the user sign-in process.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn([FromBody] SignInResource resource)
        {
            if (string.IsNullOrEmpty(resource.Email) && string.IsNullOrEmpty(resource.UserName))
            {
                return BadRequest("Either Email or UserName must be provided.");
            }

            var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            var userName = !string.IsNullOrEmpty(resource.UserName)
                ? resource.UserName
                : await queryService.GetUserNameByEmail(resource.Email);

            var email = !string.IsNullOrEmpty(resource.Email)
                ? resource.Email
                : await queryService.GetEmailByUserName(resource.UserName);

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result, userName, email);

            return Ok(userResource);
        }

        /// <summary>
        /// Handles the retrieval of user information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-info")]
        [SwaggerResponse(StatusCodes.Status200OK, "User info", typeof(UserInfoResource))]
        public async Task<ActionResult> GetInfo()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user ID");

            try
            {
                var userInfo = await queryService.GetUserInfoWithStatsAsync(userId);
                if (userInfo == null)
                    return NotFound("User not found");

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving user information: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the update of a user's profile.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("update-profile")]
        [SwaggerResponse(StatusCodes.Status200OK, "User updated successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserResource resource)
        {
            // Validate the resource
            if (string.IsNullOrWhiteSpace(resource.Username) || string.IsNullOrWhiteSpace(resource.Email))
            {
                return BadRequest("Username and Email are required");
            }

            // Extract userId from the JWT token claims
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user ID");

            try
            {
                var command = UpdateUserCommandFromResourceAssembler.ToCommandFromResource(resource);
                var result = await commandService.Handle(command, userId);
        
                if (!result)
                    return NotFound("User not found");

                return Ok(new { message = "User updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles the deletion of a user's account.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete-account")]
        [SwaggerResponse(StatusCodes.Status200OK, "Account deleted successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user ID");

            try
            {
                var command = new DeleteUserCommand(userId);
                var result = await commandService.Handle(command);
        
                if (!result)
                    return NotFound("User not found");

                return Ok(new { message = "Account deleted successfully" });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles the retrieval of the user's profile information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        [SwaggerResponse(StatusCodes.Status200OK, "User profile", typeof(UserProfileResource))]
        public async Task<ActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user ID");

            var user = await queryService.Handle(new GetUserByIdQuery(userId));
            if (user is null)
                return NotFound("User not found");

            var resource = new UserProfileResource(user.Username, user.Email, user.EmailConfirmed);
            return Ok(resource);
        }

    }
}