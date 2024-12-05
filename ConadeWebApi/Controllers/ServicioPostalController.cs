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
    public class ServicioPostalController : ControllerBase
    {
        private readonly ServicioPostalDao _dao;

        public ServicioPostalController(ServicioPostalDao dao)
        {
            _dao = dao;
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> CrearServicioPostal(
                    string numeroDeSerie,
                    DateTime fechaSolicitud,
                    int areaSolicitante,
                    int usuarioSolicitante,
                    string tipoSolicitud,
                    string tipoDeServicio,
                    int catalogoId,
                    string fechaEnvio,   // Usar string en lugar de DateOnly
                    string fechaRecepcionMaxima,  // Usar string en lugar de DateOnly
                    string? descripcionServicio = null,
                    string estado = "Solicitada",
                    string? observaciones = null)
        {
            var respuesta = new Respuesta();

            try
            {
                // Convertir las fechas de string a DateOnly
                DateOnly fechaEnvioDateOnly = DateOnly.Parse(fechaEnvio);
                DateOnly fechaRecepcionMaximaDateOnly = DateOnly.Parse(fechaRecepcionMaxima);

                // Llamar al método de creación de servicio postal y obtener el ID del nuevo servicio postal
                var idServicioPostal = await _dao.CrearServicioPostalAsync(
                    numeroDeSerie,
                    fechaSolicitud,
                    areaSolicitante,
                    usuarioSolicitante,
                    tipoDeServicio,
                    tipoSolicitud,
                    catalogoId,
                    fechaEnvioDateOnly,
                    fechaRecepcionMaximaDateOnly,
                    estado,
                    descripcionServicio,
                    observaciones);

                // Si no se pudo crear, devolver un mensaje de error
                if (idServicioPostal == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Error al crear la solicitud.";
                    return NotFound(respuesta);
                }

                // Si la solicitud se crea correctamente, devolver una respuesta de éxito
                respuesta.success = true;
                respuesta.mensaje = "Servicio Postal creado correctamente.";
                respuesta.obj = idServicioPostal;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Responde con un código de error 500
            }
        }




        // Obtener todos los Servicios Postales
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerServiciosPostales()
        {
            try
            {
                var serviciosPostales = await _dao.ObtenerServiciosPostalesAsync();
                return Ok(serviciosPostales);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Obtener Servicios Postales por Usuario
        [HttpGet("ObtenerPorUsuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerServiciosPostalesPorUsuario(int usuarioId)
        {
            try
            {
                var serviciosPostales = await _dao.ObtenerServiciosPostalesPorUsuarioAsync(usuarioId);
                return Ok(serviciosPostales);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Obtener un Servicio Postal por ID
        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerServicioPostalPorId(int id)
        {
            try
            {
                var servicioPostal = await _dao.ObtenerServicioPostalPorIdAsync(id);
                if (servicioPostal == null)
                {
                    return NotFound(new { success = false, message = "Servicio Postal no encontrado." });
                }
                return Ok(servicioPostal);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Actualizar un Servicio Postal
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarServicioPostal(int id, [FromBody] ServicioPostal servicioPostal)
        {
            try
            {
                if (id != servicioPostal.Id)
                {
                    return BadRequest(new { success = false, message = "El ID del servicio postal no coincide." });
                }

                var resultado = await _dao.ActualizarServicioPostalAsync(servicioPostal);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Servicio Postal actualizado correctamente." });
                }
                return NotFound(new { success = false, message = "Servicio Postal no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar un Servicio Postal
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarServicioPostal(int id)
        {
            try
            {
                var resultado = await _dao.EliminarServicioPostalAsync(id);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Servicio Postal eliminado correctamente." });
                }
                return NotFound(new { success = false, message = "Servicio Postal no encontrado." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
