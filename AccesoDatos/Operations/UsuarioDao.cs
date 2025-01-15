using AccesoDatos.Models.Conade1;
using AccesoDatos.Models.Nominas;
using AccesoDatos.Service;
using ClasesBase.Respuestas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace AccesoDatos.Operations
{
    public class UsuarioDao
    {
        private readonly Conade1Context _conadeContext;
        private readonly NominaOsimulacionContext _nominaContext;
        private readonly EmailService _emailService;

        // Inyección de dependencias en el constructor
        public UsuarioDao(Conade1Context conadeContext, NominaOsimulacionContext nominaContext, EmailService emailService)
        {
            _conadeContext = conadeContext;
            _nominaContext = nominaContext;
            _emailService = emailService;
        }

        public async Task<int?> CrearUsuarioAsync(
                        string nombre,
                        string apellidoPaterno,
                        string apellidoMaterno,
                        string claveEmpleado,
                        string nombreUsuario,
                        string contrasena,
                        string rol,
                        int[] areasId)
        {
            // Validar que todas las áreas estén en el rango permitido (1 a 5)
            if (areasId == null || !areasId.Any() || areasId.Any(areaId => areaId < 1 || areaId > 5))
            {
                throw new Exception("Las áreas proporcionadas no son válidas. Asegúrate de que todas las áreas estén entre 1 y 5.");
            }

            // Verificar si ya existe un usuario con el nombre de usuario proporcionado
            var usuarioExistente = await _conadeContext.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

            if (usuarioExistente != null)
            {
                throw new Exception("Ya existe un usuario con ese nombre de usuario.");
            }

            // Verificar si el empleado existe en la base de datos Nómina
            var empleado = await _nominaContext.Empleados
                .FirstOrDefaultAsync(e => e.ClaveEmpleado == claveEmpleado);

            if (empleado == null)
            {
                throw new Exception("La clave de empleado no coincide con ningún registro en la base de datos de Nómina. Verifica la clave e intenta nuevamente.");
            }

            // Verificar que el nombre y los apellidos coincidan con los datos del empleado
            if (empleado.Nombre != nombre || empleado.ApellidoPaterno != apellidoPaterno || empleado.ApellidoMaterno != apellidoMaterno)
            {
                throw new Exception("Los datos de nombre y apellido no coinciden con los registrados para esta clave de empleado.");
            }

            // Verificar si ya existe un usuario asociado a este IdEmpleado
            var usuarioConIdEmpleadoExistente = await _conadeContext.Usuarios
                .FirstOrDefaultAsync(u => u.IdEmpleado == empleado.IdEmpleado);

            if (usuarioConIdEmpleadoExistente != null)
            {
                throw new Exception("Ya existe un usuario asociado a esta clave de empleado.");
            }

            // Crear el nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Nombre = nombre,
                ApellidoPaterno = apellidoPaterno,
                ApellidoMaterno = apellidoMaterno,
                NombreUsuario = nombreUsuario,
                Contrasena = contrasena, // Guardar la contraseña como hash
                Rol = rol,
                IdEmpleado = empleado.IdEmpleado
            };

            // Agregar el nuevo usuario a la base de datos
            _conadeContext.Usuarios.Add(nuevoUsuario);
            await _conadeContext.SaveChangesAsync();

            // Asignar las áreas válidas
            var usuarioAreas = areasId.Select(areaId => new UsuarioArea
            {
                UsuarioId = nuevoUsuario.Id,
                AreaId = areaId,
                FechaAsignacion = DateTime.Now
            });

            await _conadeContext.UsuarioAreas.AddRangeAsync(usuarioAreas);
            await _conadeContext.SaveChangesAsync();

            return nuevoUsuario.Id; // Retorna el ID del nuevo usuario
        }


        public async Task<Usuario?> LoginAsync(string nombreUsuario, string password)
        {

            // Buscar al usuario por nombre de usuario
            var usuario = await _conadeContext.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

            if (usuario == null)
            {
                throw new Exception ("Usuario no existe.");
            }

            // Validar contraseña (esto debe estar alineado con tu lógica de encriptación, si existe)
            if (usuario.Contrasena != password)
            {
                throw new Exception("Contraseña incorrecta.");
            }

            return usuario; // Usuario autenticado
        }

        public async Task<Empleado> ObtenerEmpleadoDesdeNominaO(string nombre, string apellidoPaterno, string apellidoMaterno, string claveEmpleado)
        {
            var empleado = await _nominaContext.Empleados
                .Where(e => e.Nombre == nombre &&
                            e.ApellidoPaterno == apellidoPaterno &&
                            e.ApellidoMaterno == apellidoMaterno &&
                            e.ClaveEmpleado == claveEmpleado)
                .FirstOrDefaultAsync();

            return empleado;
        }

        public async Task<object> ObtenerUsuarioConEmpleadoAsync(string nombreUsuario)
        {
            // Obtener el usuario de la base de datos Conade1
            var usuario = await _conadeContext.Usuarios
                .Where(u => u.NombreUsuario == nombreUsuario)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Obtener los datos del empleado de la base de datos NominaO usando el IdEmpleado del usuario
            var empleado = await _nominaContext.Empleados
                .Where(e => e.IdEmpleado == usuario.IdEmpleado)
                .FirstOrDefaultAsync();

            if (empleado == null)
            {
                throw new Exception("Empleado no encontrado en la base de datos NominaO.");
            }

            // Devolver ambos objetos combinados
            return new
            {
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.NombreUsuario,  // Se devuelve el nombre de usuario
                    usuario.Rol,
                    usuario.FechaCreacion,
                    usuario.FechaUltimoAcceso
                },
                Empleado = new
                {
                    empleado.IdEmpleado,
                    empleado.ClaveEmpleado,
                    empleado.Nombre,
                    empleado.ApellidoPaterno,
                    empleado.ApellidoMaterno,
                    empleado.Sexo,
                    empleado.EstadoCivil,
                    empleado.FechaNacimiento,
                    empleado.Sd,
                    empleado.Sdcotizacion,
                    empleado.Sdi,
                    empleado.NetoPagar,
                    empleado.IdBancoTrad,
                    empleado.CuentaBancariaTrad,
                    empleado.CuentaInterbancariaTrad,
                    empleado.Curp,
                    empleado.Rfc,
                    empleado.Nss,
                    empleado.Foto,
                    empleado.CorreoElectronico,
                    empleado.CorreoElectronicoInstitucional,
                    empleado.FechaReconocimientoAntiguedad,
                    empleado.FechaIngreso,
                    empleado.FechaAltaSs,
                    empleado.FechaBaja,
                    empleado.MotivoBaja,
                    empleado.Recontratable,
                    empleado.IdTipoContrato,
                    empleado.IdPerfil,
                    empleado.IdEstatus,
                    empleado.IdCaptura,
                    empleado.FechaCaptura,
                    empleado.IdBaja,
                    empleado.FechaBajaSistema,
                    empleado.IdModificacion,
                    empleado.FechaModificacion,
                    empleado.Contrasena,
                    empleado.PassKiosko,
                    empleado.Telefono,
                    empleado.Celular,
                    empleado.Nacionalidad,
                    empleado.IdSindicato,
                    empleado.CodigoSindicato,
                    empleado.AfiliacionSindical,
                    empleado.IdCodigoPostal,
                    empleado.Calle,
                    empleado.NumeroExt,
                    empleado.NumeroInt,
                    empleado.Sni,
                    empleado.FechaSni,
                    empleado.IdCodigoPostalDf,
                    empleado.CalleDf,
                    empleado.NoExtDf,
                    empleado.NoIntDf,
                    empleado.ColoniaDf,
                    empleado.AlcaldiaDf,
                    empleado.EntidadDf,
                    empleado.Cpdf,
                    empleado.RutaCsf,
                    empleado.EfectoDesde,
                    empleado.EfectoHasta,
                    empleado.FechaCalculoRetroactivo,
                    empleado.AnionEfectivos,
                    empleado.QuincenaDeAniversario,
                    empleado.IdEmpleadoAnterior,
                    empleado.BanderaReactivado,
                    empleado.RetroactivoPagado,
                    empleado.IdGradoAcademico,
                    empleado.Observaciones,
                    empleado.IdTipoMovimiento,
                    empleado.IdLugarNacimiento,
                    empleado.Horario,
                    empleado.NoTarjeta,
                    empleado.IdTipoPago,
                    empleado.TipoSangre,
                    empleado.TipoVivienda,
                    empleado.NombreContacto,
                    empleado.TelefonoContacto,
                    empleado.Discapacidad,
                    empleado.Etnia,
                    empleado.Compensacion,
                    empleado.IdRecaudacion,
                    empleado.IdTipoRetencion,
                    empleado.IdRhnet,
                    empleado.IdShcp,
                    empleado.IdRusp
                }
            };
        }

        public async Task<List<Empleado>> ListarEmpleadosAsync(string? nombre = null, string? claveEmpleado = null)
        {
            // Inicia la consulta base
            var query = _nominaContext.Empleados.AsQueryable();

            // Aplica filtros opcionales si se proporcionan
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(claveEmpleado))
            {
                query = query.Where(e => e.ClaveEmpleado == claveEmpleado);
            }

            // Ejecuta la consulta y devuelve los resultados como una lista
            var empleados = await query.ToListAsync();

            return empleados;
        }


        public Respuesta ObtenerUsuarios()
        {
            var respuesta = new Respuesta();
            try
            {
                var usuarios = _conadeContext.Usuarios.ToList();
                respuesta.success = true;
                respuesta.mensaje = "Usuarios obtenidos correctamente.";
                respuesta.obj = usuarios;
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = "Error al obtener los usuarios: " + ex.Message;
            }

            return respuesta;
        }

        public async Task<List<UsuarioArea>> ListarUsuarioAreasAsync()
        {
            return await _conadeContext.UsuarioAreas
                .Include(ua => ua.Usuario) // Incluye información del usuario
                .Include(ua => ua.Area)    // Incluye información del área
                .ToListAsync();
        }

        public async Task<List<Area>> ListarAreasPorUsuarioAsync(int usuarioId)
        {
            return await _conadeContext.UsuarioAreas
                .Where(ua => ua.UsuarioId == usuarioId)
                .Include(ua => ua.Area) // Incluye la información del área
                .Select(ua => ua.Area) // Selecciona solo el área
                .ToListAsync();
        }

        public Respuesta EliminarUsuarioPorNombreUsuario(string nombreUsuario)
        {
            var respuesta = new Respuesta();
            var usuario = _conadeContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

            if (usuario == null)
            {
                respuesta.success = false;
                respuesta.mensaje = "El usuario que buscas no existe.";
                return respuesta;
            }

            _conadeContext.Usuarios.Remove(usuario);
            _conadeContext.SaveChanges();

            respuesta.success = true;
            respuesta.mensaje = "Usuario eliminado con éxito.";
            return respuesta;
        }

        public async Task<Respuesta> SolicitudesPorUsuarioAsync(int usuarioId)
        {
            var respuesta = new Respuesta();

            try
            {
                // Consultar solicitudes de diferentes tipos de tabla
                var solicitudesServicioPostal = await _conadeContext.ServicioPostals
                    .Where(s => s.UsuarioSolicitante == usuarioId)
                    .ToListAsync();

                var solicitudesServicioTransporte = await _conadeContext.ServicioTransportes
                    .Where(s => s.UsuarioSolicitante == usuarioId)
                    .ToListAsync();

                var solicitudesMantenimiento = await _conadeContext.Mantenimientos
                    .Where(s => s.UsuarioSolicitante == usuarioId)
                    .ToListAsync();

                var solicitudesEventos = await _conadeContext.Eventos
                    .Where(s => s.UsuarioSolicitante == usuarioId)
                    .ToListAsync();

                // Consultar solicitudes de Combustible
                var solicitudesCombustible = await _conadeContext.Combustibles
                    .Where(s => s.UsuarioSolicitante == usuarioId)
                    .ToListAsync();

                // Combinar todas las solicitudes en una lista
                var todasLasSolicitudes = solicitudesServicioPostal
                    .Cast<object>()
                    .Concat(solicitudesServicioTransporte.Cast<object>())
                    .Concat(solicitudesMantenimiento.Cast<object>())
                    .Concat(solicitudesEventos.Cast<object>())
                    .Concat(solicitudesCombustible.Cast<object>()) // Agregar solicitudes de combustible
                    .ToList();

                if (!todasLasSolicitudes.Any())
                {
                    respuesta.success = false;
                    respuesta.mensaje = "No se encontraron solicitudes para este usuario.";
                    return respuesta;
                }

                respuesta.success = true;
                respuesta.mensaje = "Solicitudes obtenidas correctamente.";
                respuesta.obj = todasLasSolicitudes;
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = "Error al obtener las solicitudes: " + ex.Message;
            }

            return respuesta;
        }


        public async Task<Respuesta> EliminarSolicitudPorIdAsync(int idSolicitud, int usuarioId)
        {
            var respuesta = new Respuesta();

            try
            {
                // Buscar en las tablas relacionadas según el tipo de solicitud
                // Aquí, debes determinar en qué tabla buscar (ServicioPostal, ServicioTransporte, Eventos, Mantenimiento, Combustible)
                // En este ejemplo, supongo que "TipoSolicitud" es un campo que nos indica la tabla en la que buscar.

                // Buscar en la tabla ServicioPostal
                var solicitudServicioPostal = await _conadeContext.ServicioPostals
                    .FirstOrDefaultAsync(s => s.Id == idSolicitud && s.UsuarioSolicitante == usuarioId);

                if (solicitudServicioPostal != null)
                {
                    _conadeContext.ServicioPostals.Remove(solicitudServicioPostal);
                    await _conadeContext.SaveChangesAsync();
                    respuesta.success = true;
                    respuesta.mensaje = "Solicitud eliminada con éxito de ServicioPostal.";
                    return respuesta;
                }

                // Buscar en la tabla ServicioTransporte
                var solicitudServicioTransporte = await _conadeContext.ServicioTransportes
                    .FirstOrDefaultAsync(s => s.Id == idSolicitud && s.UsuarioSolicitante == usuarioId);

                if (solicitudServicioTransporte != null)
                {
                    _conadeContext.ServicioTransportes.Remove(solicitudServicioTransporte);
                    await _conadeContext.SaveChangesAsync();
                    respuesta.success = true;
                    respuesta.mensaje = "Solicitud eliminada con éxito de ServicioTransporte.";
                    return respuesta;
                }

                // Buscar en la tabla Eventos
                var solicitudUsoInmobiliario = await _conadeContext.Eventos
                    .FirstOrDefaultAsync(s => s.Id == idSolicitud && s.UsuarioSolicitante == usuarioId);

                if (solicitudUsoInmobiliario != null)
                {
                    _conadeContext.Eventos.Remove(solicitudUsoInmobiliario);
                    await _conadeContext.SaveChangesAsync();
                    respuesta.success = true;
                    respuesta.mensaje = "Solicitud eliminada con éxito de Eventos.";
                    return respuesta;
                }

                // Buscar en la tabla Mantenimiento
                var solicitudMantenimiento = await _conadeContext.Mantenimientos
                    .FirstOrDefaultAsync(s => s.Id == idSolicitud && s.UsuarioSolicitante == usuarioId);

                if (solicitudMantenimiento != null)
                {
                    _conadeContext.Mantenimientos.Remove(solicitudMantenimiento);
                    await _conadeContext.SaveChangesAsync();
                    respuesta.success = true;
                    respuesta.mensaje = "Solicitud eliminada con éxito de Mantenimiento.";
                    return respuesta;
                }

                // Buscar en la tabla Combustible
                var solicitudCombustible = await _conadeContext.Combustibles
                    .FirstOrDefaultAsync(s => s.Id == idSolicitud && s.UsuarioSolicitante == usuarioId);

                if (solicitudCombustible != null)
                {
                    _conadeContext.Combustibles.Remove(solicitudCombustible);
                    await _conadeContext.SaveChangesAsync();
                    respuesta.success = true;
                    respuesta.mensaje = "Solicitud eliminada con éxito de Combustible.";
                    return respuesta;
                }

                // Si no se encontró la solicitud en ninguna tabla
                respuesta.success = false;
                respuesta.mensaje = "No se encontró la solicitud para el usuario especificado.";
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = "Error al eliminar la solicitud: " + ex.Message;
            }

            return respuesta;
        }

        public async Task<Respuesta> EliminarSolicitudPorIdAdminAsync(int idSolicitud, int usuarioId, string tipoSolicitud)
        {
            var respuesta = new Respuesta();

            try
            {
                // Verificar que el usuario es un admin
                var usuario = await _conadeContext.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == usuarioId);

                if (usuario == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "No se pudo encontrar el usuario.";
                    return respuesta;
                }

                // Validar si el usuario es Admin
                if (usuario.Rol != "Admin")
                {
                    if (!usuario.UsuarioAreas.Any())
                    {
                        respuesta.success = false;
                        respuesta.mensaje = "El usuario no es Administrador.";
                        return respuesta;
                    }
                }

                // Buscar la solicitud según el tipo de solicitud
                switch (tipoSolicitud.ToLower())
                {
                    case "servicio postal":
                        var solicitudServicioPostal = await _conadeContext.ServicioPostals
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);

                        if (solicitudServicioPostal != null)
                        {
                            _conadeContext.ServicioPostals.Remove(solicitudServicioPostal);
                            await _conadeContext.SaveChangesAsync();
                            respuesta.success = true;
                            respuesta.mensaje = "Solicitud eliminada con éxito de ServicioPostal.";
                            return respuesta;
                        }
                        break;

                    case "servicio transporte":
                        var solicitudServicioTransporte = await _conadeContext.ServicioTransportes
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);

                        if (solicitudServicioTransporte != null)
                        {
                            _conadeContext.ServicioTransportes.Remove(solicitudServicioTransporte);
                            await _conadeContext.SaveChangesAsync();
                            respuesta.success = true;
                            respuesta.mensaje = "Solicitud eliminada con éxito de ServicioTransporte.";
                            return respuesta;
                        }
                        break;

                    case "eventos":
                        var solicitudEvento = await _conadeContext.Eventos
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);

                        if (solicitudEvento != null)
                        {
                            _conadeContext.Eventos.Remove(solicitudEvento);
                            await _conadeContext.SaveChangesAsync();
                            respuesta.success = true;
                            respuesta.mensaje = "Solicitud eliminada con éxito de Evento.";
                            return respuesta;
                        }
                        break;

                    case "mantenimiento":
                        var solicitudMantenimiento = await _conadeContext.Mantenimientos
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);

                        if (solicitudMantenimiento != null)
                        {
                            _conadeContext.Mantenimientos.Remove(solicitudMantenimiento);
                            await _conadeContext.SaveChangesAsync();
                            respuesta.success = true;
                            respuesta.mensaje = "Solicitud eliminada con éxito de Mantenimiento.";
                            return respuesta;
                        }
                        break;

                    case "abastecimiento de combustible":
                        var solicitudCombustible = await _conadeContext.Combustibles
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);

                        if (solicitudCombustible != null)
                        {
                            _conadeContext.Combustibles.Remove(solicitudCombustible);
                            await _conadeContext.SaveChangesAsync();
                            respuesta.success = true;
                            respuesta.mensaje = "Solicitud eliminada con éxito de Combustible.";
                            return respuesta;
                        }
                        break;

                    default:
                        respuesta.success = false;
                        respuesta.mensaje = "Tipo de solicitud no válido.";
                        return respuesta;
                }

                // Si no se encontró la solicitud en ninguna tabla
                respuesta.success = false;
                respuesta.mensaje = "No se encontró la solicitud con el ID proporcionado.";
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = "Error al eliminar la solicitud: " + ex.Message;
            }

            return respuesta;
        }





        // Revisar en swagger

        public async Task<Respuesta> AprobarRechazarSolicitudAsync(int idSolicitud, int usuarioId, string accion, string observaciones, string tipoSolicitud)
        {
            var respuesta = new Respuesta();

            try
            {
                // Obtener el usuario
                var usuario = await _conadeContext.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == usuarioId);

                if (usuario == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "No se pudo encontrar el usuario.";
                    return respuesta;
                }

                // Verificar si el usuario es Admin
                if (usuario.Rol != "Admin")
                {
                    respuesta.success = false;
                    respuesta.mensaje = "El usuario no tiene permisos para aprobar o rechazar solicitudes.";
                    return respuesta;
                }

                // Determinar el tipo de solicitud y el correo del solicitante
                object solicitud = null;
                string emailSolicitante = string.Empty;

                // Buscar la solicitud según el tipo de solicitud proporcionado
                switch (tipoSolicitud.ToLower())
                {
                    case "servicio postal":
                        var solicitudPostal = await _conadeContext.ServicioPostals
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);
                        if (solicitudPostal != null)
                        {
                            solicitud = solicitudPostal;
                            // Obtener el correo del solicitante desde _nominaContext
                            var usuarioSolicitante = await _conadeContext.Usuarios
                                .FirstOrDefaultAsync(u => u.Id == solicitudPostal.UsuarioSolicitante);
                            if (usuarioSolicitante != null)
                            {
                                var correoUsuarioSolicitante = await ObtenerCorreoConEmpleadoAsync(usuarioSolicitante.NombreUsuario);
                                emailSolicitante = correoUsuarioSolicitante.Empleado.CorreoElectronico;
                            }
                        }
                        break;

                    case "servicio transporte":
                        var solicitudTransporte = await _conadeContext.ServicioTransportes
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);
                        if (solicitudTransporte != null)
                        {
                            solicitud = solicitudTransporte;
                            // Obtener el correo del solicitante
                            var usuarioSolicitante = await _conadeContext.Usuarios
                                .FirstOrDefaultAsync(u => u.Id == solicitudTransporte.UsuarioSolicitante);
                            if (usuarioSolicitante != null)
                            {
                                var correoUsuarioSolicitante = await ObtenerCorreoConEmpleadoAsync(usuarioSolicitante.NombreUsuario);
                                emailSolicitante = correoUsuarioSolicitante.Empleado.CorreoElectronico;
                            }
                        }
                        break;

                    case "eventos":
                        var solicitudEventos = await _conadeContext.Eventos
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);
                        if (solicitudEventos != null)
                        {
                            solicitud = solicitudEventos;
                            // Obtener el correo del solicitante
                            var usuarioSolicitante = await _conadeContext.Usuarios
                                .FirstOrDefaultAsync(u => u.Id == solicitudEventos.UsuarioSolicitante);
                            if (usuarioSolicitante != null)
                            {
                                var correoUsuarioSolicitante = await ObtenerCorreoConEmpleadoAsync(usuarioSolicitante.NombreUsuario);
                                emailSolicitante = correoUsuarioSolicitante.Empleado.CorreoElectronico;
                            }
                        }
                        break;

                    case "mantenimiento":
                        var solicitudMantenimiento = await _conadeContext.Mantenimientos
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);
                        if (solicitudMantenimiento != null)
                        {
                            solicitud = solicitudMantenimiento;
                            // Obtener el correo del solicitante
                            var usuarioSolicitante = await _conadeContext.Usuarios
                                .FirstOrDefaultAsync(u => u.Id == solicitudMantenimiento.UsuarioSolicitante);
                            if (usuarioSolicitante != null)
                            {
                                var correoUsuarioSolicitante = await ObtenerCorreoConEmpleadoAsync(usuarioSolicitante.NombreUsuario);
                                emailSolicitante = correoUsuarioSolicitante.Empleado.CorreoElectronico;
                            }
                        }
                        break;

                    case "abastecimiento de combustible":
                        var solicitudCombustible = await _conadeContext.Combustibles
                            .FirstOrDefaultAsync(s => s.Id == idSolicitud);
                        if (solicitudCombustible != null)
                        {
                            solicitud = solicitudCombustible;
                            // Obtener el correo del solicitante
                            var usuarioSolicitante = await _conadeContext.Usuarios
                                .FirstOrDefaultAsync(u => u.Id == solicitudCombustible.UsuarioSolicitante);
                            if (usuarioSolicitante != null)
                            {
                                var correoUsuarioSolicitante = await ObtenerCorreoConEmpleadoAsync(usuarioSolicitante.NombreUsuario);
                                emailSolicitante = correoUsuarioSolicitante.Empleado.CorreoElectronico;
                            }
                        }
                        break;

                    default:
                        respuesta.success = false;
                        respuesta.mensaje = "Tipo de solicitud no válido.";
                        return respuesta;
                }

                if (solicitud == null)
                {
                    respuesta.success = false;
                    respuesta.mensaje = "No se encontró la solicitud en el área especificada.";
                    return respuesta;
                }

                // Verificar el estado actual de la solicitud
                string estadoActual = solicitud switch
                {
                    ServicioPostal sp => sp.Estado,
                    ServicioTransporte st => st.Estado,
                    Evento ev => ev.Estado,
                    Mantenimiento m => m.Estado,
                    Combustible c => c.Estado,
                    _ => null
                };

                if (estadoActual == "Atendida" || estadoActual == "Rechazada")
                {
                    respuesta.success = false;
                    respuesta.mensaje = $"La solicitud ya ha sido {estadoActual.ToLower()}.";
                    return respuesta;
                }

                // Cambiar el estado de la solicitud según la acción
                if (accion == "Atender")
                {
                    if (solicitud is ServicioPostal sp)
                    {
                        sp.Estado = "Atendida";
                        sp.Observaciones = observaciones;
                    }
                    else if (solicitud is ServicioTransporte st)
                    {
                        st.Estado = "Atendida";
                        st.Observaciones = observaciones;
                    }
                    else if (solicitud is Evento ev)
                    {
                        ev.Estado = "Atendida";
                        ev.Observaciones = observaciones;
                    }
                    else if (solicitud is Mantenimiento m)
                    {
                        m.Estado = "Atendida";
                        m.Observaciones = observaciones;
                    }
                    else if (solicitud is Combustible c)
                    {
                        c.Estado = "Atendida";
                        c.Observaciones = observaciones;
                    }
                }
                else if (accion == "Rechazar")
                {
                    if (solicitud is ServicioPostal sp)
                    {
                        sp.Estado = "Rechazada";
                        sp.Observaciones = observaciones;
                    }
                    else if (solicitud is ServicioTransporte st)
                    {
                        st.Estado = "Rechazada";
                        st.Observaciones = observaciones;
                    }
                    else if (solicitud is Evento ev)
                    {
                        ev.Estado = "Rechazada";
                        ev.Observaciones = observaciones;
                    }
                    else if (solicitud is Mantenimiento m)
                    {
                        m.Estado = "Rechazada";
                        m.Observaciones = observaciones;
                    }
                    else if (solicitud is Combustible c)
                    {
                        c.Estado = "Rechazada";
                        c.Observaciones = observaciones;
                    }
                }
                else
                {
                    respuesta.success = false;
                    respuesta.mensaje = "Acción no válida. Use 'Atender' o 'Rechazar'.";
                    return respuesta;
                }

                // Guardar los cambios
                await _conadeContext.SaveChangesAsync();

                respuesta.success = true;

                // Verificar si la respuesta fue exitosa
                if (respuesta.success)
                {
                    // Primero, asignar el mensaje de éxito o rechazo según la acción
                    respuesta.mensaje = accion == "Atender" ? "Solicitud atendida con éxito." : "Solicitud rechazada con éxito.";

                    // Verificar si el correo es nulo o vacío
                    if (string.IsNullOrEmpty(emailSolicitante))
                    {
                        // Ajustar el mensaje si el usuario no tiene correo electrónico
                        respuesta.mensaje = $"La solicitud ha sido {(accion == "Atender" ? "atendida" : "rechazada")}, pero el usuario no tiene correo electrónico registrado.";
                    }
                    else
                    {
                        // Enviar correo al solicitante
                        string subject = accion == "Atender" ? "Solicitud atendida" : "Solicitud rechazada";
                        string body = $"Hola, su solicitud con ID {idSolicitud} ha sido {(accion == "Atender" ? "atendida" : "rechazada")}.\n\nObservaciones: {observaciones}";

                        try
                        {
                            await _emailService.EnviarCorreoAsync(emailSolicitante, subject, body);
                        }
                        catch (Exception ex)
                        {
                            respuesta.success = false;
                            respuesta.mensaje += "\nError al enviar correo: " + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.mensaje = "Error al aprobar o rechazar la solicitud: " + ex.Message;
            }

            return respuesta;
        }


        // Método para obtener el usuario y empleado asociado
        public async Task<dynamic> ObtenerCorreoConEmpleadoAsync(string nombreUsuario)
        {
            var usuario = await _conadeContext.Usuarios
                .Where(u => u.NombreUsuario == nombreUsuario)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            var empleado = await _nominaContext.Empleados
                .Where(e => e.IdEmpleado == usuario.IdEmpleado)
                .FirstOrDefaultAsync();

            if (empleado == null)
            {
                throw new Exception("Empleado no encontrado en la base de datos NominaO.");
            }

            return new
            {
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.NombreUsuario,
                    usuario.Rol,
                    usuario.FechaCreacion,
                    usuario.FechaUltimoAcceso
                },
                Empleado = new
                {
                    empleado.IdEmpleado,
                    empleado.CorreoElectronico
                }
            };
        }


    }
}
