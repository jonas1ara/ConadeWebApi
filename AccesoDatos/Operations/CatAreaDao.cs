using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class CatAreaDao
    {
        private readonly Conade1Context _context;

        // Constructor
        public CatAreaDao(Conade1Context context)
        {
            _context = context;
        }

        // Crear un nuevo CatArea
        public async Task<int> CrearCatAreaAsync(int? areaId, int? idCliente, string? clave, string? area, decimal? fuenteFinanciamiento)
        {
            var catAreaExistente = await _context.CatAreas
                .FirstOrDefaultAsync(ca => ca.Clave == clave);

            if (catAreaExistente != null)
            {
                throw new Exception("Ya existe un CatArea con esta clave.");
            }

            var catArea = new CatArea
            {
                AreaId = areaId,
                IdCliente = idCliente,
                Clave = clave,
                NombreArea = area,
                FuenteFinanciamiento = fuenteFinanciamiento,
                FechaCaptura = DateTime.UtcNow
            };

            _context.CatAreas.Add(catArea);
            await _context.SaveChangesAsync();

            return catArea.IdArea;
        }

        // Obtener todas las CatAreas
        public async Task<IQueryable<CatArea>> ObtenerCatAreasAsync()
        {
            return _context.CatAreas.Include(ca => ca.AreaNavigation);
        }

        // Obtener un CatArea por ID
        public async Task<CatArea> ObtenerCatAreaPorIdAsync(int id)
        {
            var catArea = await _context.CatAreas
                .Include(ca => ca.AreaNavigation)
                .FirstOrDefaultAsync(ca => ca.IdArea == id);

            if (catArea == null)
            {
                throw new Exception("CatArea no encontrada.");
            }

            return catArea;
        }

        // Actualizar CatArea
        public async Task ActualizarCatAreaAsync(int id, int? areaId, int? idCliente, string? clave, string? area, decimal? fuenteFinanciamiento)
        {
            var catArea = await _context.CatAreas
                .FirstOrDefaultAsync(ca => ca.IdArea == id);

            if (catArea == null)
            {
                throw new Exception("CatArea no encontrada.");
            }

            catArea.AreaId = areaId;
            catArea.IdCliente = idCliente;
            catArea.Clave = clave;
            catArea.NombreArea = area;
            catArea.FuenteFinanciamiento = fuenteFinanciamiento;
            catArea.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // Eliminar un CatArea
        public async Task EliminarCatAreaAsync(int id)
        {
            var catArea = await _context.CatAreas
                .FirstOrDefaultAsync(ca => ca.IdArea == id);

            if (catArea == null)
            {
                throw new Exception("CatArea no encontrada.");
            }

            _context.CatAreas.Remove(catArea);
            await _context.SaveChangesAsync();
        }
    }
}
