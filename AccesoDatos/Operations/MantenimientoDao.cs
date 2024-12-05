using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class MantenimientoDao
    {
        private readonly Conade1Context _context;

        // Constructor
        public MantenimientoDao(Conade1Context context)
        {
            _context = context;
        }

        // Crear un nuevo Mantenimiento
        // Crear un nuevo Mantenimiento
        public async Task<int?> CrearMantenimientoAsync(
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
        string estado,
        string observaciones)
        {
            // Validar tipo de solicitud (debe ser 'Mantenimiento')
            if (tipoSolicitud != "Mantenimiento")
            {
                throw new ArgumentException("El tipo de solicitud debe ser 'Mantenimiento'.");
            }

            // Validar estado (debe ser uno de los valores permitidos)
            var estadosPermitidos = new[] { "Solicitada", "Atendida", "Rechazada" };
            if (!estadosPermitidos.Contains(estado))
            {
                throw new ArgumentException("El estado debe ser 'Solicitada', 'Atendida' o 'Rechazada'.");
            }

            // Validar tipo de servicio (debe ser 'Preventivo' o 'Correctivo')
            var tiposServicioPermitidos = new[] { "Preventivo", "Correctivo" };
            if (!tiposServicioPermitidos.Contains(tipoServicio))
            {
                throw new ArgumentException("El tipo de servicio debe ser 'Preventivo' o 'Correctivo'.");
            }

            // Crear un nuevo objeto de Mantenimiento con los datos proporcionados
            var mantenimiento = new Mantenimiento
            {
                NumeroDeSerie = numeroDeSerie,
                FechaSolicitud = fechaSolicitud,
                AreaSolicitante = areaSolicitante,
                UsuarioSolicitante = usuarioSolicitante,
                TipoSolicitud = tipoSolicitud,
                TipoServicio = tipoServicio,
                CatalogoId = catalogoId,
                DescripcionServicio = descripcionServicio,
                FechaInicio = fechaInicio,
                FechaEntrega = fechaEntrega,
                AreaId = 4, // Asignar AreaId = 4 directamente
                Estado = estado,
                Observaciones = observaciones
            };

            // Agregar el nuevo Mantenimiento a la base de datos
            _context.Mantenimientos.Add(mantenimiento);
            await _context.SaveChangesAsync();

            // Retornar el ID del nuevo Mantenimiento creado
            return mantenimiento.Id;
        }


        // Obtener todos los Mantenimientos
        public async Task<IQueryable<Mantenimiento>> ObtenerMantenimientosAsync()
        {
            try
            {
                return _context.Mantenimientos.AsQueryable();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al obtener los mantenimientos", ex);
            }
        }

        // Obtener mantenimientos por usuario
        // Obtener las solicitudes de Mantenimiento por Usuario
        public async Task<List<Mantenimiento>> ObtenerMantenimientosPorUsuarioAsync(int usuarioSolicitanteId)
        {
            try
            {
                // Obtener las solicitudes de mantenimiento para el usuario solicitante
                var mantenimientos = await _context.Mantenimientos
                    .Where(m => m.UsuarioSolicitante == usuarioSolicitanteId)
                    .Include(m => m.AreaSolicitanteNavigation) // Incluye el área solicitante
                    .Include(m => m.UsuarioSolicitanteNavigation) // Incluye el usuario solicitante
                    .Include(m => m.Catalogo) // Incluye el catálogo asociado
                    .ToListAsync();

                return mantenimientos;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al obtener las solicitudes de mantenimiento", ex);
            }
        }


        // Obtener un Mantenimiento por ID
        public async Task<Mantenimiento> ObtenerMantenimientoPorIdAsync(int id)
        {
            try
            {
                return await _context.Mantenimientos
                                     .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al obtener el mantenimiento", ex);
            }
        }

        // Actualizar un Mantenimiento
        public async Task<bool> ActualizarMantenimientoAsync(Mantenimiento mantenimiento)
        {
            try
            {
                var mantenimientoExistente = await _context.Mantenimientos
                                                           .FirstOrDefaultAsync(m => m.Id == mantenimiento.Id);

                if (mantenimientoExistente == null)
                    return false;

                // Actualizar las propiedades del mantenimiento
                mantenimientoExistente.NumeroDeSerie = mantenimiento.NumeroDeSerie;
                mantenimientoExistente.FechaSolicitud = mantenimiento.FechaSolicitud;
                mantenimientoExistente.AreaSolicitante = mantenimiento.AreaSolicitante;
                mantenimientoExistente.UsuarioSolicitante = mantenimiento.UsuarioSolicitante;
                mantenimientoExistente.CatalogoId = mantenimiento.CatalogoId;
                //mantenimientoExistente.TipoMantenimiento = mantenimiento.TipoMantenimiento;
                mantenimientoExistente.DescripcionServicio = mantenimiento.DescripcionServicio;
                mantenimientoExistente.FechaInicio = mantenimiento.FechaInicio;
                mantenimientoExistente.FechaEntrega = mantenimiento.FechaEntrega;
                mantenimientoExistente.Estado = mantenimiento.Estado;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al actualizar el mantenimiento", ex);
            }
        }

        // Eliminar un Mantenimiento
        public async Task<bool> EliminarMantenimientoAsync(int id)
        {
            try
            {
                var mantenimiento = await _context.Mantenimientos
                                                  .FirstOrDefaultAsync(m => m.Id == id);

                if (mantenimiento == null)
                    return false;

                _context.Mantenimientos.Remove(mantenimiento);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al eliminar el mantenimiento", ex);
            }
        }
    }
}
