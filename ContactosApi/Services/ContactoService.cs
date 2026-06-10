using ContactosApi.Domain;
using ContactosApi.Domain.Exceptions;
using ContactosApi.Models;

namespace ContactosApi.Services;

public class ContactoService : IContactoService
{
    private readonly IContactoRepository _repository;
    private readonly ILogger<ContactoService> _logger;

    public ContactoService(IContactoRepository repository, ILogger<ContactoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public IReadOnlyList<Contacto> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Contacto? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Result<Contacto> Crear(CrearContactoRequest request)
    {
        var nombreResult = NormalizarCampo(request.Nombre, "nombre");
        if (!nombreResult.IsSuccess)
        {
            _logger.LogWarning("Validación fallida al crear contacto: {Error}", nombreResult.Error);
            return Result<Contacto>.ValidationFailure(nombreResult.Error!);
        }

        var telefonoResult = NormalizarCampo(request.Telefono, "teléfono");
        if (!telefonoResult.IsSuccess)
        {
            _logger.LogWarning("Validación fallida al crear contacto: {Error}", telefonoResult.Error);
            return Result<Contacto>.ValidationFailure(telefonoResult.Error!);
        }

        try
        {
            var contacto = _repository.Agregar(nombreResult.Value!, telefonoResult.Value!);
            _logger.LogInformation("Contacto creado con Id {ContactoId}", contacto.Id);
            return Result<Contacto>.Success(contacto);
        }
        catch (ContactoDuplicateException ex)
        {
            _logger.LogWarning("Intento de crear contacto con teléfono duplicado: {Telefono}", telefonoResult.Value);
            return Result<Contacto>.ConflictFailure(ex.Message);
        }
    }

    private static Result<string> NormalizarCampo(string? valor, string nombreCampo)
    {
        if (valor is null || string.IsNullOrWhiteSpace(valor))
        {
            return Result<string>.ValidationFailure($"El campo {nombreCampo} es obligatorio.");
        }

        return Result<string>.Success(valor.Trim());
    }
}
