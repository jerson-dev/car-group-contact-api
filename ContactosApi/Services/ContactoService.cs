using ContactosApi.Domain;
using ContactosApi.Domain.Exceptions;
using ContactosApi.Models;

namespace ContactosApi.Services;

public class ContactoService : IContactoService
{
    private readonly IContactoRepository _repository;

    public ContactoService(IContactoRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Contacto> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Contacto? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Contacto Crear(CrearContactoRequest request)
    {
        var nombre = NormalizarCampo(request.Nombre, "nombre");
        var telefono = NormalizarCampo(request.Telefono, "teléfono");

        return _repository.Agregar(nombre, telefono);
    }

    private static string NormalizarCampo(string? valor, string nombreCampo)
    {
        if (valor is null || string.IsNullOrWhiteSpace(valor))
        {
            throw new ContactoValidationException($"El campo {nombreCampo} es obligatorio.");
        }

        return valor.Trim();
    }
}
