using AccesoDatos.Models;
using ClasesBase.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class SolicitudDao
    {
        Conade1Context context = new Conade1Context();

        public Respuesta Guardar(Solicitud solicitud)
        {
            Respuesta rs = new Respuesta();

            try
            {
                solicitud.FechaSolicitud = DateTime.Now;
                solicitud.Estado = 1; // Estado inicial
                context.Solicituds.Add(solicitud);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Solicitud registrada correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al guardar la solicitud: " + ex.Message;
            }

            return rs;
        }

        public List<Solicitud> ObtenerTodas()
        {
            return context.Solicituds.ToList();
        }

        public Respuesta Actualizar(int id, Solicitud solicitudActualizada)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var solicitud = context.Solicituds.Find(id);
                if (solicitud == null)
                {
                    rs.success = false;
                    rs.mensaje = "Solicitud no encontrada.";
                    return rs;
                }

                solicitud.Estado = solicitudActualizada.Estado;
                solicitud.Observaciones = solicitudActualizada.Observaciones;

                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Solicitud actualizada correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar la solicitud: " + ex.Message;
            }

            return rs;
        }

        public Respuesta Eliminar(int id)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var solicitud = context.Solicituds.Find(id);
                if (solicitud == null)
                {
                    rs.success = false;
                    rs.mensaje = "Solicitud no encontrada.";
                    return rs;
                }

                context.Solicituds.Remove(solicitud);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Solicitud eliminada correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar la solicitud: " + ex.Message;
            }

            return rs;
        }

    }
}
