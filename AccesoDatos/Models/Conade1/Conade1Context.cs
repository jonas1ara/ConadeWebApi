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

    public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }

    public virtual DbSet<ServicioPostal> ServicioPostals { get; set; }

    public virtual DbSet<ServicioTransporte> ServicioTransportes { get; set; }

    public virtual DbSet<UsoInmobiliario> UsoInmobiliarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Area__3214EC27C051A5B8");

            entity.ToTable("Area");

            entity.HasIndex(e => e.Nombre, "UQ__Area__75E3EFCF0E482563").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<CatArea>(entity =>
        {
            entity.HasKey(e => e.IdArea).HasName("PK__CatArea__2FC141AAED2FC54A");

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
                .HasConstraintName("FK__CatArea__AreaId__38A457AD");
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mantenim__3214EC273D1FD9C6");

            entity.ToTable("Mantenimiento");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Mantenim__584C2CBE7BCE508F").IsUnique();

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
                .HasConstraintName("FK__Mantenimi__AreaI__544C7222");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.MantenimientoAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mantenimi__AreaS__51700577");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.MantenimientoCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mantenimi__Catal__53584DE9");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mantenimi__Usuar__526429B0");
        });

        modelBuilder.Entity<ServicioPostal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC273DFBF529");

            entity.ToTable("ServicioPostal");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Servicio__584C2CBE8B697D65").IsUnique();

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
                .HasConstraintName("FK__ServicioP__AreaI__5ECA0095");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.ServicioPostalAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioP__AreaS__5BED93EA");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.ServicioPostalCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioP__Catal__5DD5DC5C");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.ServicioPostals)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioP__Usuar__5CE1B823");
        });

        modelBuilder.Entity<ServicioTransporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC27A06DA30E");

            entity.ToTable("ServicioTransporte");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Servicio__584C2CBEF4143FD3").IsUnique();

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
                .HasConstraintName("FK__ServicioT__AreaI__69478F08");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.ServicioTransporteAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__AreaS__666B225D");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.ServicioTransporteCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__Catal__68536ACF");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.ServicioTransportes)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__Usuar__675F4696");
        });

        modelBuilder.Entity<UsoInmobiliario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsoInmob__3214EC275C95DEA8");

            entity.ToTable("UsoInmobiliario");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__UsoInmob__584C2CBEFB7B7B5E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroDeSerie).HasMaxLength(20);
            entity.Property(e => e.Sala).HasMaxLength(255);
            entity.Property(e => e.TipoSolicitud).HasMaxLength(255);

            entity.HasOne(d => d.Area).WithMany(p => p.UsoInmobiliarios)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__UsoInmobi__AreaI__49CEE3AF");

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.UsoInmobiliarioAreaSolicitanteNavigations)
                .HasForeignKey(d => d.AreaSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsoInmobi__AreaS__46F27704");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.UsoInmobiliarioCatalogos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsoInmobi__Catal__48DABF76");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.UsoInmobiliarios)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsoInmobi__Usuar__47E69B3D");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC2703F61044");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaUltimoAcceso).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.NombreUsuario).HasMaxLength(255);
            entity.Property(e => e.Rol).HasMaxLength(50);

            entity.HasOne(d => d.Area).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__Usuario__AreaID__33DFA290");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
