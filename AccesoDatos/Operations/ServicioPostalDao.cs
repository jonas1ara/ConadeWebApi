using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Operations
{
    public class ServicioPostalDao
    {
        private readonly Conade1Context _context;

        public ServicioPostalDao(Conade1Context context)
        {
            _context = context;
        }

        public async Task<int?> CrearServicioPostalAsync(
                        string numeroDeSerie,
                        DateTime fechaSolicitud,
                        int areaSolicitante,
                        int usuarioSolicitante,
                        string tipoDeServicio,
                        string tipoSolicitud,
                        int catalogoId,
                        DateOnly fechaEnvio,
                        DateOnly FechaRecepcion,
                        string estado,
                        string? descripcionServicio = null,
                        string? observaciones = null)
        {
            // Validar tipo de solicitud (debe ser 'Servicio Postal')
            if (tipoSolicitud != "Servicio Postal")
            {
                throw new ArgumentException("El tipo de solicitud debe ser 'Servicio Postal'.");
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
            if (fechaEnvio > FechaRecepcion)
            {
                throw new ArgumentException("La fecha de recepción maxima debe ser después o el mismo día de la fecha de envio.");
            }

            // Crear un nuevo objeto de servicio postal con los datos proporcionados
            var servicioPostal = new ServicioPostal
            {
                NumeroDeSerie = numeroDeSerie,
                FechaSolicitud = fechaSolicitud,
                AreaSolicitante = areaSolicitante,
                UsuarioSolicitante = usuarioSolicitante,
                TipoDeServicio = tipoDeServicio,  // Aquí se ajusta al tipo de servicio
                TipoSolicitud = tipoSolicitud,
                CatalogoId = catalogoId,
                FechaEnvio = fechaEnvio,  // Se usa DateOnly directamente
                FechaRecepcion = FechaRecepcion,  // Se usa DateOnly directamente
                DescripcionServicio = descripcionServicio,
                AreaId = 1,  // Se ajusta al valor por defecto
                Estado = estado,
                Observaciones = observaciones
            };

            // Agregar el nuevo servicio postal a la base de datos
            _context.ServicioPostals.Add(servicioPostal);
            await _context.SaveChangesAsync();

            // Retornar el ID del nuevo servicio postal creado
            return servicioPostal.Id;
        }




        // Obtener todos los Servicios Postales
        public async Task<List<ServicioPostal>> ObtenerServiciosPostalesAsync()
        {
            return await _context.ServicioPostals.ToListAsync();
        }

        // Obtener Servicios Postales por Usuario
        public async Task<List<ServicioPostal>> ObtenerServiciosPostalesPorUsuarioAsync(int usuarioId)
        {
            return await _context.ServicioPostals
                .Where(s => s.UsuarioSolicitante == usuarioId)
                .ToListAsync();
        }

        // Obtener un Servicio Postal por ID
        public async Task<ServicioPostal> ObtenerServicioPostalPorIdAsync(int id)
        {
            return await _context.ServicioPostals
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Actualizar un Servicio Postal
        public async Task<bool> ActualizarServicioPostalAsync(ServicioPostal servicioPostal)
        {
            var existingServicioPostal = await _context.ServicioPostals
                .FirstOrDefaultAsync(s => s.Id == servicioPostal.Id);

            if (existingServicioPostal == null)
            {
                return false;
            }

            // Actualizamos los campos del servicio postal
            existingServicioPostal.FechaSolicitud = servicioPostal.FechaSolicitud;
            existingServicioPostal.AreaSolicitante = servicioPostal.AreaSolicitante;
            existingServicioPostal.UsuarioSolicitante = servicioPostal.UsuarioSolicitante;
            existingServicioPostal.CatalogoId = servicioPostal.CatalogoId;
            existingServicioPostal.FechaEnvio = servicioPostal.FechaEnvio;
            existingServicioPostal.FechaRecepcion = servicioPostal.FechaRecepcion;
            existingServicioPostal.DescripcionServicio = servicioPostal.DescripcionServicio;
            existingServicioPostal.Estado = servicioPostal.Estado;
            existingServicioPostal.Observaciones = servicioPostal.Observaciones;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar un Servicio Postal
        public async Task<bool> EliminarServicioPostalAsync(int id)
        {
            var servicioPostal = await _context.ServicioPostals
                .FirstOrDefaultAsync(s => s.Id == id);

            if (servicioPostal == null)
            {
                return false;
            }

            _context.ServicioPostals.Remove(servicioPostal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
