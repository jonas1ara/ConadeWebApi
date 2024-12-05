using AccesoDatos.Models.Conade1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class AreaDao
    {
        private readonly Conade1Context _context;

        // Constructor
        public AreaDao(Conade1Context context)
        {
            _context = context;
        }

        // Crear un nuevo área
        public async Task<int> CrearAreaAsync(string nombre)
        {
            var areaExistente = await _context.Areas
                .FirstOrDefaultAsync(a => a.Nombre == nombre);

            if (areaExistente != null)
            {
                throw new Exception("Ya existe un área con este nombre.");
            }

            var area = new Area
            {
                Nombre = nombre
            };

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            return area.Id;
        }

        // Obtener todas las áreas
        public async Task<IQueryable<Area>> ObtenerAreasAsync()
        {
            return _context.Areas;
        }

        // Obtener un área por ID
        public async Task<Area> ObtenerAreaPorIdAsync(int id)
        {
            var area = await _context.Areas
                .FirstOrDefaultAsync(a => a.Id == id);

            if (area == null)
            {
                throw new Exception("Área no encontrada.");
            }

            return area;
        }

        // Actualizar área
        public async Task ActualizarAreaAsync(int id, string nombre)
        {
            var area = await _context.Areas
                .FirstOrDefaultAsync(a => a.Id == id);

            if (area == null)
            {
                throw new Exception("Área no encontrada.");
            }

            area.Nombre = nombre;

            await _context.SaveChangesAsync();
        }

        // Eliminar un área
        public async Task EliminarAreaAsync(int id)
        {
            var area = await _context.Areas
                .FirstOrDefaultAsync(a => a.Id == id);

            if (area == null)
            {
                throw new Exception("Área no encontrada.");
            }

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
        }
    }
}
