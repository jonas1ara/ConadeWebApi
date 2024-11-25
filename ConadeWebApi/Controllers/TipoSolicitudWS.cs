using AccesoDatos.Models;
using AccesoDatos.Operations;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoSolicitudWS : ControllerBase
    {
        TipoSolicitudDao dao = new TipoSolicitudDao();

        [HttpPost("Guardar")]
        public Respuesta Guardar(string nombreTipoSolicitud)
        {
            return dao.Guardar(nombreTipoSolicitud);
        }

        [HttpGet("ObtenerTodos")]
        public List<TipoSolicitud> ObtenerTodos()
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
