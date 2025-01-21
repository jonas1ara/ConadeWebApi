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
    public class EventoController : ControllerBase
    {
        private readonly EventosDao _dao;

        public EventoController(EventosDao dao)
        {
            _dao = dao;
        }

        // Crear un nuevo Evento
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearEvento(
            string numeroDeSerie,
            DateTime fechaSolicitud,
            int areaSolicitante,
            int usuarioSolicitante,
            string tipoSolicitud,
            string tipoServicio,
            string? sala,
            int catalogoId,
            string descripcionServicio,
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
                var idEvento = await _dao.CrearEventoAsync(
                    numeroDeSerie,
                    fechaSolicitud,
                    areaSolicitante,
                    usuarioSolicitante,
                    tipoSolicitud,
                    tipoServicio,
                    sala,
                    catalogoId,
                    descripcionServicio,
                    fechaInicioDateOnly,
                    fechaFinDateOnly,
                    horarioInicioTimeOnly,
                    horarioFinTimeOnly,
                    estado,
                    observaciones);

                if (idEvento == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Error al crear la solicitud.";
                    return NotFound(respuesta);
                }

                respuesta.success = true;
                respuesta.mensaje = "Evento creado correctamente.";
                respuesta.obj = idEvento;
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


        // Obtener todos los Eventos
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerEventos()
        {
            try
            {
                var eventos = await _dao.ObtenerEventosAsync();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Obtener un Evento por ID
        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerEventoPorId(int id)
        {
            try
            {
                var evento = await _dao.ObtenerEventosPorIdAsync(id);
                if (evento == null)
                {
                    return NotFound(new { success = false, message = "Evento no encontrado." });
                }
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // Actualizar un Evento
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarEvento(int id, [FromBody] Evento evento)
        {
            try
            {
                if (id != evento.Id)
                {
                    return BadRequest(new { success = false, message = "El ID del evento no coincide." });
                }

                var resultado = await _dao.ActualizarEventoAsync(evento);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Evento actualizado correctamente." });
                }
                return NotFound(new { success = false, message = "Evento no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Eliminar un Uso Inmobiliario
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            try
            {
                var resultado = await _dao.EliminarEventoAsync(id);
                if (resultado)
                {
                    return Ok(new { success = true, message = "Evento eliminado correctamente." });
                }
                return NotFound(new { success = false, message = "Evento no encontrado." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
