using ContactosApi.Domain;

namespace ContactosApi.Domain;

public interface IContactoRepository
{
    IReadOnlyList<Contacto> ObtenerTodos();

    Contacto? ObtenerPorId(int id);

    Contacto Agregar(string nombre, string telefonoNormalizado);
}
