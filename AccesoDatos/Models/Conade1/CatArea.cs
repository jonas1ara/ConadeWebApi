using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AccesoDatos.Models.Conade1;

public partial class CatArea
{
    public int IdArea { get; set; }

    public int? AreaId { get; set; }

    public int? IdCliente { get; set; }

    public string? Clave { get; set; }

    public string? NombreArea { get; set; }

    public decimal? FuenteFinanciamiento { get; set; }

    public int? IdEstatus { get; set; }

    public int? IdCaptura { get; set; }

    public DateTime? FechaCaptura { get; set; }

    public int? IdModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? Comentarios { get; set; }

    public virtual Area? AreaNavigation { get; set; }

    [JsonIgnore]

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    [JsonIgnore]

    public virtual ICollection<ServicioPostal> ServicioPostals { get; set; } = new List<ServicioPostal>();

    [JsonIgnore]

    public virtual ICollection<ServicioTransporte> ServicioTransportes { get; set; } = new List<ServicioTransporte>();

    [JsonIgnore]

    public virtual ICollection<UsoInmobiliario> UsoInmobiliarios { get; set; } = new List<UsoInmobiliario>();
}
