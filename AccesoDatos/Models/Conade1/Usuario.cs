﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AccesoDatos.Models.Conade1;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public int? AreaId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimoAcceso { get; set; }

    public int? IdEmpleado { get; set; }

    public virtual Area? Area { get; set; }

    [JsonIgnore]

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    [JsonIgnore]

    public virtual ICollection<ServicioPostal> ServicioPostals { get; set; } = new List<ServicioPostal>();

    [JsonIgnore]

    public virtual ICollection<ServicioTransporte> ServicioTransportes { get; set; } = new List<ServicioTransporte>();

    [JsonIgnore]

    public virtual ICollection<UsoInmobiliario> UsoInmobiliarios { get; set; } = new List<UsoInmobiliario>();
}
