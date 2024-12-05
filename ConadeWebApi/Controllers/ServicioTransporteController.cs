using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccesoDatos.Operations;
using AccesoDatos.Models.Conade1;
using Microsoft.AspNetCore.Mvc;
using ClasesBase.Respuestas;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioTransporteController : ControllerBase
    {
        private readonly ServicioTransporteDao _dao;

        public ServicioTransporteController(ServicioTransporteDao dao)
        {
            _dao = dao;
        }

        // Crear un nuevo Servicio de Transporte
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearServicioTransporte(
        string numeroDeSerie,
        DateTime fechaSolicitud,
        int areaSolicitante,
        int usuarioSolicitante,
        string tipoSolicitud,
        string tipoDeServicio,
        int catalogoId,
        string fechaTransporte, // Usar string en lugar de DateOnly
        string? fechaTransporteVuelta,
        string origen,
        string destino,
        string? descripcionServicio = null,
        string estado = "Solicitada",
        string? observaciones = null)
        {
            var respuesta = new Respuesta();

            try
            {
                // Convertir la fecha de string a DateOnly
                DateOnly fechaTransporteDateOnly = DateOnly.Parse(fechaTransporte);
                DateOnly? fechaTransporteVueltaDateOnly = DateOnly.Parse(fechaTransporteVuelta); ;

                // Llamar al método de creación de servicio de transporte y obtener el ID del nuevo servicio
                var idServicioTransporte = await _dao.CrearServicioTransporteAsync(
                    numeroDeSerie,
                    fechaSolicitud,
                    areaSolicitante,
                    usuarioSolicitante,
                    tipoDeServicio,
                    tipoSolicitud,
                    catalogoId,
                    fechaTransporteDateOnly,
                    fechaTransporteVueltaDateOnly,
                    origen,
                    destino,
                    descripcionServicio,
                    estado,
                    observaciones);

                if (idServicioTransporte == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Error al crear la solicitud.";
                    return NotFound(respuesta);
                }

                respuesta.success = true;
                respuesta.mensaje = "Servicio de Transporte creado correctamente.";
                respuesta.obj = idServicioTransporte;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Responde con un código de error 500
            }
        }



        // Obtener todos los Servicios de Transporte
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerServiciosTransporte()
        {
            try
            {
                var serviciosTransporte = await _dao.ObtenerServiciosTransporteAsync();
                return Ok(serviciosTransporte);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Obtener Servicios de Transporte por Usuario
        [HttpGet("ObtenerPorUsuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerServiciosTransportePorUsuario(int usuarioId)
        {
            try
            {
                var serviciosTransporte = await _dao.ObtenerServiciosTransportePorUsuarioAsync(usuarioId);
                return Ok(serviciosTransporte);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Obtener un Servicio de Transporte por ID
        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerServicioTransportePorId(int id)
        {
            try
            {
                var servicioTransporte = await _dao.ObtenerServicioTransportePorIdAsync(id);
                if (servicioTransporte == null)
                {
                    return NotFound(new { success = false, message = "Servicio de Transporte no encontrado." });
                }
                return Ok(servicioTransporte);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Actualizar un Servicio de Transporte
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarServicioTransporte(int id, [FromBody] ServicioTransporte servicioTransporte)
        {
            try
            {
                if (id != servicioTransporte.Id)
                {
                    return BadRequest(new { success = false, message = "El ID del servicio de transporte no coincide." });
                }

                var resultado = await _dao.ActualizarServicioTransporteAsync(servicioTransporte);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Servicio de Transporte actualizado correctamente." });
                }
                return NotFound(new { success = false, message = "Servicio de Transporte no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar un Servicio de Transporte
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarServicioTransporte(int id)
        {
            try
            {
                var resultado = await _dao.EliminarServicioTransporteAsync(id);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Servicio de Transporte eliminado correctamente." });
                }
                return NotFound(new { success = false, message = "Servicio de Transporte no encontrado." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
