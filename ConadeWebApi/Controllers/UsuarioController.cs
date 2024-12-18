using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Operations;
using AccesoDatos.Models.Conade1;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioDao _dao;

        // Inyectar UsuarioDao a través del constructor
        public UsuarioController(UsuarioDao dao)
        {
            _dao = dao;
        }


        [HttpPost("Crear")]
        public async Task<IActionResult> CrearUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string claveEmpleado, string nombreUsuario, string contrasena, string rol, int? areaId)
        {
            // Crear un objeto de respuesta
            var respuesta = new Respuesta();

            try
            {
                // Llamar al método de creación de usuario y obtener el ID del nuevo usuario
                var idUsuario = await _dao.CrearUsuarioAsync(nombre, apellidoPaterno, apellidoMaterno, claveEmpleado, nombreUsuario, contrasena, rol, areaId);

                // Si el ID es nulo, el empleado no fue encontrado
                if (idUsuario == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Empleado no encontrado en la base de datos nominaO.";
                    return NotFound(respuesta);
                }

                // Si el ID es diferente de nulo, el usuario fue creado con éxito
                respuesta.success = true;
                respuesta.mensaje = "Usuario creado correctamente.";
                respuesta.obj = idUsuario;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolver un StatusCode 500 con el mensaje de la excepción
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Responde con un código de error 500
            }
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string nombreUsuario, string contrasena)
        {
            var respuesta = new Respuesta();

            try
            {
                // Llamar al método de login y obtener el usuario
                var usuario = await _dao.LoginAsync(nombreUsuario, contrasena);

                if (usuario == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Usuario no encontrado.";
                    return NotFound(respuesta);
                }

                // Crear el principal para la cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Role, usuario.Rol),  // Puedes agregar más roles si lo deseas
                    new Claim("IdUsuario", usuario.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Crear la cookie de sesión
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                respuesta.success = true;
                respuesta.mensaje = "Login exitoso.";
                respuesta.obj = usuario;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Eliminar la cookie de sesión
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var respuesta = new Respuesta
                {
                    success = true,
                    mensaje = "Logout exitoso."
                };

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                var respuesta = new Respuesta
                {
                    success = false,
                    mensaje = ex.Message
                };
                return StatusCode(500, respuesta);
            }
        }




        [HttpGet("Listar")]
        public Respuesta Listar()
        {
            return _dao.ObtenerUsuarios();
        }

        [HttpGet("ObtenerUsuarioConEmpleado")]
        public async Task<IActionResult> ObtenerUsuarioConEmpleado(string nombreUsuario)
        {
            var respuesta = new Respuesta();

            try
            {
                // Obtener el usuario y empleado asociado usando el nombreUsuario
                var datosUsuarioEmpleado = await _dao.ObtenerUsuarioConEmpleadoAsync(nombreUsuario);

                respuesta.success = true;
                respuesta.mensaje = "Datos obtenidos correctamente.";
                respuesta.obj = datosUsuarioEmpleado;
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                respuesta.obj = null;
            }

            return Ok(respuesta);
        }

        [HttpGet("ListarEmpleados")]
        public async Task<IActionResult> ListarEmpleados([FromQuery] string? nombre = null, [FromQuery] string? claveEmpleado = null)
        {
            try
            {
                // Llama al método en la capa de datos
                var empleados = await _dao.ListarEmpleadosAsync(nombre, claveEmpleado);

                // Devuelve los empleados encontrados en una respuesta exitosa
                return Ok(new { success = true, mensaje = "Empleados listados correctamente.", obj = empleados });
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { success = false, mensaje = ex.Message });
            }
        }


        [HttpDelete("Eliminar")]
        public IActionResult EliminarUsuario(string nombreUsuario)
        {
            try
            {
                // Llamar al método de eliminación y obtener la respuesta
                var respuesta = _dao.EliminarUsuarioPorNombreUsuario(nombreUsuario);

                // Si el éxito es false, devolver un NotFound con el mensaje
                if (!respuesta.success)
                {
                    return NotFound(respuesta);
                }

                // Si el usuario es eliminado con éxito, devolver la respuesta
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolver un StatusCode 500 con el mensaje de la excepción
                var respuesta = new Respuesta
                {
                    success = false,
                    mensaje = ex.Message
                };
                return StatusCode(500, respuesta); // Responde con un código de error 500
            }
        }

        [HttpGet("SolicitudesPorUsuario/{usuarioId}")]
        public async Task<IActionResult> SolicitudesPorUsuario(int usuarioId)
        {
            var respuesta = new Respuesta();

            try
            {
                // Llamar al método de UsuarioDao para obtener las solicitudes del usuario
                var solicitudes = await _dao.SolicitudesPorUsuarioAsync(usuarioId);

                if (solicitudes.success == false)
                {
                    return NotFound(solicitudes);  // Si no se encuentran solicitudes, devolvemos NotFound
                }

                // Si se encuentran solicitudes, devolverlas con el mensaje de éxito
                return Ok(solicitudes);  // Responde con las solicitudes encontradas
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = "Error al obtener las solicitudes: " + ex.Message;
                return StatusCode(500, respuesta);  // Si ocurre un error, responde con un código 500
            }
        }

        [HttpDelete("EliminarSolicitud")]
        public async Task<IActionResult> EliminarSolicitud(int idSolicitud, int usuarioId)
        {
            var respuesta = await _dao.EliminarSolicitudPorIdAsync(idSolicitud, usuarioId);

            if (respuesta.success)
            {
                return Ok(respuesta);
            }
            else
            {
                return NotFound(respuesta);
            }
        }

        [HttpDelete("EliminarSolicitudAdmin")]
        public async Task<IActionResult> EliminarSolicitudAdmin(int idSolicitud, int usuarioId)
        {
            var respuesta = await _dao.EliminarSolicitudPorIdAdminAsync(idSolicitud, usuarioId);

            if (respuesta.success)
            {
                return Ok(respuesta);
            }
            else
            {
                return NotFound(respuesta);
            }
        }

        [HttpPost("AprobarRechazarSolicitud")]
        public async Task<IActionResult> AprobarRechazarSolicitud(int idSolicitud, int usuarioId, string accion, string observaciones)
        {
            // Validar los parámetros de entrada
            if (idSolicitud <= 0 || usuarioId <= 0 || string.IsNullOrWhiteSpace(accion) || string.IsNullOrWhiteSpace(observaciones))
            {
                return BadRequest(new { success = false, mensaje = "Los parámetros proporcionados no son válidos." });
            }

            try
            {
                // Llamar al método de lógica de negocio
                var respuesta = await _dao.AprobarRechazarSolicitudAsync(idSolicitud, usuarioId, accion, observaciones);

                // Evaluar la respuesta
                if (respuesta.success)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return NotFound(respuesta);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                return StatusCode(500, new { success = false, mensaje = "Ocurrió un error interno: " + ex.Message });
            }
        }






    }
}
