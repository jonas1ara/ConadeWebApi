using System;
using System.Collections.Generic;

namespace AccesoDatos.Models;

public partial class Solicitud
{
    public int Id { get; set; }

    public string NumeroDeSerie { get; set; } = null!;

    public DateTime FechaSolicitud { get; set; }

    public int? AreaSolicitante { get; set; }

    public int? UsuarioSolicitante { get; set; }

    public int? TipoSolicitud { get; set; }

    public int? Estado { get; set; }

    public string? Observaciones { get; set; }

    public virtual Area? AreaSolicitanteNavigation { get; set; }

    public virtual EstadoSolicitud? EstadoNavigation { get; set; }

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    public virtual TipoSolicitud? TipoSolicitudNavigation { get; set; }

    public virtual ICollection<UsoInmobiliario> UsoInmobiliarios { get; set; } = new List<UsoInmobiliario>();

    public virtual Usuario? UsuarioSolicitanteNavigation { get; set; }
}
