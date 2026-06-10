using ContactosApi.Domain;
using ContactosApi.Domain.Exceptions;

namespace ContactosApi.Tests.Unit;

public class ContactoRepositoryConcurrencyTests
{
    [Fact]
    public async Task Agregar_Concurrente_IdsUnicos()
    {
        var repository = new InMemoryContactoRepository();
        var tasks = Enumerable.Range(1, 20)
            .Select(i => Task.Run(() => repository.Agregar($"Contacto {i}", $"600000{i:D3}")))
            .ToArray();

        await Task.WhenAll(tasks);

        var contactos = repository.ObtenerTodos();
        Assert.Equal(20, contactos.Count);
        Assert.Equal(20, contactos.Select(c => c.Id).Distinct().Count());
    }

    [Fact]
    public async Task Agregar_MismoTelefonoConcurrente_Uno201Equivalente()
    {
        var repository = new InMemoryContactoRepository();
        var barrier = new Barrier(2);
        Contacto? created = null;
        var duplicates = 0;

        var tasks = Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
        {
            barrier.SignalAndWait();
            try
            {
                var contacto = repository.Agregar("Test", "111222333");
                Interlocked.CompareExchange(ref created, contacto, null);
            }
            catch (ContactoDuplicateException)
            {
                Interlocked.Increment(ref duplicates);
            }
        })).ToArray();

        await Task.WhenAll(tasks);

        Assert.NotNull(created);
        Assert.Equal(1, duplicates);
        Assert.Single(repository.ObtenerTodos());
    }
}
