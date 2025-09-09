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
/// API controller for managing vaccines
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("/api/v1/vaccines")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vaccines")]
public class VaccineController(
   IVaccineCommandService commandService,
   IVaccineQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Posts a new vaccine to the system.
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new vaccine",
        Description = "Creates a new vaccine associated with the authenticated user.",
        OperationId = "CreateVaccines"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Vaccine successfully created", typeof(VaccineResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateVaccines([FromForm] CreateVaccineResource resource)
    {
        // Extract userId from the JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized("User not authenticated.");
        
        var command = CreateVaccineCommandFromResourceAssembler.ToCommandFromResource(resource, userId);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();

        return CreatedAtAction(nameof(GetVaccineById), new { id = result.Id },
            VaccineResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    /// <summary>
    /// Gets all vaccines in the system.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all vaccines",
        Description = "Get all vaccines",
        OperationId = "GetAllVaccine")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of vaccines were found", typeof(IEnumerable<VaccineResource>))]
    public async Task<IActionResult> GetAllVaccine()
    {
        // Retrieve userId from the JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized("User not authenticated.");
        
        var vaccines = await queryService.Handle(new GetAllVaccinesQuery(userId));
        var vaccineResources = vaccines.Select(VaccineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(vaccineResources);
    }

    /// <summary>
    /// Gets a vaccine by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetVaccineById(int id)
    {
        var getVaccineById = new GetVaccinesByIdQuery(id);
        var result = await queryService.Handle(getVaccineById);
        var resources = VaccineResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resources);
    }

    /// <summary>
    /// Gets all vaccines by bovine ID.
    /// </summary>
    /// <param name="bovineId"></param>
    /// <returns></returns>
    [HttpGet("bovine/{bovineId}")]
    [SwaggerOperation(
        Summary = "Get all vaccines by bovine ID",
        Description = "Get all vaccines by bovine ID",
        OperationId = "GetVaccinesByBovineId")]
    public async Task<ActionResult> GetVaccinesByBovineId(int? bovineId)
    {
        var getVaccinesByBovineIdQuery = new GetVaccinesByBovineIdQuery(bovineId);
        var vaccines = await queryService.Handle(getVaccinesByBovineIdQuery);
        var vaccineResources = vaccines.Select(VaccineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(vaccineResources);
    }

    /// <summary>
    /// Updates a vaccine by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateVaccine(int id, [FromForm] UpdateVaccineResource resource)
    {
        var command = UpdateVaccineCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();

        return Ok(VaccineResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    /// <summary>
    /// Deletes a vaccine by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVaccine(int id)
    {
        var command = new DeleteVaccineCommand(id);
        var result = await commandService.Handle(command);
        if (result is null)
            return NotFound(new { message = "Vaccine not found" });

        return Ok(new { message = "Deleted successfully" });
    }
}