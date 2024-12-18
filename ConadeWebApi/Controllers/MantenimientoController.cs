using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Operations;
using AccesoDatos.Models.Conade1;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using ClasesBase.Respuestas;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenimientoController : ControllerBase
    {
        private readonly MantenimientoDao _dao;

        public MantenimientoController(MantenimientoDao dao)
        {
            _dao = dao;
        }

        // Crear un nuevo Mantenimiento
        // Crear un nuevo Mantenimiento
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearMantenimiento(
            string numeroDeSerie,
            DateTime fechaSolicitud,
            int areaSolicitante,
            int usuarioSolicitante,
            string tipoSolicitud,
            string tipoServicio,
            int catalogoId,
            string descripcionServicio,
            DateTime fechaInicio,
            DateTime? fechaEntrega,
            string estado = "Solicitada",
            string observaciones = ""
            )
        {
            var respuesta = new Respuesta();

            try
            {
                // Llamar al método de creación de Mantenimiento y obtener el ID del nuevo registro
                var idMantenimiento = await _dao.CrearMantenimientoAsync(
                    numeroDeSerie,
                    fechaSolicitud,
                    areaSolicitante,
                    usuarioSolicitante,
                    tipoSolicitud,
                    tipoServicio,
                    catalogoId,
                    descripcionServicio,
                    fechaInicio,
                    fechaEntrega,
                    estado,
                    observaciones);

                if (idMantenimiento == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Error al crear la solicitud.";
                    return NotFound(respuesta);
                }

                respuesta.success = true;
                respuesta.mensaje = "Mantenimiento creado correctamente.";
                respuesta.obj = idMantenimiento;
                return Ok(respuesta);
            }
            catch (ArgumentException ex)
            {
                // Si la excepción es de tipo ArgumentException, devolvemos un mensaje detallado
                respuesta.success = false;
                respuesta.mensaje = ex.Message; // Este mensaje será el del error de fecha o cualquier otro
                return BadRequest(respuesta); // 400 para errores de validación
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Responde con un código de error 500
            }
        }

        [HttpGet("ObtenerPorUsuario/{usuarioSolicitanteId}")]
        public async Task<IActionResult> ObtenerMantenimientosPorUsuario(int usuarioSolicitanteId)
        {
            var respuesta = new Respuesta();

            try
            {
                // Llamar al DAO para obtener las solicitudes de mantenimiento
                var mantenimientos = await _dao.ObtenerMantenimientosPorUsuarioAsync(usuarioSolicitanteId);

                if (mantenimientos == null || mantenimientos.Count == 0)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "No se encontraron solicitudes de mantenimiento para este usuario.";
                    return NotFound(respuesta);
                }

                respuesta.success = true;
                respuesta.mensaje = "Solicitudes de mantenimiento obtenidas correctamente.";
                respuesta.obj = mantenimientos;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Error en el servidor
            }
        }

        // Obtener todos los Mantenimientos
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerMantenimientos()
        {
            try
            {
                var mantenimientos = await _dao.ObtenerMantenimientosAsync();
                return Ok(mantenimientos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        // Obtener un Mantenimiento por ID
        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> ObtenerMantenimientoPorId(int id)
        {
            try
            {
                var mantenimiento = await _dao.ObtenerMantenimientoPorIdAsync(id);
                if (mantenimiento == null)
                {
                    return NotFound(new { success = false, message = "Mantenimiento no encontrado." });
                }

                return Ok(mantenimiento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Actualizar un Mantenimiento
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarMantenimiento(int id, [FromBody] Mantenimiento mantenimiento)
        {
            try
            {
                if (id != mantenimiento.Id)
                {
                    return BadRequest(new { success = false, message = "El ID del mantenimiento no coincide." });
                }

                bool actualizado = await _dao.ActualizarMantenimientoAsync(mantenimiento);
                if (!actualizado)
                {
                    return NotFound(new { success = false, message = "Mantenimiento no encontrado." });
                }

                return Ok(new { success = true, message = "Mantenimiento actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar Mantenimiento
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarMantenimiento(int id)
        {
            try
            {
                bool eliminado = await _dao.EliminarMantenimientoAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { success = false, message = "Mantenimiento no encontrado." });
                }

                return Ok(new { success = true, message = "Mantenimiento eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
