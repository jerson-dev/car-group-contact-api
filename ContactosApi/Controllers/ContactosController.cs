using Asp.Versioning;
using ContactosApi.Domain;
using ContactosApi.Models;
using ContactosApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactosApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ContactosController : ControllerBase
{
    private readonly IContactoService _contactoService;
    private readonly ILogger<ContactosController> _logger;

    public ContactosController(IContactoService contactoService, ILogger<ContactosController> logger)
    {
        _contactoService = contactoService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Contacto>> ObtenerTodos()
    {
        return Ok(_contactoService.ObtenerTodos());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Contacto> ObtenerPorId(int id)
    {
        var contacto = _contactoService.ObtenerPorId(id);
        if (contacto is null)
        {
            _logger.LogWarning("Contacto con Id {ContactoId} no encontrado", id);
            return NotFound(new ErrorResponse { Mensaje = "Contacto no encontrado." });
        }

        return Ok(contacto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<Contacto> Crear([FromBody] CrearContactoRequest? request)
    {
        if (request is null)
        {
            return BadRequest(new ErrorResponse { Mensaje = "El cuerpo de la solicitud es obligatorio." });
        }

        var result = _contactoService.Crear(request);
        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = result.Value!.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0" },
                result.Value);
        }

        if (result.ErrorKind == ResultErrorKind.Conflict)
        {
            _logger.LogWarning("Conflicto al crear contacto: {Error}", result.Error);
            return Conflict(new ErrorResponse { Mensaje = result.Error! });
        }

        return BadRequest(new ErrorResponse { Mensaje = result.Error! });
    }
}
