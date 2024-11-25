using AccesoDatos.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDao dao;
        private readonly IConfiguration conf;

        public AuthController(UsuarioDao usuarioDao, IConfiguration configuration)
        {
            dao = usuarioDao;
            conf = configuration;
        }

        [HttpPost("Register")]
        public IActionResult Register(string nombre, string correo, string password, string rol = "User")
        {
            if (dao.GetUsuarioByCorreo(correo) != null)
            {
                return BadRequest(new { Message = "El correo ya está registrado." });
            }

            dao.RegistrarUsuario(nombre, correo, password, rol);
            return Ok(new { Message = "Usuario registrado exitosamente." });
        }

        [HttpPost("Login")]
        public IActionResult Login(string correo, string password)
        {
            // Obtener el usuario por correo
            var usuario = dao.GetUsuarioByCorreo(correo);

            // Verificar si el usuario existe y la contraseña es válida
            if (usuario == null || !dao.VerificarPassword(correo, password))
            {
                return Unauthorized(new { Message = "Correo o contraseña incorrectos." });
            }

            // Obtener la clave secreta de la configuración
            var secretKey = conf["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                return StatusCode(500, new { Message = "Clave secreta no configurada." });
            }

            // Crear los claims que estarán dentro del token JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo), // El correo como sujeto
                new Claim(ClaimTypes.Name, usuario.Nombre), // Nombre del usuario
                new Claim(ClaimTypes.Role, usuario.Rol), // Rol del usuario
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Identificador único del token (JTI)
            };

            // Asegurarse de que la clave secreta tiene una longitud adecuada (256 bits)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));  // Clave secreta desde la configuración
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear el token JWT con los parámetros deseados
            var token = new JwtSecurityToken(
                issuer: conf["Jwt:Issuer"], // Emisor del token
                audience: conf["Jwt:Audience"], // Audiencia (puede ser nula si no se requiere)
                claims: claims, // Claims generados anteriormente
                expires: DateTime.UtcNow.AddHours(1), // Expiración del token (1 hora)
                signingCredentials: creds); // Firmado con las credenciales

            // Devolver el token JWT como parte de la respuesta
            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

    }
}
