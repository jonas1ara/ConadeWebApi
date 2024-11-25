using AccesoDatos.Models;
using ClasesBase.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class CatalogoInstalacionesDao
    {
        Conade1Context context = new Conade1Context();

        public Respuesta Guardar(string nombreInstalacion)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var instalacionExistente = context.CatalogoInstalaciones
                    .FirstOrDefault(i => i.Nombre.Equals(nombreInstalacion, StringComparison.OrdinalIgnoreCase));

                if (instalacionExistente != null)
                {
                    rs.success = false;
                    rs.mensaje = "Ya existe una instalación con ese nombre.";
                    return rs;
                }

                CatalogoInstalaciones nuevaInstalacion = new CatalogoInstalaciones { Nombre = nombreInstalacion };
                context.CatalogoInstalaciones.Add(nuevaInstalacion);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Instalación creada correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al guardar la instalación: " + ex.Message;
            }
            return rs;
        }

        public List<CatalogoInstalaciones> ObtenerTodas()
        {
            return context.CatalogoInstalaciones.ToList();
        }

        public Respuesta Actualizar(int id, string nuevoNombre)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var instalacion = context.CatalogoInstalaciones.Find(id);
                if (instalacion == null)
                {
                    rs.success = false;
                    rs.mensaje = "Instalación no encontrada.";
                    return rs;
                }

                instalacion.Nombre = nuevoNombre;
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Instalación actualizada correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar la instalación: " + ex.Message;
            }

            return rs;
        }

        public Respuesta Eliminar(int id)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var instalacion = context.CatalogoInstalaciones.Find(id);
                if (instalacion == null)
                {
                    rs.success = false;
                    rs.mensaje = "Instalación no encontrada.";
                    return rs;
                }

                context.CatalogoInstalaciones.Remove(instalacion);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Instalación eliminada correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar la instalación: " + ex.Message;
            }

            return rs;
        }
    }
}
