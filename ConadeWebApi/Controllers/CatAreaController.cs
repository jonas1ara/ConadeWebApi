using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Operations;
using AccesoDatos.Models.Conade1;
using System.Threading.Tasks;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatAreaController : ControllerBase
    {
        private readonly CatAreaDao _dao;

        public CatAreaController(CatAreaDao dao)
        {
            _dao = dao;
        }

        // Crear CatArea
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearCatArea(int? areaId, int? idCliente, string? clave, string? area, decimal? fuenteFinanciamiento)
        {
            try
            {
                var catAreaId = await _dao.CrearCatAreaAsync(areaId, idCliente, clave, area, fuenteFinanciamiento);
                return Ok(new { success = true, message = "CatArea creada correctamente.", catAreaId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Obtener todas las CatAreas
        [HttpGet("Listar")]
        public async Task<IActionResult> ObtenerCatAreas()
        {
            try
            {
                var catAreas = await _dao.ObtenerCatAreasAsync();
                return Ok(new { success = true, message = "CatAreas obtenidas correctamente.", catAreas });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Obtener CatArea por ID
        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> ObtenerCatAreaPorId(int id)
        {
            try
            {
                var catArea = await _dao.ObtenerCatAreaPorIdAsync(id);
                return Ok(new { success = true, message = "CatArea obtenida correctamente.", catArea });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Actualizar CatArea
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarCatArea(int id, int? areaId, int? idCliente, string? clave, string? area, decimal? fuenteFinanciamiento)
        {
            try
            {
                await _dao.ActualizarCatAreaAsync(id, areaId, idCliente, clave, area, fuenteFinanciamiento);
                return Ok(new { success = true, message = "CatArea actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar CatArea
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarCatArea(int id)
        {
            try
            {
                await _dao.EliminarCatAreaAsync(id);
                return Ok(new { success = true, message = "CatArea eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
