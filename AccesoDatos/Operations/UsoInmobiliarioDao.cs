using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class UsoInmobiliarioDao
    {
        private readonly Conade1Context _context;

        // Constructor
        public UsoInmobiliarioDao(Conade1Context context)
        {
            _context = context;
        }

        // Crear un nuevo Uso Inmobiliario
        public async Task<int?> CrearUsoInmobiliarioAsync(
                string numeroDeSerie,
                DateTime fechaSolicitud,
                int areaSolicitante,
                int usuarioSolicitante,
                string tipoSolicitud,
                string sala,
                int catalogoId,
                DateOnly fechaInicio,
                DateOnly? fechaFin,
                TimeOnly horarioInicio,
                TimeOnly horarioFin,
                string estado,
                string? observaciones = null)
        {
            // Validar tipo de solicitud (debe ser 'Uso Inmobiliario')
            if (tipoSolicitud != "Uso Inmobiliario")
            {
                throw new ArgumentException("El tipo de solicitud debe ser 'Uso Inmobiliario'.");
            }

            // Validar estado (debe ser uno de los valores permitidos)
            var estadosPermitidos = new[] { "Solicitada", "Atendida", "Rechazada" };
            if (!estadosPermitidos.Contains(estado))
            {
                throw new ArgumentException("El estado debe ser 'Solicitada', 'Atendida' o 'Rechazada'.");
            }

            // Crear un nuevo objeto de UsoInmobiliario con los datos proporcionados
            var usoInmobiliario = new UsoInmobiliario
            {
                NumeroDeSerie = numeroDeSerie,
                FechaSolicitud = fechaSolicitud,
                AreaSolicitante = areaSolicitante,
                UsuarioSolicitante = usuarioSolicitante,
                TipoSolicitud = tipoSolicitud,
                Sala = sala,
                CatalogoId = catalogoId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                HorarioInicio = horarioInicio,
                HorarioFin = horarioFin,
                AreaId = 3, // Se asigna un valor por defecto
                Estado = estado,
                Observaciones = observaciones
            };

            // Agregar el nuevo UsoInmobiliario a la base de datos
            _context.UsoInmobiliarios.Add(usoInmobiliario);
            await _context.SaveChangesAsync();

            // Retornar el ID del nuevo UsoInmobiliario creado
            return usoInmobiliario.Id;
        }


        // Obtener todos los Uso Inmobiliarios
        public async Task<List<UsoInmobiliario>> ObtenerUsoInmobiliariosAsync()
        {
            return await _context.UsoInmobiliarios.ToListAsync();
        }

        // Obtener un Uso Inmobiliario por ID
        public async Task<UsoInmobiliario> ObtenerUsoInmobiliarioPorIdAsync(int id)
        {
            return await _context.UsoInmobiliarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Actualizar un Uso Inmobiliario
        public async Task<bool> ActualizarUsoInmobiliarioAsync(UsoInmobiliario usoInmobiliario)
        {
            var existingUsoInmobiliario = await _context.UsoInmobiliarios
                .FirstOrDefaultAsync(u => u.Id == usoInmobiliario.Id);

            if (existingUsoInmobiliario == null)
            {
                return false;
            }

            // Actualizamos los campos del uso inmobiliario
            existingUsoInmobiliario.FechaSolicitud = usoInmobiliario.FechaSolicitud;
            existingUsoInmobiliario.AreaSolicitante = usoInmobiliario.AreaSolicitante;
            existingUsoInmobiliario.UsuarioSolicitante = usoInmobiliario.UsuarioSolicitante;
            existingUsoInmobiliario.CatalogoId = usoInmobiliario.CatalogoId;
            existingUsoInmobiliario.FechaInicio = usoInmobiliario.FechaInicio;
            existingUsoInmobiliario.FechaFin = usoInmobiliario.FechaFin;
            existingUsoInmobiliario.HorarioInicio = usoInmobiliario.HorarioInicio;
            existingUsoInmobiliario.HorarioFin = usoInmobiliario.HorarioFin;
            existingUsoInmobiliario.Estado = usoInmobiliario.Estado;
            existingUsoInmobiliario.Observaciones = usoInmobiliario.Observaciones;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar un Uso Inmobiliario
        public async Task<bool> EliminarUsoInmobiliarioAsync(int id)
        {
            var usoInmobiliario = await _context.UsoInmobiliarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usoInmobiliario == null)
            {
                return false;
            }

            _context.UsoInmobiliarios.Remove(usoInmobiliario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
