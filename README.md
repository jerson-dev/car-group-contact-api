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

## Endpoints (v1)

| Método | Ruta | Descripción | Códigos |
|--------|------|-------------|---------|
| GET | `/api/v1/contactos` | Listar contactos | 200 |
| GET | `/api/v1/contactos/{id}` | Obtener por ID | 200, 404 |
| POST | `/api/v1/contactos` | Crear contacto | 201, 400, 409 |

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
| Controller → Service → Repository | Alineado con separación de responsabilidades |
| `ConcurrentDictionary` + lock en creación | Thread-safe para lecturas concurrentes y creación atómica |
| `Result<T>` en creación | Validación y duplicados sin excepciones en flujo normal |
| `ILogger<T>` estructurado | Trazabilidad de operaciones y errores sin dependencias extra |
| `ErrorHandlingMiddleware` | Respuestas 500 uniformes sin exponer stack traces |
| Versionado con `Asp.Versioning.Mvc` 8.x | Rutas `/api/v1/` preparadas para evolución de la API |
| xUnit + Moq + WebApplicationFactory | Estándar .NET para unit e integration tests |

## Estructura

```text
backend/
├── ContactosApi/
│   ├── Controllers/     # Solo HTTP
│   ├── Services/        # Lógica de negocio
│   ├── Domain/          # Entidades, Result y repositorio
│   ├── Middleware/      # Manejo global de errores
│   └── Models/          # DTOs
└── ContactosApi.Tests/
    ├── Unit/
    └── Integration/
```
