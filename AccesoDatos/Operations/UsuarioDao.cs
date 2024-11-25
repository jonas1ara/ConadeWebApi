using AccesoDatos.Models;
using ClasesBase.Respuestas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operations
{
    public class UsuarioDao
    {
        Conade1Context context = new Conade1Context();

        // Método para guardar un usuario
        public Respuesta guardarUsuario(string nombreUsuario, string correoUsuario)
        {
            Respuesta rs = new Respuesta();

            try
            {
                // Verificar si el correo ya está registrado
                if (context.Usuarios.Any(u => u.Correo == correoUsuario))
                {
                    rs.success = false;
                    rs.mensaje = "El correo ya está registrado.";
                    return rs;
                }

                Usuario nuevoUsuario = new Usuario
                {
                    Nombre = nombreUsuario,
                    Correo = correoUsuario
                };

                context.Usuarios.Add(nuevoUsuario);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Usuario guardado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al guardar el usuario: " + ex.Message;
            }

            return rs;
        }

        // Método para obtener todos los usuarios
        public Respuesta obtenerUsuarios()
        {
            Respuesta rs = new Respuesta();

            try
            {
                var usuarios = context.Usuarios.ToList();
                rs.success = true;
                rs.mensaje = "Usuarios obtenidos correctamente.";
                rs.obj = usuarios;
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al obtener los usuarios: " + ex.Message;
            }

            return rs;
        }

        // Método para actualizar un usuario
        public Respuesta actualizarUsuario(int idUsuario, string nuevoNombre, string nuevoCorreo)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var usuario = context.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

                if (usuario == null)
                {
                    rs.success = false;
                    rs.mensaje = "Usuario no encontrado.";
                    return rs;
                }

                // Verificar si el nuevo correo ya está en uso por otro usuario
                if (context.Usuarios.Any(u => u.Correo == nuevoCorreo && u.Id != idUsuario))
                {
                    rs.success = false;
                    rs.mensaje = "El correo ya está registrado por otro usuario.";
                    return rs;
                }

                usuario.Nombre = nuevoNombre;
                usuario.Correo = nuevoCorreo;

                context.Usuarios.Update(usuario);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Usuario actualizado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al actualizar el usuario: " + ex.Message;
            }

            return rs;
        }

        // Método para eliminar un usuario
        public Respuesta eliminarUsuario(int idUsuario)
        {
            Respuesta rs = new Respuesta();

            try
            {
                var usuario = context.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

                if (usuario == null)
                {
                    rs.success = false;
                    rs.mensaje = "Usuario no encontrado.";
                    return rs;
                }

                context.Usuarios.Remove(usuario);
                context.SaveChanges();

                rs.success = true;
                rs.mensaje = "Usuario eliminado correctamente.";
            }
            catch (Exception ex)
            {
                rs.success = false;
                rs.mensaje = "Error al eliminar el usuario: " + ex.Message;
            }

            return rs;
        }

        // Metodo para registrar un usuario

        public void RegistrarUsuario(string nombre, string correo, string password, string rol)
        {
            using var hmac = new HMACSHA512();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            var usuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo,
                PasswordSalt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(hash),
                Rol = rol
            };

            context.Usuarios.Add(usuario);
            context.SaveChanges();
        }

        // Verificar las credenciales de un usuario

        public bool VerificarPassword(string correo, string password)
        {
            var usuario = GetUsuarioByCorreo(correo);
            if (usuario == null) return false;

            using var hmac = new HMACSHA512(Convert.FromBase64String(usuario.PasswordSalt));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(hash) == usuario.PasswordHash;
        }

        // Obtener un usuario por su correo 
        public Usuario? GetUsuarioByCorreo(string correo)
        {
            return context.Usuarios.SingleOrDefault(u => u.Correo == correo);
        }




    }
}
