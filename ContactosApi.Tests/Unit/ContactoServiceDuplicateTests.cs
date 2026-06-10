using ContactosApi.Domain;
using ContactosApi.Domain.Exceptions;
using ContactosApi.Models;
using ContactosApi.Services;
using Moq;

namespace ContactosApi.Tests.Unit;

public class ContactoServiceDuplicateTests
{
    [Fact]
    public void Crear_TelefonoDuplicado_LanzaConflicto()
    {
        var repository = new Mock<IContactoRepository>();
        repository
            .Setup(r => r.Agregar("Otro", "123456789"))
            .Throws(new ContactoDuplicateException("El teléfono ya está registrado."));
        var service = new ContactoService(repository.Object);

        Assert.Throws<ContactoDuplicateException>(() =>
            service.Crear(new CrearContactoRequest { Nombre = "Otro", Telefono = "123456789" }));
    }

    [Fact]
    public void Crear_TelefonoDuplicadoConTrim_LanzaConflicto()
    {
        var repository = new Mock<IContactoRepository>();
        repository
            .Setup(r => r.Agregar("Bob", "987654321"))
            .Throws(new ContactoDuplicateException("El teléfono ya está registrado."));
        var service = new ContactoService(repository.Object);

        Assert.Throws<ContactoDuplicateException>(() =>
            service.Crear(new CrearContactoRequest { Nombre = "Bob", Telefono = " 987654321 " }));
    }
}
