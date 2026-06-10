using ContactosApi.Domain.Exceptions;
using ContactosApi.Models;
using ContactosApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactosController : ControllerBase
{
    private readonly IContactoService _contactoService;

    public ContactosController(IContactoService contactoService)
    {
        _contactoService = contactoService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Domain.Contacto>> ObtenerTodos()
    {
        return Ok(_contactoService.ObtenerTodos());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Domain.Contacto> ObtenerPorId(int id)
    {
        var contacto = _contactoService.ObtenerPorId(id);
        if (contacto is null)
        {
            return NotFound(new ErrorResponse { Mensaje = "Contacto no encontrado." });
        }

        return Ok(contacto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<Domain.Contacto> Crear([FromBody] CrearContactoRequest? request)
    {
        if (request is null)
        {
            return BadRequest(new ErrorResponse { Mensaje = "El cuerpo de la solicitud es obligatorio." });
        }

        try
        {
            var contacto = _contactoService.Crear(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = contacto.Id }, contacto);
        }
        catch (ContactoValidationException ex)
        {
            return BadRequest(new ErrorResponse { Mensaje = ex.Message });
        }
        catch (ContactoDuplicateException ex)
        {
            return Conflict(new ErrorResponse { Mensaje = ex.Message });
        }
    }
}
