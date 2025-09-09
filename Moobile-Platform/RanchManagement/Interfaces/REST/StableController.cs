using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moobile_Platform.RanchManagement.Domain.Model.Commands;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;
using Moobile_Platform.RanchManagement.Domain.Services;
using Moobile_Platform.RanchManagement.Interfaces.REST.Resources;
using Moobile_Platform.RanchManagement.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace Moobile_Platform.RanchManagement.Interfaces.REST;

/// <summary>
/// API controller for managing stables
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("/api/v1/stables")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Stables")]
public class StableController(
   IStableCommandService commandService,
   IStableQueryService queryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new stable",
        Description = "Creates a new stable associated with the authenticated user.",
        OperationId = "CreateStables"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Stable successfully created", typeof(StableResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> CreateStables([FromBody] CreateStableResource resource)
    {
        // Extract userId from the JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized("User not authenticated.");
        
        var command = CreateStableCommandFromResourceAssembler.ToCommandFromResource(resource, userId);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();

        return CreatedAtAction(nameof(GetStableById), new { id = result.Id },
            StableResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all stables",
        Description = "Get all stables",
        OperationId = "GetAllStable")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of stables were found", typeof(IEnumerable<StableResource>))]
    public async Task<IActionResult> GetAllStable()
    {
        // Retrieve userId from the JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized("User not authenticated.");
        
        var stables = await queryService.Handle(new GetAllStablesQuery(userId));
        var stableResources = stables.Select(StableResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(stableResources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetStableById(int id)
    {
        var getStableById = new GetStablesByIdQuery(id);
        var result = await queryService.Handle(getStableById);
        if (result is null) return NotFound();
        var resources = StableResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resources);
    }

    /// <summary>
    /// Updates a stable by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStable(int id, [FromBody] UpdateStableResource resource)
    {
        var command = UpdateStableCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();

        return Ok(StableResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    /// <summary>
    /// Deletes a stable by its ID.
    /// </summary>
    /// <param name="id">Stable ID</param>
    /// <returns>Success message or NotFound</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStable(int id)
    {
        var command = new DeleteStableCommand(id);
        var result = await commandService.Handle(command);

        if (result is null)
            return NotFound(new { message = "Stable not found" });

        return Ok(new { message = "Deleted successfully" });
    }
    
    /// <summary>
    /// Get stable by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("name/{name}")]
    [SwaggerOperation(
        Summary = "Get stable by name",
        Description = "Get stable by name",
        OperationId = "GetStableByName")]
    [SwaggerResponse(StatusCodes.Status200OK, "The stable was found", typeof(StableResource))]
    public async Task<IActionResult> GetStableByName(string name)
    {
        var getStableByNameQuery = new GetStableByNameQuery(name);
        var result = await queryService.Handle(getStableByNameQuery);
        if (result is null) return NotFound();
        var resources = StableResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resources);
    }
}