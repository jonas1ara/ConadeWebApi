using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Models.Conade1;

public partial class Conade1Context : DbContext
{
    public Conade1Context()
    {
    }

    public Conade1Context(DbContextOptions<Conade1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<CatArea> CatAreas { get; set; }

    public virtual DbSet<Combustible> Combustibles { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }

    public virtual DbSet<ServicioPostal> ServicioPostals { get; set; }

    public virtual DbSet<ServicioTransporte> ServicioTransportes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioArea> UsuarioAreas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Conade1"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Area__3214EC270F649148");

            entity.ToTable("Area");

            entity.HasIndex(e => e.Nombre, "UQ__Area__75E3EFCF1EAAF06B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<CatArea>(entity =>
        {
            entity.HasKey(e => e.IdArea).HasName("PK__CatArea__2FC141AAE5A198DA");

            entity.ToTable("CatArea", tb => tb.HasTrigger("TRG_CatArea"));

            entity.Property(e => e.Clave).HasMaxLength(50);
            entity.Property(e => e.FechaCaptura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FuenteFinanciamiento).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NombreArea).HasMaxLength(100);

            entity.HasOne(d => d.Area).WithMany(p => p.CatAreas)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__CatArea__AreaId__4CEB477C");
        });

        modelBuilder.Entity<Combustible>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Combusti__3214EC272B0DC875");

            entity.ToTable("Combustible");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Combusti__584C2CBEB607DD22").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroDeSerie).HasMaxLength(20);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.TipoCombustible).HasMaxLength(255);
            entity.Property(e => e.TipoSolicitud).HasMaxLength(255);

            entity.HasOne(d => d.Area).WithMany(p => p.Combustibles)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__Combustib__AreaI__78C9C9BA");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.CombustibleAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Combustib__AreaS__75ED5D0F");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.CombustibleCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Combustib__Catal__77D5A581");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.Combustibles)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .HasConstraintName("FK__Combustib__Usuar__76E18148");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Eventos__3214EC27E1406FC7");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Eventos__584C2CBE25334A98").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroDeSerie).HasMaxLength(20);
            entity.Property(e => e.Sala).HasMaxLength(255);
            entity.Property(e => e.TipoServicio).HasMaxLength(255);
            entity.Property(e => e.TipoSolicitud).HasMaxLength(255);

            entity.HasOne(d => d.Area).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__Eventos__AreaID__63CEACD4");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.EventoAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Eventos__AreaSol__60F24029");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.EventoCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Eventos__Catalog__62DA889B");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .HasConstraintName("FK__Eventos__Usuario__61E66462");
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mantenim__3214EC27591AE432");

            entity.ToTable("Mantenimiento");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Mantenim__584C2CBE9F55AB29").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaEntrega).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroDeSerie).HasMaxLength(20);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.TipoServicio).HasMaxLength(255);
            entity.Property(e => e.TipoSolicitud).HasMaxLength(255);

            entity.HasOne(d => d.Area).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__Mantenimi__AreaI__6E4C3B47");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.MantenimientoAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mantenimi__AreaS__6B6FCE9C");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.MantenimientoCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mantenimi__Catal__6D58170E");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .HasConstraintName("FK__Mantenimi__Usuar__6C63F2D5");
        });

        modelBuilder.Entity<ServicioPostal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC277B37DBC1");

            entity.ToTable("ServicioPostal");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Servicio__584C2CBEC2CDE0B5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.DescripcionServicio).HasMaxLength(500);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroDeSerie).HasMaxLength(20);
            entity.Property(e => e.TipoDeServicio).HasMaxLength(50);
            entity.Property(e => e.TipoSolicitud).HasMaxLength(255);

            entity.HasOne(d => d.Area).WithMany(p => p.ServicioPostals)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__ServicioP__AreaI__0347582D");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.ServicioPostalAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioP__AreaS__006AEB82");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.ServicioPostalCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioP__Catal__025333F4");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.ServicioPostals)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .HasConstraintName("FK__ServicioP__Usuar__015F0FBB");
        });

        modelBuilder.Entity<ServicioTransporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC2711EC09C0");

            entity.ToTable("ServicioTransporte");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Servicio__584C2CBE7BF04413").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.DescripcionServicio).HasMaxLength(500);
            entity.Property(e => e.Destino).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroDeSerie).HasMaxLength(20);
            entity.Property(e => e.Origen).HasMaxLength(255);
            entity.Property(e => e.TipoDeServicio).HasMaxLength(50);
            entity.Property(e => e.TipoSolicitud).HasMaxLength(255);

            entity.HasOne(d => d.Area).WithMany(p => p.ServicioTransportes)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__ServicioT__AreaI__585CFA28");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.ServicioTransporteAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__AreaS__55808D7D");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.ServicioTransporteCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__Catal__5768D5EF");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.ServicioTransportes)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .HasConstraintName("FK__ServicioT__Usuar__5674B1B6");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC2747D93AE7");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApellidoMaterno).HasMaxLength(255);
            entity.Property(e => e.ApellidoPaterno).HasMaxLength(255);
            entity.Property(e => e.ClaveEmpleado).HasMaxLength(255);
            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaUltimoAcceso).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.NombreUsuario).HasMaxLength(255);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        modelBuilder.Entity<UsuarioArea>(entity =>
        {
            entity.HasKey(e => new { e.UsuarioId, e.AreaId }).HasName("PK__UsuarioA__BC36659ABDC0DDBE");

            entity.ToTable("UsuarioArea");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Area).WithMany(p => p.UsuarioAreas)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__UsuarioAr__AreaI__47326E26");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioAreas)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__UsuarioAr__Usuar__463E49ED");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
