using ContactosApi.Domain;
using ContactosApi.Domain.Exceptions;
using ContactosApi.Models;
using ContactosApi.Services;
using Moq;

namespace ContactosApi.Tests.Unit;

public class ContactoServiceCreateTests
{
    [Fact]
    public void Crear_ContactoValido_RetornaConId()
    {
        var repository = new Mock<IContactoRepository>();
        repository
            .Setup(r => r.Agregar("Juan Perez", "123456789"))
            .Returns(new Contacto(1, "Juan Perez", "123456789"));
        var service = new ContactoService(repository.Object);

        var result = service.Crear(new CrearContactoRequest
        {
            Nombre = "Juan Perez",
            Telefono = "123456789"
        });

        Assert.Equal(1, result.Id);
        Assert.Equal("Juan Perez", result.Nombre);
    }

    [Fact]
    public void Crear_CamposVacios_LanzaValidacion()
    {
        var repository = new Mock<IContactoRepository>();
        var service = new ContactoService(repository.Object);

        var exception = Assert.Throws<ContactoValidationException>(() =>
            service.Crear(new CrearContactoRequest { Nombre = "   ", Telefono = "123" }));

        Assert.Contains("nombre", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
