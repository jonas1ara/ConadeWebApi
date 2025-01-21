using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AccesoDatos.Models.Conade1;

public partial class Area
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;


    [JsonIgnore]
    public virtual ICollection<CatArea> CatAreas { get; set; } = new List<CatArea>();

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
