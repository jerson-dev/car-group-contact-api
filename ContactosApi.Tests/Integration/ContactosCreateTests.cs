using System.Net;
using System.Net.Http.Json;
using ContactosApi.Domain;
using ContactosApi.Models;

namespace ContactosApi.Tests.Integration;

public class ContactosCreateTests : IClassFixture<ContactosApiFactory>
{
    private readonly HttpClient _client;

    public ContactosCreateTests(ContactosApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostContacto_Valido_Retorna201()
    {
        var request = new CrearContactoRequest { Nombre = "Juan Perez", Telefono = "123456789" };

        var response = await _client.PostAsJsonAsync("/api/contactos", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var contacto = await response.Content.ReadFromJsonAsync<Contacto>();
        Assert.NotNull(contacto);
        Assert.Equal(1, contacto.Id);
        Assert.Equal("Juan Perez", contacto.Nombre);
        Assert.Equal("123456789", contacto.Telefono);
    }

    [Fact]
    public async Task PostContacto_CamposVacios_Retorna400()
    {
        var request = new CrearContactoRequest { Nombre = "", Telefono = "123" };

        var response = await _client.PostAsJsonAsync("/api/contactos", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Contains("nombre", error.Mensaje, StringComparison.OrdinalIgnoreCase);
    }
}
