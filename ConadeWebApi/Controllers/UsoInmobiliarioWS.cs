using AccesoDatos.Models;
using AccesoDatos.Operations;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsoInmobiliarioWS : ControllerBase
    {
        UsoInmobiliarioDao dao = new UsoInmobiliarioDao();

        [HttpGet("Listar")]
        public List<UsoInmobiliario> Listar()
        {
            return dao.Listar();
        }

        [HttpPost("Guardar")]
        public Respuesta Guardar(
            int solicitudId, string sala, int catalogoId,
            DateOnly fechaInicio, DateOnly fechaFin,
            TimeSpan horarioInicio, TimeSpan horarioFin,
            string? observaciones)
        {
            return dao.Guardar(solicitudId, sala, catalogoId, fechaInicio, fechaFin, horarioInicio, horarioFin, observaciones);
        }

        [HttpPut("Actualizar")]
        public Respuesta Actualizar(int id, [FromBody] UsoInmobiliario usoActualizado)
        {
            return dao.Actualizar(id, usoActualizado);
        }

        [HttpDelete("Eliminar")]
        public Respuesta Eliminar(int id)
        {
            return dao.Eliminar(id);
        }
    }
}
