using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AccesoDatos.Models.Conade1;

public partial class UsuarioArea
{
    public int UsuarioId { get; set; }

    public int AreaId { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
