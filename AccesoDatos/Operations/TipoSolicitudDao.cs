using AccesoDatos.Models;
using ClasesBase.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class TipoSolicitudDao
    {
        Conade1Context context = new Conade1Context();

        public Respuesta Guardar(string nombreTipoSolicitud)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var tipoExistente = context.TipoSolicituds
                    .FirstOrDefault(t => t.Nombre.Equals(nombreTipoSolicitud, StringComparison.OrdinalIgnoreCase));

                if (tipoExistente != null)
                {
                    rs.success = false;
                    rs.mensaje = "Ya existe un tipo de solicitud con ese nombre.";
                    return rs;
                }

                TipoSolicitud nuevoTipo = new TipoSolicitud { Nombre = nombreTipoSolicitud };
                context.TipoSolicituds.Add(nuevoTipo);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Tipo de solicitud creado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al guardar el tipo de solicitud: " + ex.Message;
            }

            return rs;
        }

        public List<TipoSolicitud> ObtenerTodos()
        {
            return context.TipoSolicituds.ToList();
        }

        public Respuesta Actualizar(int id, string nuevoNombre)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var tipo = context.TipoSolicituds.Find(id);
                if (tipo == null)
                {
                    rs.success = false;
                    rs.mensaje = "Tipo de solicitud no encontrado.";
                    return rs;
                }

                tipo.Nombre = nuevoNombre;
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Tipo de solicitud actualizado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar el tipo de solicitud: " + ex.Message;
            }

            return rs;
        }

        public Respuesta Eliminar(int id)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var tipo = context.TipoSolicituds.Find(id);
                if (tipo == null)
                {
                    rs.success = false;
                    rs.mensaje = "Tipo de solicitud no encontrado.";
                    return rs;
                }

                context.TipoSolicituds.Remove(tipo);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Tipo de solicitud eliminado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar el tipo de solicitud: " + ex.Message;
            }

            return rs;
        }

    }
}
