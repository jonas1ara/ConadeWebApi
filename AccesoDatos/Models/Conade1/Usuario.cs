using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AccesoDatos.Models.Conade1;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimoAcceso { get; set; }

    public int? IdEmpleado { get; set; }

    [JsonIgnore]
    public virtual ICollection<Combustible> Combustibles { get; set; } = new List<Combustible>();

    [JsonIgnore]
    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    [JsonIgnore]
    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    [JsonIgnore]
    public virtual ICollection<ServicioPostal> ServicioPostals { get; set; } = new List<ServicioPostal>();

    [JsonIgnore]
    public virtual ICollection<ServicioTransporte> ServicioTransportes { get; set; } = new List<ServicioTransporte>();

    [JsonIgnore]
    public virtual ICollection<UsuarioArea> UsuarioAreas { get; set; } = new List<UsuarioArea>();
}
