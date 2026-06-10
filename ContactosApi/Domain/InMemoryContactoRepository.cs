using System.Collections.Concurrent;
using ContactosApi.Domain.Exceptions;

namespace ContactosApi.Domain;

public class InMemoryContactoRepository : IContactoRepository
{
    private readonly ConcurrentDictionary<int, Contacto> _contactos = new();
    private readonly ConcurrentDictionary<string, int> _telefonos = new(StringComparer.Ordinal);
    private int _nextId;
    private readonly object _createLock = new();

    public IReadOnlyList<Contacto> ObtenerTodos()
    {
        return _contactos.Values.OrderBy(c => c.Id).ToList();
    }

    public Contacto? ObtenerPorId(int id)
    {
        return _contactos.TryGetValue(id, out var contacto) ? contacto : null;
    }

    public Contacto Agregar(string nombre, string telefonoNormalizado)
    {
        lock (_createLock)
        {
            if (_telefonos.ContainsKey(telefonoNormalizado))
            {
                throw new ContactoDuplicateException("El teléfono ya está registrado.");
            }

            var id = Interlocked.Increment(ref _nextId);
            var contacto = new Contacto(id, nombre, telefonoNormalizado);
            _contactos[id] = contacto;
            _telefonos[telefonoNormalizado] = id;
            return contacto;
        }
    }
}
