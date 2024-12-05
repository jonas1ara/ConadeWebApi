using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Operations;
using AccesoDatos.Models.Conade1;
using System.Threading.Tasks;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly AreaDao _dao;

        public AreaController(AreaDao dao)
        {
            _dao = dao;
        }

        // Crear área
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearArea(string nombre)
        {
            try
            {
                var areaId = await _dao.CrearAreaAsync(nombre);
                return Ok(new { success = true, message = "Área creada correctamente.", areaId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Obtener todas las áreas
        [HttpGet("Listar")]
        public async Task<IActionResult> ObtenerAreas()
        {
            try
            {
                var areas = await _dao.ObtenerAreasAsync();
                return Ok(new { success = true, message = "Áreas obtenidas correctamente.", areas });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Obtener área por ID
        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> ObtenerAreaPorId(int id)
        {
            try
            {
                var area = await _dao.ObtenerAreaPorIdAsync(id);
                return Ok(new { success = true, message = "Área obtenida correctamente.", area });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Actualizar área
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarArea(int id, string nombre)
        {
            try
            {
                await _dao.ActualizarAreaAsync(id, nombre);
                return Ok(new { success = true, message = "Área actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar área
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarArea(int id)
        {
            try
            {
                await _dao.EliminarAreaAsync(id);
                return Ok(new { success = true, message = "Área eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
