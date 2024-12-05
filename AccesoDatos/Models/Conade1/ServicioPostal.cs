using System;
using System.Collections.Generic;

namespace AccesoDatos.Models.Conade1;

public partial class ServicioPostal
{
    public int Id { get; set; }

    public string NumeroDeSerie { get; set; } = null!;

    public DateTime FechaSolicitud { get; set; }

    public int AreaSolicitante { get; set; }

    public int UsuarioSolicitante { get; set; }

    public string TipoDeServicio { get; set; } = null!;

    public string TipoSolicitud { get; set; } = null!;

    public int CatalogoId { get; set; }

    public DateOnly FechaEnvio { get; set; }

    public DateOnly FechaRecepcionMaxima { get; set; }

    public string? DescripcionServicio { get; set; }

    public int? AreaId { get; set; }


    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Area AreaSolicitanteNavigation { get; set; } = null!;

    public virtual CatArea Catalogo { get; set; } = null!;

    public virtual Usuario UsuarioSolicitanteNavigation { get; set; } = null!;

    public virtual Area? Area { get; set; }
}
