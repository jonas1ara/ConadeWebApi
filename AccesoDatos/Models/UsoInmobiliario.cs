using System;
using System.Collections.Generic;

namespace AccesoDatos.Models;

public partial class UsoInmobiliario
{
    public int Id { get; set; }

    public int? SolicitudId { get; set; }

    public string Sala { get; set; } = null!;

    public int CatalogoId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public TimeSpan HorarioInicio { get; set; }

    public TimeSpan HorarioFin { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual CatalogoInstalaciones Catalogo { get; set; } = null!;

    public virtual Solicitud? Solicitud { get; set; }
}
