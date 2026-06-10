using ContactosApi.Domain;
using ContactosApi.Models;
using ContactosApi.Services;
using Microsoft.Extensions.Logging.Abstractions;
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
        var service = new ContactoService(repository.Object, NullLogger<ContactoService>.Instance);

        var result = service.Crear(new CrearContactoRequest
        {
            Nombre = "Juan Perez",
            Telefono = "123456789"
        });

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value!.Id);
        Assert.Equal("Juan Perez", result.Value.Nombre);
    }

    [Fact]
    public void Crear_CamposVacios_RetornaValidacion()
    {
        var repository = new Mock<IContactoRepository>();
        var service = new ContactoService(repository.Object, NullLogger<ContactoService>.Instance);

        var result = service.Crear(new CrearContactoRequest { Nombre = "   ", Telefono = "123" });

        Assert.False(result.IsSuccess);
        Assert.Equal(ResultErrorKind.Validation, result.ErrorKind);
        Assert.Contains("nombre", result.Error!, StringComparison.OrdinalIgnoreCase);
    }
}
