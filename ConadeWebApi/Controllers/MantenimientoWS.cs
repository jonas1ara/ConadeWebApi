using AccesoDatos.Models;
using AccesoDatos.Operations;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenimientoWS : ControllerBase
    {
        MantenimientoDao dao = new MantenimientoDao();

        [HttpPost("Guardar")]
        public Respuesta Guardar([FromBody] Mantenimiento mantenimiento)
        {
            return dao.Guardar(mantenimiento);
        }

        [HttpGet("ObtenerTodos")]
        public List<Mantenimiento> ObtenerTodos()
        {
            return dao.ObtenerTodos();
        }

        [HttpPut("Actualizar")]
        public Respuesta Actualizar(int id, [FromBody] Mantenimiento mantenimientoActualizado)
        {
            return dao.Actualizar(id, mantenimientoActualizado);
        }

        [HttpDelete("Eliminar")]
        public Respuesta Eliminar(int id)
        {
            return dao.Eliminar(id);
        }
    }
}
