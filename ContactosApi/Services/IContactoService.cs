using ContactosApi.Domain;
using ContactosApi.Models;

namespace ContactosApi.Services;

public interface IContactoService
{
    IReadOnlyList<Contacto> ObtenerTodos();

    Contacto? ObtenerPorId(int id);

    Result<Contacto> Crear(CrearContactoRequest request);
}
