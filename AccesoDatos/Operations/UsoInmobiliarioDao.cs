using AccesoDatos.Models;
using ClasesBase.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class UsoInmobiliarioDao
    {

        Conade1Context context = new Conade1Context();

        public List<UsoInmobiliario> Listar()
        {
            return context.UsoInmobiliarios.ToList();
        }

        public Respuesta Guardar(int solicitudId, string sala, int catalogoId,
                                        DateOnly fechaInicio, DateOnly fechaFin,
                                        TimeSpan horarioInicio, TimeSpan horarioFin,
                                        string? observaciones)
        {
            Respuesta rs = new Respuesta();

            try
            {
                // Validar que la solicitud existe
                var solicitudExistente = context.Solicituds.FirstOrDefault(s => s.Id == solicitudId);
                if (solicitudExistente == null)
                {
                    rs.success = false;
                    rs.mensaje = "El ID de solicitud proporcionado no existe.";
                    return rs;
                }

                // Validaciones de fechas y horarios
                if (fechaFin < fechaInicio)
                {
                    rs.success = false;
                    rs.mensaje = "La fecha de fin no puede ser anterior a la fecha de inicio.";
                    return rs;
                }

                if (fechaInicio == fechaFin && horarioFin < horarioInicio)
                {
                    rs.success = false;
                    rs.mensaje = "La hora de fin debe ser posterior a la hora de inicio.";
                    return rs;
                }

                // Crear la entidad
                UsoInmobiliario nuevoUso = new UsoInmobiliario
                {
                    SolicitudId = solicitudId,
                    Sala = sala,
                    CatalogoId = catalogoId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    HorarioInicio = horarioInicio,
                    HorarioFin = horarioFin,
                    Estado = "Solicitada", // Estado inicial
                    Observaciones = observaciones
                };

                // Guardar en la base de datos
                context.UsoInmobiliarios.Add(nuevoUso);
                context.SaveChanges();

                // Respuesta exitosa
                rs.success = true;
                rs.mensaje = "Uso inmobiliario registrado correctamente.";
            }
            catch (Exception ex)
            {
                // Manejo de errores
                rs.success = false;
                rs.mensaje = "Error al guardar el uso inmobiliario: " + ex.Message;
            }

            return rs;
        }

        public Respuesta Actualizar(int id, UsoInmobiliario usoActualizado)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var uso = context.UsoInmobiliarios.Find(id);
                if (uso == null)
                {
                    rs.success = false;
                    rs.mensaje = "Uso inmobiliario no encontrado.";
                    return rs;
                }

                uso.Sala = usoActualizado.Sala;
                uso.FechaInicio = usoActualizado.FechaInicio;
                uso.FechaFin = usoActualizado.FechaFin;
                uso.HorarioInicio = usoActualizado.HorarioInicio;
                uso.HorarioFin = usoActualizado.HorarioFin;
                uso.Estado = usoActualizado.Estado;
                uso.Observaciones = usoActualizado.Observaciones;

                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Uso inmobiliario actualizado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar el uso inmobiliario: " + ex.Message;
            }

            return rs;
        }

        public Respuesta Eliminar(int id)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var uso = context.UsoInmobiliarios.Find(id);
                if (uso == null)
                {
                    rs.success = false;
                    rs.mensaje = "Uso inmobiliario no encontrado.";
                    return rs;
                }

                context.UsoInmobiliarios.Remove(uso);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Uso inmobiliario eliminado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar el uso inmobiliario: " + ex.Message;
            }

            return rs;
        }

    }
}
