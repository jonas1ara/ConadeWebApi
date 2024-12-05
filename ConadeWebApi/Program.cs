using AccesoDatos.Models.Conade1;
using AccesoDatos.Models.Nominas;
using AccesoDatos.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Agregar contextos de base de datos
builder.Services.AddDbContext<Conade1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conade1")));

builder.Services.AddDbContext<NominaOsimulacionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Nominas")));

// Registrar Dao's
builder.Services.AddScoped<AreaDao>();
builder.Services.AddScoped<CatAreaDao>();
builder.Services.AddScoped<MantenimientoDao>();
builder.Services.AddScoped<UsoInmobiliarioDao>();
builder.Services.AddScoped<UsuarioDao>();
builder.Services.AddScoped<ServicioPostalDao>();
builder.Services.AddScoped<ServicioTransporteDao>();



// Agregar servicios de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Establecer las rutas de Login y Logout
        options.LoginPath = "/api/usuario/Login";  // Ruta de login
        options.LogoutPath = "/api/usuario/Logout"; // Ruta de logout
        options.ExpireTimeSpan = TimeSpan.FromHours(1);  // Duración de la cookie
        options.SlidingExpiration = true;  // Renueva la cookie cuando esté cerca de expirar
        options.Cookie.HttpOnly = true;  // La cookie no estará accesible desde JavaScript
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Solo en HTTPS
    });

builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Conade API",
        Version = "2024.08.06-PreV1",
        Description = "API para el manejo de datos de Conade1 y Nominas",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "soporte@macroart.com"
        }
    });
});

builder.Services.AddCors(policyBuilder => policyBuilder.AddDefaultPolicy(policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Macro Art API v1");
    });
}

app.UseCors();

app.UseHttpsRedirection();

// Configurar el middleware de autenticación y autorización
app.UseAuthentication();  // Middleware de autenticación
app.UseAuthorization();   // Middleware de autorización

// Mapear los controladores
app.MapControllers();

app.Run();
