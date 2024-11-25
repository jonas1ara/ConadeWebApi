using AccesoDatos.Models;
using ClasesBase.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class EstadoSolicitudDao
    {
        Conade1Context context = new Conade1Context();

        public Respuesta Guardar(string nombreEstado)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var estadoExistente = context.EstadoSolicituds
                    .FirstOrDefault(e => e.Nombre.Equals(nombreEstado, StringComparison.OrdinalIgnoreCase));

                if (estadoExistente != null)
                {
                    rs.success = false;
                    rs.mensaje = "Ya existe un estado con ese nombre.";
                    return rs;
                }

                EstadoSolicitud nuevoEstado = new EstadoSolicitud { Nombre = nombreEstado };
                context.EstadoSolicituds.Add(nuevoEstado);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Estado creado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al guardar el estado: " + ex.Message;
            }

            return rs;
        }

        public List<EstadoSolicitud> ObtenerTodos()
        {
            return context.EstadoSolicituds.ToList();
        }

        public Respuesta Actualizar(int id, string nuevoNombre)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var estado = context.EstadoSolicituds.Find(id);
                if (estado == null)
                {
                    rs.success = false;
                    rs.mensaje = "Estado no encontrado.";
                    return rs;
                }

                estado.Nombre = nuevoNombre;
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Estado actualizado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar el estado: " + ex.Message;
            }

            return rs;
        }

        public Respuesta Eliminar(int id)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var estado = context.EstadoSolicituds.Find(id);
                if (estado == null)
                {
                    rs.success = false;
                    rs.mensaje = "Estado no encontrado.";
                    return rs;
                }

                context.EstadoSolicituds.Remove(estado);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Estado eliminado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar el estado: " + ex.Message;
            }

            return rs;
        }
    }
}
