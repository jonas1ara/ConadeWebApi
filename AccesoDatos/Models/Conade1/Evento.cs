using System;
using System.Collections.Generic;

namespace AccesoDatos.Models.Conade1;

public partial class Evento
{
    public int Id { get; set; }

    public string NumeroDeSerie { get; set; } = null!;

    public DateTime FechaSolicitud { get; set; }

    public int AreaSolicitante { get; set; }

    public int UsuarioSolicitante { get; set; }

    public string TipoSolicitud { get; set; } = null!;

    public string TipoServicio { get; set; } = null!;

    public string? Sala { get; set; }

    public int CatalogoId { get; set; }

    public string? DescripcionServicio { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public TimeOnly HorarioInicio { get; set; }

    public TimeOnly HorarioFin { get; set; }

    public int? AreaId { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Area? Area { get; set; }

    public virtual CatArea AreaSolicitanteNavigation { get; set; } = null!;

    public virtual CatArea Catalogo { get; set; } = null!;

    public virtual Usuario UsuarioSolicitanteNavigation { get; set; } = null!;
}
