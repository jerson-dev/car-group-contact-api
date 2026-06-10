using System.Net;
using System.Net.Http.Json;
using ContactosApi.Domain;
using ContactosApi.Models;

namespace ContactosApi.Tests.Integration;

public class ContactosReadTests : IClassFixture<ContactosApiFactory>
{
    private readonly HttpClient _client;

    public ContactosReadTests(ContactosApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetContactos_ListaVacia_Retorna200()
    {
        var response = await _client.GetAsync("/api/contactos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contactos = await response.Content.ReadFromJsonAsync<List<Contacto>>();
        Assert.NotNull(contactos);
        Assert.Empty(contactos);
    }

    [Fact]
    public async Task GetContactoPorId_NoExiste_Retorna404()
    {
        var response = await _client.GetAsync("/api/contactos/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.False(string.IsNullOrWhiteSpace(error.Mensaje));
    }
}
