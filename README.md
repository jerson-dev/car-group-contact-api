# API de Contactos — Prueba Técnica Backend

API REST en .NET 8 para gestión de contactos con almacenamiento in-memory, thread-safe.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download) (compatible con .NET 6+)

## Instalación

```bash
cd backend
dotnet restore
dotnet build
```

## Ejecución

```bash
cd backend/ContactosApi
dotnet run
```

- API: `http://localhost:5xxx` o `https://localhost:7xxx` (ver consola)
- Swagger: `/swagger`

## Tests

```bash
cd backend
dotnet test
```

## Endpoints

| Método | Ruta | Descripción | Códigos |
|--------|------|-------------|---------|
| GET | `/api/contactos` | Listar contactos | 200 |
| GET | `/api/contactos/{id}` | Obtener por ID | 200, 404 |
| POST | `/api/contactos` | Crear contacto | 201, 400, 409 |

### Ejemplo POST

```json
{
  "nombre": "Juan Perez",
  "telefono": "123456789"
}
```

### Ejemplo error

```json
{
  "mensaje": "El teléfono ya está registrado."
}
```

## Decisiones técnicas

| Decisión | Justificación |
|----------|---------------|
| .NET 8 | LTS actual, cumple requisito .NET 6+ |
| Controller → Service → Repository | Alineado con constitución (separación de responsabilidades) |
| `ConcurrentDictionary` + lock en creación | Thread-safe para lecturas concurrentes y creación atómica |
| Excepciones de dominio | Mapeo claro a códigos HTTP sin over-engineering |
| Sin base de datos | Requisito explícito de la prueba técnica |
| xUnit + Moq + WebApplicationFactory | Estándar .NET para unit e integration tests |

## Estructura

```text
backend/
├── ContactosApi/
│   ├── Controllers/     # Solo HTTP
│   ├── Services/        # Lógica de negocio
│   ├── Domain/          # Entidades y repositorio
│   └── Models/          # DTOs
└── ContactosApi.Tests/
    ├── Unit/
    └── Integration/
```

## Documentación adicional

- [Spec](../specs/001-api-contactos-backend/spec.md)
- [Quickstart](../specs/001-api-contactos-backend/quickstart.md)
- [OpenAPI](../specs/001-api-contactos-backend/contracts/openapi.yaml)
