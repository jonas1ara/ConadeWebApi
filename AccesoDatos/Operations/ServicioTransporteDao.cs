using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Operations
{
    public class ServicioTransporteDao
    {
        private readonly Conade1Context _context;

        public ServicioTransporteDao(Conade1Context context)
        {
            _context = context;
        }

        // Crear un nuevo Servicio de Transporte

        public async Task<int?> CrearServicioTransporteAsync(
        string numeroDeSerie,
        DateTime fechaSolicitud,
        int areaSolicitante,
        int usuarioSolicitante,
        string tipoDeServicio,
        string tipoSolicitud,
        int catalogoId,
        DateOnly? fechaTransporte,
        DateOnly? fechaTransporteVuelta,
        string origen,
        string destino,
        string? descripcionServicio,
        string estado,
        string? observaciones = null)
        {
            // Validar tipo de solicitud (debe ser 'Servicio Transporte')
            if (tipoSolicitud != "Servicio Transporte")
            {
                throw new ArgumentException("El tipo de solicitud debe ser 'Servicio Transporte'.");
            }

            // Validar tipo de servicio (debe ser uno de los valores permitidos)
            var tiposServicioPermitidos = new[] { "Llevar", "Recoger", "Llevar y Recoger" };
            if (!tiposServicioPermitidos.Contains(tipoDeServicio))
            {
                throw new ArgumentException("El tipo de servicio debe ser 'Llevar', 'Recoger' o 'Llevar y Recoger'.");
            }

            // Validar estado (debe ser uno de los valores permitidos)
            var estadosPermitidos = new[] { "Solicitada", "Atendida", "Rechazada" };
            if (!estadosPermitidos.Contains(estado))
            {
                throw new ArgumentException("El estado debe ser 'Solicitada', 'Atendida' o 'Rechazada'.");
            }

            // Validar que la fecha de envío sea antes que la fecha de recepción máxima
            if (fechaTransporte > fechaTransporteVuelta)
            {
                throw new ArgumentException("La fecha de vuelta debe ser después o el mismo día de la fecha de ida.");
            }

            // Crear un nuevo objeto de servicio de transporte con los datos proporcionados
            var servicioTransporte = new ServicioTransporte
            {
                NumeroDeSerie = numeroDeSerie,
                FechaSolicitud = fechaSolicitud,
                AreaSolicitante = areaSolicitante,
                UsuarioSolicitante = usuarioSolicitante,
                TipoDeServicio = tipoDeServicio,
                TipoSolicitud = tipoSolicitud,
                CatalogoId = catalogoId,
                FechaTransporte = fechaTransporte,
                FechaTransporteVuelta = fechaTransporteVuelta,
                Origen = origen,
                Destino = destino,
                DescripcionServicio = descripcionServicio,
                AreaId = 2, // Valor por defecto
                Estado = estado,
                Observaciones = observaciones
            };

            // Agregar el nuevo servicio de transporte a la base de datos
            _context.ServicioTransportes.Add(servicioTransporte);
            await _context.SaveChangesAsync();

            // Retornar el ID del nuevo servicio de transporte creado
            return servicioTransporte.Id;
        }


        // Obtener todos los Servicios de Transporte
        public async Task<List<ServicioTransporte>> ObtenerServiciosTransporteAsync()
        {
            return await _context.ServicioTransportes.ToListAsync();
        }

        // Obtener Servicios de Transporte por Usuario
        public async Task<List<ServicioTransporte>> ObtenerServiciosTransportePorUsuarioAsync(int usuarioId)
        {
            return await _context.ServicioTransportes
                .Where(s => s.UsuarioSolicitante == usuarioId)
                .ToListAsync();
        }

        // Obtener un Servicio de Transporte por ID
        public async Task<ServicioTransporte> ObtenerServicioTransportePorIdAsync(int id)
        {
            return await _context.ServicioTransportes
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Actualizar un Servicio de Transporte
        public async Task<bool> ActualizarServicioTransporteAsync(ServicioTransporte servicioTransporte)
        {
            var existingServicioTransporte = await _context.ServicioTransportes
                .FirstOrDefaultAsync(s => s.Id == servicioTransporte.Id);

            if (existingServicioTransporte == null)
            {
                return false;
            }

            // Actualizamos los campos del servicio de transporte
            existingServicioTransporte.FechaSolicitud = servicioTransporte.FechaSolicitud;
            existingServicioTransporte.AreaSolicitante = servicioTransporte.AreaSolicitante;
            existingServicioTransporte.UsuarioSolicitante = servicioTransporte.UsuarioSolicitante;
            existingServicioTransporte.CatalogoId = servicioTransporte.CatalogoId;
            existingServicioTransporte.FechaTransporte = servicioTransporte.FechaTransporte;
            existingServicioTransporte.Origen = servicioTransporte.Origen;
            existingServicioTransporte.Destino = servicioTransporte.Destino;
            existingServicioTransporte.Estado = servicioTransporte.Estado;
            existingServicioTransporte.Observaciones = servicioTransporte.Observaciones;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar un Servicio de Transporte
        public async Task<bool> EliminarServicioTransporteAsync(int id)
        {
            var servicioTransporte = await _context.ServicioTransportes
                .FirstOrDefaultAsync(s => s.Id == id);

            if (servicioTransporte == null)
            {
                return false;
            }

            _context.ServicioTransportes.Remove(servicioTransporte);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
