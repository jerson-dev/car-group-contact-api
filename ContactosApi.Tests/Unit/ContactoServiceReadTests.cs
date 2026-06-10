using ContactosApi.Domain;
using ContactosApi.Services;
using Moq;

namespace ContactosApi.Tests.Unit;

public class ContactoServiceReadTests
{
    [Fact]
    public void ObtenerTodos_RetornaListaVacia()
    {
        var repository = new Mock<IContactoRepository>();
        repository.Setup(r => r.ObtenerTodos()).Returns(Array.Empty<Contacto>());
        var service = new ContactoService(repository.Object);

        var result = service.ObtenerTodos();

        Assert.Empty(result);
    }

    [Fact]
    public void ObtenerPorId_NoExiste_RetornaNull()
    {
        var repository = new Mock<IContactoRepository>();
        repository.Setup(r => r.ObtenerPorId(999)).Returns((Contacto?)null);
        var service = new ContactoService(repository.Object);

        var result = service.ObtenerPorId(999);

        Assert.Null(result);
    }
}
