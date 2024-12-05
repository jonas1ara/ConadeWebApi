using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class SolicitudDao
    {
        private readonly Conade1Context _context;

        // Constructor
        public SolicitudDao(Conade1Context context)
        {
            _context = context;
        }

        // Obtener todas las solicitudes según el área que administra el Admin
        public async Task<List<object>> ObtenerSolicitudesPorAreaAsync(int areaId)
        {
            try
            {
                var solicitudes = new List<object>();

                // Lista de consultas dependiendo del área
                var areas = new List<IQueryable<object>>
                {
                    _context.ServicioPostals.Where(s => s.AreaSolicitante == areaId)
                        .Include(s => s.AreaSolicitanteNavigation)
                        .Include(s => s.UsuarioSolicitanteNavigation)
                        .Include(s => s.Catalogo),
                    _context.ServicioTransportes.Where(s => s.AreaSolicitante == areaId)
                        .Include(s => s.AreaSolicitanteNavigation)
                        .Include(s => s.UsuarioSolicitanteNavigation)
                        .Include(s => s.Catalogo),
                    _context.UsoInmobiliarios.Where(s => s.AreaSolicitante == areaId)
                        .Include(s => s.AreaSolicitanteNavigation)
                        .Include(s => s.UsuarioSolicitanteNavigation)
                        .Include(s => s.Catalogo),
                    _context.Mantenimientos.Where(s => s.AreaSolicitante == areaId)
                        .Include(s => s.AreaSolicitanteNavigation)
                        .Include(s => s.UsuarioSolicitanteNavigation)
                        .Include(s => s.Catalogo)
                };

                // Recorrer todas las consultas de áreas y añadir los resultados
                foreach (var areaQuery in areas)
                {
                    var solicitudesArea = await areaQuery.ToListAsync();
                    solicitudes.AddRange(solicitudesArea);
                }

                return solicitudes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las solicitudes", ex);
            }
        }
    }
}
