using AccesoDatos.Operations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ClasesBase.Respuestas;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        private readonly SolicitudDao _dao;

        // Constructor
        public SolicitudController(SolicitudDao dao)
        {
            _dao = dao;
        }

        // Obtener todas las solicitudes de acuerdo al área administrada por el Admin
        [HttpGet("ObtenerPorArea/{areaId}")]
        public async Task<IActionResult> ObtenerSolicitudesPorArea(int areaId)
        {
            var respuesta = new Respuesta();

            try
            {
                // Llamar al DAO para obtener las solicitudes según el área
                var solicitudes = await _dao.ObtenerSolicitudesPorAreaAsync(areaId);

                if (solicitudes == null || solicitudes.Count == 0)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "No se encontraron solicitudes para este área.";
                    return NotFound(respuesta);
                }

                respuesta.success = true;
                respuesta.mensaje = "Solicitudes obtenidas correctamente.";
                respuesta.obj = solicitudes;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Error en el servidor
            }
        }
    }
}
