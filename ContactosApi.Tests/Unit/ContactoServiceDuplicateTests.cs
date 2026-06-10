using ContactosApi.Domain;
using ContactosApi.Domain.Exceptions;
using ContactosApi.Models;
using ContactosApi.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ContactosApi.Tests.Unit;

public class ContactoServiceDuplicateTests
{
    [Fact]
    public void Crear_TelefonoDuplicado_RetornaConflicto()
    {
        var repository = new Mock<IContactoRepository>();
        repository
            .Setup(r => r.Agregar("Otro", "123456789"))
            .Throws(new ContactoDuplicateException("El teléfono ya está registrado."));
        var service = new ContactoService(repository.Object, NullLogger<ContactoService>.Instance);

        var result = service.Crear(new CrearContactoRequest { Nombre = "Otro", Telefono = "123456789" });

        Assert.False(result.IsSuccess);
        Assert.Equal(ResultErrorKind.Conflict, result.ErrorKind);
    }

    [Fact]
    public void Crear_TelefonoDuplicadoConTrim_RetornaConflicto()
    {
        var repository = new Mock<IContactoRepository>();
        repository
            .Setup(r => r.Agregar("Bob", "987654321"))
            .Throws(new ContactoDuplicateException("El teléfono ya está registrado."));
        var service = new ContactoService(repository.Object, NullLogger<ContactoService>.Instance);

        var result = service.Crear(new CrearContactoRequest { Nombre = "Bob", Telefono = " 987654321 " });

        Assert.False(result.IsSuccess);
        Assert.Equal(ResultErrorKind.Conflict, result.ErrorKind);
    }
}
