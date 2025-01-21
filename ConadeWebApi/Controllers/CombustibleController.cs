using AccesoDatos.Operations;
using AccesoDatos.Models.Conade1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ClasesBase.Respuestas;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombustibleController : ControllerBase
    {
        private readonly CombustibleDao _dao;

        public CombustibleController(CombustibleDao dao)
        {
            _dao = dao;
        }

        // Crear un nuevo Combustible
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearCombustible(
            string numeroDeSerie,
            DateTime fechaSolicitud,
            int areaSolicitante,
            int usuarioSolicitante,
            string tipoSolicitud,
            string tipoCombustible,
            int litros,
            int catalogoId,
            string? descripcionServicio,
            DateTime fecha,
            string estado = "Solicitada",
            string? observaciones = null)
        {
            var respuesta = new Respuesta();

            try
            {
                // Llamar al método de creación de Combustible y obtener el ID del nuevo registro
                var idCombustible = await _dao.CrearCombustibleAsync(
                    numeroDeSerie,
                    fechaSolicitud,
                    areaSolicitante,
                    usuarioSolicitante,
                    tipoSolicitud,
                    tipoCombustible,
                    litros,
                    catalogoId,
                    descripcionServicio,
                    fecha,
                    estado,
                    observaciones);

                if (idCombustible == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Error al crear la solicitud de combustible.";
                    return NotFound(respuesta);
                }

                respuesta.success = true;
                respuesta.mensaje = "Combustible creado correctamente.";
                respuesta.obj = idCombustible;
                return Ok(respuesta);
            }
            catch (ArgumentException ex)
            {
                // Si la excepción es de tipo ArgumentException, devolvemos un mensaje detallado
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return BadRequest(respuesta); // 400 para errores de validación
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Responde con un código de error 500
            }
        }

        // Obtener todos los Combustibles
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerCombustibles()
        {
            try
            {
                var combustibles = await _dao.ObtenerCombustiblesAsync();
                return Ok(combustibles);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Obtener un Combustible por ID
        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerCombustiblePorId(int id)
        {
            try
            {
                var combustible = await _dao.ObtenerCombustiblePorIdAsync(id);
                if (combustible == null)
                {
                    return NotFound(new { success = false, message = "Combustible no encontrado." });
                }
                return Ok(combustible);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Actualizar un Combustible
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarCombustible(int id, [FromBody] Combustible combustible)
        {
            try
            {
                if (id != combustible.Id)
                {
                    return BadRequest(new { success = false, message = "El ID del combustible no coincide." });
                }

                var resultado = await _dao.ActualizarCombustibleAsync(combustible);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Combustible actualizado correctamente." });
                }
                return NotFound(new { success = false, message = "Combustible no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar un Combustible
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarCombustible(int id)
        {
            try
            {
                var resultado = await _dao.EliminarCombustibleAsync(id);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Combustible eliminado correctamente." });
                }
                return NotFound(new { success = false, message = "Combustible no encontrado." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
