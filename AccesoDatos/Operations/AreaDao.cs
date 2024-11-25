using AccesoDatos.Models;
using ClasesBase.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class AreaDao
    {
        Conade1Context context = new Conade1Context();

        public Respuesta mostrar()
        {
            Respuesta rs = new Respuesta();

            if (context.Areas.Count() > 0)
            {
                rs.success = true;
                rs.mensaje = "Registros encontrados";
                rs.obj = context.Areas.ToList();
            }
            else
            {
                rs.success = false;
                rs.mensaje = "No hay registros";
            }

            return rs;

        }

        public Respuesta guardar(string nombreArea)
        {
            Respuesta rs = new Respuesta();

            try
            {
                // Verificar si ya existe un área con el mismo nombre
                var areaExistente = context.Areas
                    .FirstOrDefault(a => a.Nombre.Equals(nombreArea));

                if (areaExistente != null)
                {
                    // Si existe, retornar un mensaje de error
                    rs.success = false;
                    rs.mensaje = "Ya existe un área con ese nombre.";
                    return rs;
                }

                // Si no existe, crear la nueva área
                Area nuevaArea = new Area
                {
                    Nombre = nombreArea
                };

                // Agregar la nueva área al contexto y guardar los cambios
                context.Areas.Add(nuevaArea);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Área guardada correctamente";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al guardar el área: " + ex.Message;
            }

            return rs;
        }

        public Respuesta buscar(string nombre)
        {
            Respuesta rs = new Respuesta();
            var areas = context.Areas.Where(a => a.Nombre.Contains(nombre)).ToList();

            if (areas.Any())
            {
                rs.success = true;
                rs.mensaje = "Áreas encontradas";
                rs.obj = areas;
            }
            else
            {
                rs.success = false;
                rs.mensaje = "No se encontraron áreas con ese nombre";
            }

            return rs;
        }

        public Respuesta actualizarArea(Area areaActualizada)
        {
            Respuesta rs = new Respuesta();
            try
            {
                var area = context.Areas.FirstOrDefault(a => a.Id == areaActualizada.Id);
                if (area != null)
                {
                    area.Nombre = areaActualizada.Nombre;
                    context.SaveChanges();
                    rs.success = true;
                    rs.mensaje = "Área actualizada correctamente";
                }
                else
                {
                    rs.success = false;
                    rs.mensaje = "Área no encontrada";
                }
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar el área: " + ex.Message;
            }

            return rs;
        }

        public Respuesta eliminarArea(int areaId)
        {
            Respuesta rs = new Respuesta();
            try
            {
                var area = context.Areas.FirstOrDefault(a => a.Id == areaId);
                if (area != null)
                {
                    context.Areas.Remove(area);
                    context.SaveChanges();
                    rs.success = true;
                    rs.mensaje = "Área eliminada correctamente";
                }
                else
                {
                    rs.success = false;
                    rs.mensaje = "Área no encontrada";
                }
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar el área: " + ex.Message;
            }

            return rs;
        }

    }
}
