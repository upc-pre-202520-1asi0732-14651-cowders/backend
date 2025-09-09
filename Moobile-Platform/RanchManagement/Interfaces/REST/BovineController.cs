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
/// API controller for managing bovines
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("/api/v1/bovines")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Bovines")]
public class BovinesController(IBovineCommandService commandService,
    IBovineQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Posts a new bovine to the system.
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new bovine",
        Description = "Creates a new bovine associated with the authenticated user.",
        OperationId = "CreateBovines"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Bovine successfully created", typeof(BovineResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateBovines([FromForm] CreateBovineResource resource)
    {
        // Extract userId from the JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized("User not authenticated.");

        var command = CreateBovineCommandFromResourceAssembler.ToCommandFromResource(resource, userId);
        var result = await commandService.Handle(command);

        // If the result is null, it indicates an error in creation
        if (result is null) return BadRequest();

        return CreatedAtAction(nameof(GetBovineById), new { id = result.Id },
            BovineResourceFromEntityAssembler.ToResourceFromEntity(result));
    }


    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all bovines",
        Description = "Get all bovines",
        OperationId = "GetAllBovine")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of bovines were found", typeof(IEnumerable<BovineResource>))]
    public async Task<IActionResult> GetAllBovine()
    {
        // Retrieve userId from the JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized("User not authenticated.");

        // Use the userId to fetch all bovines
        var bovines = await queryService.Handle(new GetAllBovinesQuery(userId));
        var bovineResources = bovines.Select(BovineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(bovineResources);
    }


    /// <summary>
    /// Gets a bovine by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetBovineById(int id)
    {
        var getBovineById = new GetBovinesByIdQuery(id);
        var result = await queryService.Handle(getBovineById);
        var resources = BovineResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resources);
    }

    /// <summary>
    /// Gets all bovines by stable ID.
    /// </summary>
    /// <param name="stableId"></param>
    /// <returns></returns>
    [HttpGet("stable/{stableId}")]
    [SwaggerOperation(
        Summary = "Get all bovines by stable ID",
        Description = "Get all bovines by stable ID",
        OperationId = "GetBovinesByStableId")]
    public async Task<ActionResult> GetBovinesByStableId(int? stableId)
    {
        var getBovinesByStableIdQuery = new GetBovinesByStableIdQuery(stableId);
        var bovines = await queryService.Handle(getBovinesByStableIdQuery);
        var bovineResources = bovines.Select(BovineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(bovineResources);
    }

    /// <summary>
    /// Updates a bovine by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateBovine(int id, [FromForm] UpdateBovineResource resource)
    {
        var command = UpdateBovineCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();

        return Ok(BovineResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    /// <summary>
    /// Deletes a bovine by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBovine(int id)
    {
        var command = new DeleteBovineCommand(id);
        var result = await commandService.Handle(command);
        if (result is null)
            return NotFound(new { message = "Bovine not found" });

        return Ok(new { message = "Deleted successfully" });
    }
}