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

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    [JsonIgnore]

    public virtual ICollection<ServicioPostal> ServicioPostals { get; set; } = new List<ServicioPostal>();

    [JsonIgnore]

    public virtual ICollection<ServicioTransporte> ServicioTransportes { get; set; } = new List<ServicioTransporte>();

    [JsonIgnore]

    public virtual ICollection<UsoInmobiliario> UsoInmobiliarios { get; set; } = new List<UsoInmobiliario>();

    [JsonIgnore]

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
