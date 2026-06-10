using ContactosApi.Domain;
using ContactosApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "API de Contactos", Version = "v1" });
});

builder.Services.AddSingleton<IContactoRepository, InMemoryContactoRepository>();
builder.Services.AddScoped<IContactoService, ContactoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;
