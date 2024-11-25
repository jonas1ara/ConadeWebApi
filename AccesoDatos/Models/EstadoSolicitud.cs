using System;
using System.Collections.Generic;

namespace AccesoDatos.Models;

public partial class EstadoSolicitud
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
