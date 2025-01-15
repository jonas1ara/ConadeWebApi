using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class CombustibleDao
    {
        private readonly Conade1Context _context;

        // Constructor
        public CombustibleDao(Conade1Context context)
        {
            _context = context;
        }

        // Crear un nuevo Combustible
        public async Task<int?> CrearCombustibleAsync(
                string numeroDeSerie,
                DateTime fechaSolicitud,
                int areaSolicitante,
                int usuarioSolicitante,
                string tipoSolicitud,
                int catalogoId,
                string? descripcionServicio,
                DateTime fecha,
                int? areaId,
                string estado,
                string? observaciones = null)
        {
            // Validar tipo de solicitud (debe ser 'Combustible')
            if (tipoSolicitud != "Combustible")
            {
                throw new ArgumentException("El tipo de solicitud debe ser 'Combustible'.");
            }

            // Validar estado (debe ser uno de los valores permitidos)
            var estadosPermitidos = new[] { "Solicitada", "Atendida", "Rechazada" };
            if (!estadosPermitidos.Contains(estado))
            {
                throw new ArgumentException("El estado debe ser 'Solicitada', 'Atendida' o 'Rechazada'.");
            }

            // Crear un nuevo objeto de combustible con los datos proporcionados
            var combustible = new Combustible
            {
                NumeroDeSerie = numeroDeSerie,
                FechaSolicitud = fechaSolicitud,
                AreaSolicitante = areaSolicitante,
                UsuarioSolicitante = usuarioSolicitante,
                TipoSolicitud = tipoSolicitud,
                CatalogoId = catalogoId,
                DescripcionServicio = descripcionServicio,
                Fecha = fecha,
                AreaId = areaId,
                Estado = estado,
                Observaciones = observaciones
            };

            // Agregar el nuevo combustible a la base de datos
            _context.Combustibles.Add(combustible);
            await _context.SaveChangesAsync();

            // Retornar el ID del nuevo combustible creado
            return combustible.Id;
        }

        // Obtener todos los Combustibles
        public async Task<List<Combustible>> ObtenerCombustiblesAsync()
        {
            return await _context.Combustibles.ToListAsync();
        }

        // Obtener un Combustible por ID
        public async Task<Combustible> ObtenerCombustiblePorIdAsync(int id)
        {
            return await _context.Combustibles
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Actualizar un Combustible
        public async Task<bool> ActualizarCombustibleAsync(Combustible combustible)
        {
            var existingCombustible = await _context.Combustibles
                .FirstOrDefaultAsync(c => c.Id == combustible.Id);

            if (existingCombustible == null)
            {
                return false;
            }

            // Actualizamos los campos del combustible
            existingCombustible.FechaSolicitud = combustible.FechaSolicitud;
            existingCombustible.AreaSolicitante = combustible.AreaSolicitante;
            existingCombustible.UsuarioSolicitante = combustible.UsuarioSolicitante;
            existingCombustible.CatalogoId = combustible.CatalogoId;
            existingCombustible.DescripcionServicio = combustible.DescripcionServicio;
            existingCombustible.Fecha = combustible.Fecha;
            existingCombustible.AreaId = combustible.AreaId;
            existingCombustible.Estado = combustible.Estado;
            existingCombustible.Observaciones = combustible.Observaciones;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar un Combustible
        public async Task<bool> EliminarCombustibleAsync(int id)
        {
            var combustible = await _context.Combustibles
                .FirstOrDefaultAsync(c => c.Id == id);

            if (combustible == null)
            {
                return false;
            }

            _context.Combustibles.Remove(combustible);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
