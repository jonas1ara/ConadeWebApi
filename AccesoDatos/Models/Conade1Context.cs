using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Models;

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

    public virtual DbSet<CatalogoInstalaciones> CatalogoInstalaciones { get; set; }

    public virtual DbSet<EstadoSolicitud> EstadoSolicituds { get; set; }

    public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }

    public virtual DbSet<Solicitud> Solicituds { get; set; }

    public virtual DbSet<TipoSolicitud> TipoSolicituds { get; set; }

    public virtual DbSet<UsoInmobiliario> UsoInmobiliarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=hp\\SQLEXPRESS; Encrypt=False; TrustServerCertificate=True; Database=Conade1; Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Area__3214EC27184A2FFA");

            entity.ToTable("Area");

            entity.HasIndex(e => e.Nombre, "UQ__Area__75E3EFCF35AB936C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<CatalogoInstalaciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Catalogo__3214EC27F74A4C54");

            entity.ToTable(tb => tb.HasTrigger("TRG_GenerarCodigoInstalacion"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Codigo).HasMaxLength(20);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.Tipo).HasMaxLength(50);
            entity.Property(e => e.Ubicacion).HasMaxLength(255);
        });

        modelBuilder.Entity<EstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstadoSo__3214EC277219D022");

            entity.ToTable("EstadoSolicitud");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mantenim__3214EC279426EFA1");

            entity.ToTable("Mantenimiento");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaEntrega).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.SolicitudId).HasColumnName("SolicitudID");
            entity.Property(e => e.TipoMantenimiento).HasMaxLength(255);

            entity.HasOne(d => d.Catalogo).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mantenimi__Catal__4589517F");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Mantenimi__Solic__44952D46");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC276EEF9D46");

            entity.ToTable("Solicitud");

            entity.HasIndex(e => e.NumeroDeSerie, "UQ__Solicitu__584C2CBEE0372359").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroDeSerie).HasMaxLength(20);

            entity.HasOne(d => d.AreaSolicitanteNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.AreaSolicitante)
                .HasConstraintName("FK__Solicitud__AreaS__336AA144");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.Estado)
                .HasConstraintName("FK__Solicitud__Estad__36470DEF");

            entity.HasOne(d => d.TipoSolicitudNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.TipoSolicitud)
                .HasConstraintName("FK__Solicitud__TipoS__3552E9B6");

            entity.HasOne(d => d.UsuarioSolicitanteNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.UsuarioSolicitante)
                .HasConstraintName("FK__Solicitud__Usuar__345EC57D");
        });

        modelBuilder.Entity<TipoSolicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoSoli__3214EC27C3C2CDB6");

            entity.ToTable("TipoSolicitud");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<UsoInmobiliario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsoInmob__3214EC27D0B6E47F");

            entity.ToTable("UsoInmobiliario");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Sala).HasMaxLength(255);
            entity.Property(e => e.SolicitudId).HasColumnName("SolicitudID");

            entity.HasOne(d => d.Catalogo).WithMany(p => p.UsoInmobiliarios)
                .HasForeignKey(d => d.CatalogoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsoInmobi__Catal__3FD07829");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.UsoInmobiliarios)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__UsoInmobi__Solic__3EDC53F0");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC270757FF53");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Correo).HasMaxLength(255);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
