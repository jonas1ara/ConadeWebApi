using AccesoDatos.Models;
using AccesoDatos.Operations;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudWS : ControllerBase
    {
        SolicitudDao dao = new SolicitudDao();

        [HttpPost("Guardar")]
        public Respuesta Guardar([FromBody] Solicitud solicitud)
        {
            return dao.Guardar(solicitud);
        }

        [HttpGet("ObtenerTodas")]
        public List<Solicitud> ObtenerTodas()
        {
            return dao.ObtenerTodas();
        }

        [HttpPut("Actualizar")]
        public Respuesta Actualizar(int id, [FromBody] Solicitud solicitudActualizada)
        {
            return dao.Actualizar(id, solicitudActualizada);
        }

        [HttpDelete("Eliminar")]
        public Respuesta Eliminar(int id)
        {
            return dao.Eliminar(id);
        }
    }
}
