using System;
using System.Collections.Generic;

namespace AccesoDatos.Models;

public partial class CatalogoInstalaciones
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public int Capacidad { get; set; }

    public string Ciudad { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Ubicacion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string? Comentarios { get; set; }

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    public virtual ICollection<UsoInmobiliario> UsoInmobiliarios { get; set; } = new List<UsoInmobiliario>();
}
