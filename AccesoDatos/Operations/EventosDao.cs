using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class EventosDao
    {
        private readonly Conade1Context _context;

        // Constructor
        public EventosDao(Conade1Context context)
        {
            _context = context;
        }

        // Crear un nuevo Evento
        public async Task<int?> CrearEventoAsync(
                string numeroDeSerie,
                DateTime fechaSolicitud,
                int areaSolicitante,
                int usuarioSolicitante,
                string tipoSolicitud,
                string tipoServicio,
                string? sala,
                int catalogoId,
                string descripcionServicio,
                DateOnly fechaInicio,
                DateOnly? fechaFin,
                TimeOnly horarioInicio,
                TimeOnly horarioFin,
                string estado,
                string? observaciones = null)
        {
            // Validar tipo de solicitud (debe ser 'Uso Inmobiliario')
            if (tipoSolicitud != "Eventos")
            {
                throw new ArgumentException("El tipo de solicitud debe ser 'Eventos'.");
            }

            // Validar estado (debe ser uno de los valores permitidos)
            var estadosPermitidos = new[] { "Solicitada", "Atendida", "Rechazada" };
            if (!estadosPermitidos.Contains(estado))
            {
                throw new ArgumentException("El estado debe ser 'Solicitada', 'Atendida' o 'Rechazada'.");
            }

            // Validar que la fecha de Inicio sea mayor a la fecha de fin
            if (fechaInicio > fechaFin)
            {
                throw new ArgumentException("La fecha de fin debe ser después o el mismo día de la fecha de inicio.");
            }

            if (horarioInicio >= horarioFin)
            {
                throw new ArgumentException("El horario de inicio no puede ser posterior o igual al horario de fin.");
            }

            // Crear un nuevo objeto de evento con los datos proporcionados
            var evento = new Evento
            {
                NumeroDeSerie = numeroDeSerie,
                FechaSolicitud = fechaSolicitud,
                AreaSolicitante = areaSolicitante,
                UsuarioSolicitante = usuarioSolicitante,
                TipoSolicitud = tipoSolicitud,
                TipoServicio = tipoServicio,
                Sala = sala,
                CatalogoId = catalogoId,
                DescripcionServicio = descripcionServicio,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                HorarioInicio = horarioInicio,
                HorarioFin = horarioFin,
                AreaId = 3, // Se asigna un valor por defecto
                Estado = estado,
                Observaciones = observaciones
            };

            // Agregar el nuevo evento a la base de datos
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            // Retornar el ID del nuevo evento creado
            return evento.Id;
        }


        // Obtener todos los Eventos
        public async Task<List<Evento>> ObtenerEventosAsync()
        {
            return await _context.Eventos.ToListAsync();
        }

        // Obtener un Uso Inmobiliario por ID
        public async Task<Evento> ObtenerEventosPorIdAsync(int id)
        {
            return await _context.Eventos
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Actualizar un Uso Inmobiliario
        public async Task<bool> ActualizarEventoAsync(Evento evento)
        {
            var existingEvento = await _context.Eventos
                .FirstOrDefaultAsync(u => u.Id == evento.Id);

            if (existingEvento == null)
            {
                return false;
            }

            // Actualizamos los campos del uso inmobiliario
            existingEvento.FechaSolicitud = evento.FechaSolicitud;
            existingEvento.AreaSolicitante = evento.AreaSolicitante;
            existingEvento.UsuarioSolicitante = evento.UsuarioSolicitante;
            existingEvento.CatalogoId = evento.CatalogoId;
            existingEvento.FechaInicio = evento.FechaInicio;
            existingEvento.FechaFin = evento.FechaFin;
            existingEvento.HorarioInicio = evento.HorarioInicio;
            existingEvento.HorarioFin = evento.HorarioFin;
            existingEvento.Estado = evento.Estado;
            existingEvento.Observaciones = evento.Observaciones;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar un Evento
        public async Task<bool> EliminarEventoAsync(int id)
        {
            var evento = await _context.Eventos
                .FirstOrDefaultAsync(u => u.Id == id);

            if (evento == null)
            {
                return false;
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
