using AccesoDatos.Models;
using ClasesBase.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class MantenimientoDao
    {
        Conade1Context context = new Conade1Context();

        public Respuesta Guardar(Mantenimiento mantenimiento)
        {
            Respuesta rs = new Respuesta();

            try
            {
                context.Mantenimientos.Add(mantenimiento);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Mantenimiento registrado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al guardar el mantenimiento: " + ex.Message;
            }

            return rs;
        }

        public List<Mantenimiento> ObtenerTodos()
        {
            return context.Mantenimientos.ToList();
        }

        public Respuesta Actualizar(int id, Mantenimiento mantenimientoActualizado)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var mantenimiento = context.Mantenimientos.Find(id);
                if (mantenimiento == null)
                {
                    rs.success = false;
                    rs.mensaje = "Mantenimiento no encontrado.";
                    return rs;
                }

                mantenimiento.TipoMantenimiento = mantenimientoActualizado.TipoMantenimiento;
                mantenimiento.DescripcionServicio = mantenimientoActualizado.DescripcionServicio;
                mantenimiento.Estado = mantenimientoActualizado.Estado;

                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Mantenimiento actualizado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar el mantenimiento: " + ex.Message;
            }

            return rs;
        }

        public Respuesta Eliminar(int id)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var mantenimiento = context.Mantenimientos.Find(id);
                if (mantenimiento == null)
                {
                    rs.success = false;
                    rs.mensaje = "Mantenimiento no encontrado.";
                    return rs;
                }

                context.Mantenimientos.Remove(mantenimiento);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Mantenimiento eliminado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar el mantenimiento: " + ex.Message;
            }

            return rs;
        }
    }
}
