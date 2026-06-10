using System.Net;
using System.Net.Http.Json;
using ContactosApi.Models;

namespace ContactosApi.Tests.Integration;

public class ContactosDuplicateTests : IClassFixture<ContactosApiFactory>
{
    private readonly HttpClient _client;

    public ContactosDuplicateTests(ContactosApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostContacto_TelefonoDuplicado_Retorna409()
    {
        var request = new CrearContactoRequest { Nombre = "Juan Perez", Telefono = "123456789" };
        await _client.PostAsJsonAsync("/api/contactos", request);

        var duplicate = new CrearContactoRequest { Nombre = "Otro", Telefono = "123456789" };
        var response = await _client.PostAsJsonAsync("/api/contactos", duplicate);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Contains("teléfono", error.Mensaje, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PostContacto_TelefonoDuplicadoConTrim_Retorna409()
    {
        var request = new CrearContactoRequest { Nombre = "Ana", Telefono = "987654321" };
        await _client.PostAsJsonAsync("/api/contactos", request);

        var duplicate = new CrearContactoRequest { Nombre = "Bob", Telefono = " 987654321 " };
        var response = await _client.PostAsJsonAsync("/api/contactos", duplicate);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
