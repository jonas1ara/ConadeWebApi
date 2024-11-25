# ConadeWebApi

Este repositorio contiene una solución de ASP.NET Core desarrollada en una arquitectura MVC, diseñada para administrar diferentes solicitudes en el sistema interno de la CONADE (Comisión Nacional de Cultura Física y Deporte).

## Modelo de la Base de Datos

A continuación se muestra el diagrama del modelo de la base de datos utilizado en este proyecto:

![Diagrama del Modelo de la Base de Datos](GitHub/Diagram_Conade.png)


## Estructura del proyecto 

La solución incluye tres proyectos dentro de una arquitectura modular:

1. **AccesoDatos**: Biblioteca de clases para gestionar modelos y operaciones relacionadas con la base de datos.
2. **ClasesBase**: Biblioteca de clases base, como modelos de respuesta estándar.
3. **ConadeWebApi**: API principal que implementa los controladores para interactuar con las entidades y operaciones.

### AccesoDatos

 - **Models**: Define las entidades del sistema, como `Usuario`, `Solicitud`, `Mantenimiento`, etc.
 - **Operations**: Contiene los DAO (Data Access Objects) para operaciones CRUD sobre las entidades.

### ClasesBase

 - **Respuestas**: Contiene clases base reutilizables, como `Respuesta`.

### ConadeWebApi

 - **Controllers**: Define los controladres que exponen los endpoints de la API RESTful.
 - **Program.cs**: Configuración inicial del proyecto, incluyendo inyección de dependencias y middleware. 

## Requisitos

Asegúrate de tener instalado lo siguiente:

- [Visual Studio 2022](https://visualstudio.microsoft.com/es/) con soporte para desarrollo de ASP.NET y herramientas de Entity Framework.
- [SDK .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0).
- SQL Server configurado localmente o en un contenedor.
- [Git](https://git-scm.com/).

## Configuración Inicial

Sigue estos pasos para clonar y configurar el proyecto:

### 1. Clonar el Repositorio

Clona este repositorio en tu máquina local:

```bash
git clone https://github.com/tu-usuario/ConadeWebApi.git
cd ConadeWebApi
```

### 2. Restaurar Paquetes NuGet

Restaura los paquetes necesarios con el siguiente comando:

```bash
dotnet restore
```

Los paquetes NuGet que utilizan para la relación con la base de datos son:

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`

Los paquetes NuGet que se utilizan para manejar JWT son:

- `Microsoft.IdentityModel.Tokens`
- `Microsoft.AspNetCore.Authentication.JwtBearer` 

Debido a que el proyecto utiliza .NET 8, los paquetes para manejar JWT deben estar en la version 8 para garantizar la compatibilidad.

Los paquetes NuGet que se utilizan para ConfigurationBuilder son:

 - `Microsoft.Extensions.Configuration`
 - `Microsoft.Extensions.Configuration.Json`
 - `Microsoft.Extensions.DependencyInjection`

### 3. Configurar la Base de Datos

El proyecto utiliza **Entity Framework Core** para interactuar con la base de datos. Configura tu cadena de conexión en el archivo `appsettings.json` del proyecto `ConadeWebApi`, por el momento se encuentra configurado para SQL Server local, pero puedes cambiarlo a tu conveniencia en el archivo de configuración se agregan dos  `DevelopmentConnection` y `ProductionConnection` para que puedas cambiar entre ambientes de desarrollo y producción.

```json
{
  "ConnectionStrings": {
    "DevelopmentConnection": "Server=hp\\SQLEXPRESS; Encrypt=False; TrustServerCertificate=True; Database=Conade1; Integrated Security=True",
    "ProductionConnection": "Server=prod-server-name;Database=ConadeDb;User Id=your_user;Password=your_password;Encrypt=True;TrustServerCertificate=False"
  }
}
```

Recuerda cambiar el valor de `connectionString` por la cadena de conexión de tu base de datos, esta variable se encuentra en `AccesoDatos/Conade1Context.cs` en el método `OnConfiguring`.

Para aplicar las migraciones para crear la base de datos, utiliza estos comandos:

#### Desde dotnet CLI en la terminal:

```
dotnet ef database update --project AccesoDatos --startup-project ConadeWebApi
```

#### Desde NuGet Package Manager Console en Visual Studio:

```
Update-Database -Project AccesoDatos -StartupProject ConadeWebApi
```

### 4. Ejecutar el proyecto


#### Desde dotnet CLI en la terminal:

```
dotnet run --project ConadeWebApi
```

#### Desde Visual Studio:

 1. Abre la solución `ConadeWebApi.sln` en Visual Studio.
 2. Establece el proyecto `ConadeWebApi` como proyecto de inicio.
 3. Presiona `Ctrl + F5` para ejecutar el proyecto.

### 5. Probar la API

La API estará disponible en el siguiente URL por defecto:

```bash
https://localhost:5001
```

Puedes probar los endpoints utilizando:

 - **Postman**: Crea una colección y prueba los endpoints.
 - **Swagger**: Documentación generada automáticamente en:

```bash
https://localhost:5001/swagger/index.html
```

### 6. Colaborar

Si deseas colaborar al proyecto, sigue estos pasos:

 1. Haz un fork del repositorio.
 2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
 3. Realiza tus cambios y haz commit (`git commit -m "Agregada nueva funcionalidad"`).
 4. Haz push a la rama (`git push origin feature/nueva-funcionalidad`).
 5. Crea un pull request en este repositorio.




