# Cinedex Backend

Backend ASP.NET Core 8 que sirve la API Cinedex.

## Requisitos previos

- .NET 8 SDK instalado.
- SQL Server accesible (LocalDB, Express o Developer) con permisos para crear la base Cinedex.
- dotnet-ef global tool (solo si vas a aplicar migraciones manualmente).

## Paquetes NuGet

- `AutoMapper` 15.0.1 – para mapear DTOs y entidades fácilmente.
- `BCrypt.Net-Core` 1.6.0 – para hashear y verificar contraseñas de manera segura.
- `Microsoft.AspNetCore.Authentication.JwtBearer` 8.0.20 – middleware JWT con ASP.NET Core.
- `Microsoft.EntityFrameworkCore.SqlServer` 9.0.9 – proveedor de EF Core para SQL Server.
- `Microsoft.EntityFrameworkCore.Tools` 9.0.9 – herramientas en tiempo de diseño para migraciones y scaffolding.
- `Swashbuckle.AspNetCore` 9.0.6 – genera la documentación Swagger/OpenAPI.

## 1. Clonar y restaurar paquetes

1. Clona el repositorio (ejemplo: git clone https://github.com/PikiSpitale/CinedexAPI).
2. Abre la terminal en la carpeta donde aparece proyecto prog4.sln.
3. Ejecuta:
   dotnet restore "proyecto prog4.sln"

## 2. Configurar la cadena de conexion y el secreto JWT

- El backend busca ConnectionStrings:devConnection para la base y Secrets:JWT para firmar tokens.
- Puedes editar appsettings.json o preferir user secrets / variables de entorno para no subir credenciales.

```json
{
  "ConnectionStrings": {
    "devConnection": "Server=localhost;Database=Cinedex;User Id=sa;Password=TuContraseña;TrustServerCertificate=True;"
  },
  "Secrets": {
    "JWT": "una-clave-muy-larga-para-firmar-tokens"
  }
}
```

- Con user secrets (el UserSecretsId ya esta en el proyecto) ejecuta desde la carpeta proyecto prog4:

```ps
dotnet user-secrets set "ConnectionStrings:devConnection" "Server=localhost;Database=Cinedex;User Id=sa;Password=TuContrasena;TrustServerCertificate=True;"
dotnet user-secrets set "Secrets:JWT" "una-clave-muy-larga-para-firmar-tokens"
```

## 3. Crear la base y aplicar migraciones

- Instala dotnet-ef si no lo tienes:
  dotnet tool install --global dotnet-ef --version 9.0.9
- Aplica las migraciones:
  dotnet ef database update --project "./proyecto prog4/proyecto prog4.csproj" --startup-project "./proyecto prog4/proyecto prog4.csproj"

## 4. Roles iniciales

- Crea manualmente en la base de datos `Cinedex` los roles `Admin`, `Mod` y `User` (p. ej. insertando en `Roles` o `AspNetRoles`, según tu modelo).
- El primer rol `Admin` debe ingresarse también de forma manual porque no hay datos semilla; asegúrate de que exista la fila con ese nombre antes de crear usuarios con permisos elevados.

## 5. Ejecutar el backend

Desde la carpeta raiz ejecuta:

dotnet run --project "./proyecto prog4/proyecto prog4.csproj"

El servidor expone Swagger en https://localhost:xxxx/swagger y permite peticiones desde http://localhost:5173 (CORS definido en Program.cs).

## 6. Verificar la API

- Abre Swagger para probar los endpoints y confirmar que el token JWT se genera y valida.
- Usa tu frontend o Postman contra el host indicado en la consola.
