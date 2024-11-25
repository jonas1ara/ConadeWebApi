using AccesoDatos.Operations;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioWS : ControllerBase
    {
        UsuarioDao dao = new UsuarioDao();

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            return Ok(new { Message = "Este endpoint está protegido." });
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet("AdminOnly")]
        //public IActionResult GetAdminData()
        //{
        //    return Ok(new { Message = "Solo administradores pueden acceder a este endpoint." });
        //}


        // Endpoint para guardar un usuario
        [HttpPost("Guardar")]
        public Respuesta Guardar(string nombreUsuario, string correoUsuario)
        {
            return dao.guardarUsuario(nombreUsuario, correoUsuario);
        }

        // Endpoint para obtener todos los usuarios
        [HttpGet("Listar")]
        public Respuesta Listar()
        {
            return dao.obtenerUsuarios();
        }

        // Endpoint para actualizar un usuario
        [HttpPut("Actualizar")]
        public Respuesta Actualizar(int idUsuario, string nuevoNombre, string nuevoCorreo)
        {
            return dao.actualizarUsuario(idUsuario, nuevoNombre, nuevoCorreo);
        }

        // Endpoint para eliminar un usuario
        [HttpDelete("Eliminar")]
        public Respuesta Eliminar(int idUsuario)
        {
            return dao.eliminarUsuario(idUsuario);
        }
    }
}
