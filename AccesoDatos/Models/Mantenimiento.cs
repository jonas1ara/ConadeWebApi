using System;
using System.Collections.Generic;

namespace AccesoDatos.Models;

public partial class Mantenimiento
{
    public int Id { get; set; }

    public int? SolicitudId { get; set; }

    public int CatalogoId { get; set; }

    public string TipoMantenimiento { get; set; } = null!;

    public string? DescripcionServicio { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string Estado { get; set; } = null!;

    public virtual CatalogoInstalaciones Catalogo { get; set; } = null!;

    public virtual Solicitud? Solicitud { get; set; }
}
