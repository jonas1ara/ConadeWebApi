using AccesoDatos.Operations;
using AccesoDatos.Models.Conade1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClasesBase.Respuestas;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsoInmobiliarioController : ControllerBase
    {
        private readonly UsoInmobiliarioDao _dao;

        public UsoInmobiliarioController(UsoInmobiliarioDao dao)
        {
            _dao = dao;
        }

        // Crear un nuevo Uso Inmobiliario
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearUsoInmobiliario(
            string numeroDeSerie,
            DateTime fechaSolicitud,
            int areaSolicitante,
            int usuarioSolicitante,
            string tipoSolicitud,
            string sala,
            int catalogoId,
            string fechaInicio, // Usar string en lugar de DateOnly
            string? fechaFin,
            string horarioInicio, // Usar string en lugar de TimeOnly
            string horarioFin,    // Usar string en lugar de TimeOnly
            string estado = "Solicitada",
            string? observaciones = null)
        {
            var respuesta = new Respuesta();

            try
            {
                // Convertir la fecha de string a DateOnly
                DateOnly fechaInicioDateOnly = DateOnly.Parse(fechaInicio);
                DateOnly? fechaFinDateOnly = string.IsNullOrEmpty(fechaFin) ? null : DateOnly.Parse(fechaFin);

                // Convertir horarios de string a TimeOnly
                TimeOnly horarioInicioTimeOnly = TimeOnly.Parse(horarioInicio);
                TimeOnly horarioFinTimeOnly = TimeOnly.Parse(horarioFin);

                // Llamar al método de creación de UsoInmobiliario y obtener el ID del nuevo registro
                var idUsoInmobiliario = await _dao.CrearUsoInmobiliarioAsync(
                    numeroDeSerie,
                    fechaSolicitud,
                    areaSolicitante,
                    usuarioSolicitante,
                    tipoSolicitud,
                    sala,
                    catalogoId,
                    fechaInicioDateOnly,
                    fechaFinDateOnly,
                    horarioInicioTimeOnly,
                    horarioFinTimeOnly,
                    estado,
                    observaciones);

                if (idUsoInmobiliario == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Error al crear la solicitud.";
                    return NotFound(respuesta);
                }

                respuesta.success = true;
                respuesta.mensaje = "Uso Inmobiliario creado correctamente.";
                respuesta.obj = idUsoInmobiliario;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = ex.Message;
                return StatusCode(500, respuesta); // Responde con un código de error 500
            }
        }


        // Obtener todos los Uso Inmobiliarios
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerUsoInmobiliarios()
        {
            try
            {
                var usoInmobiliarios = await _dao.ObtenerUsoInmobiliariosAsync();
                return Ok(usoInmobiliarios);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Obtener un Uso Inmobiliario por ID
        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerUsoInmobiliarioPorId(int id)
        {
            try
            {
                var usoInmobiliario = await _dao.ObtenerUsoInmobiliarioPorIdAsync(id);
                if (usoInmobiliario == null)
                {
                    return NotFound(new { success = false, message = "Uso Inmobiliario no encontrado." });
                }
                return Ok(usoInmobiliario);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Actualizar un Uso Inmobiliario
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarUsoInmobiliario(int id, [FromBody] UsoInmobiliario usoInmobiliario)
        {
            try
            {
                if (id != usoInmobiliario.Id)
                {
                    return BadRequest(new { success = false, message = "El ID del uso inmobiliario no coincide." });
                }

                var resultado = await _dao.ActualizarUsoInmobiliarioAsync(usoInmobiliario);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Uso Inmobiliario actualizado correctamente." });
                }
                return NotFound(new { success = false, message = "Uso Inmobiliario no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar un Uso Inmobiliario
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarUsoInmobiliario(int id)
        {
            try
            {
                var resultado = await _dao.EliminarUsoInmobiliarioAsync(id);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Uso Inmobiliario eliminado correctamente." });
                }
                return NotFound(new { success = false, message = "Uso Inmobiliario no encontrado." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
