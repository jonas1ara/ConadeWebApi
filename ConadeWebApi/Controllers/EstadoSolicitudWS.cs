using AccesoDatos.Models;
using AccesoDatos.Operations;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoSolicitudWS : ControllerBase
    {
        EstadoSolicitudDao dao = new EstadoSolicitudDao();

        [HttpPost("Guardar")]
        public Respuesta Guardar(string nombreEstado)
        {
            return dao.Guardar(nombreEstado);
        }

        [HttpGet("ObtenerTodos")]
        public List<EstadoSolicitud> ObtenerTodos()
        {
            return dao.ObtenerTodos();
        }

        [HttpPut("Actualizar")]
        public Respuesta Actualizar(int id, string nuevoNombre)
        {
            return dao.Actualizar(id, nuevoNombre);
        }

        [HttpDelete("Eliminar")]
        public Respuesta Eliminar(int id)
        {
            return dao.Eliminar(id);
        }

    }
}
