using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Models.Nominas;

public partial class NominaOsimulacionContext : DbContext
{
    public NominaOsimulacionContext()
    {
    }

    public NominaOsimulacionContext(DbContextOptions<NominaOsimulacionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Empleado> Empleados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Nominas"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9E844FE293");

            entity.ToTable("Empleado");

            entity.Property(e => e.AfiliacionSindical).HasMaxLength(50);
            entity.Property(e => e.AlcaldiaDf).HasMaxLength(150);
            entity.Property(e => e.ApellidoMaterno).HasMaxLength(50);
            entity.Property(e => e.ApellidoPaterno).HasMaxLength(50);
            entity.Property(e => e.Calle).HasMaxLength(200);
            entity.Property(e => e.CalleDf).HasMaxLength(100);
            entity.Property(e => e.Celular).HasMaxLength(50);
            entity.Property(e => e.ClaveEmpleado).HasMaxLength(10);
            entity.Property(e => e.CodigoSindicato).HasMaxLength(50);
            entity.Property(e => e.ColoniaDf).HasMaxLength(150);
            entity.Property(e => e.Compensacion).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CorreoElectronico).HasMaxLength(50);
            entity.Property(e => e.CorreoElectronicoInstitucional).HasMaxLength(50);
            entity.Property(e => e.Cpdf).HasMaxLength(5);
            entity.Property(e => e.CuentaBancariaTrad).HasMaxLength(25);
            entity.Property(e => e.CuentaInterbancariaTrad).HasMaxLength(25);
            entity.Property(e => e.Curp).HasMaxLength(18);
            entity.Property(e => e.Discapacidad).HasMaxLength(50);
            entity.Property(e => e.EntidadDf).HasMaxLength(150);
            entity.Property(e => e.EstadoCivil).HasMaxLength(20);
            entity.Property(e => e.Etnia).HasMaxLength(50);
            entity.Property(e => e.FechaBajaSistema).HasColumnType("datetime");
            entity.Property(e => e.FechaCaptura).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Foto).HasMaxLength(200);
            entity.Property(e => e.Horario).HasMaxLength(100);
            entity.Property(e => e.IdRhnet)
                .HasMaxLength(50)
                .HasColumnName("IdRHNET");
            entity.Property(e => e.IdRusp)
                .HasMaxLength(50)
                .HasColumnName("ID_RUSP");
            entity.Property(e => e.IdShcp)
                .HasMaxLength(50)
                .HasColumnName("IdSHCP");
            entity.Property(e => e.MotivoBaja).HasMaxLength(500);
            entity.Property(e => e.Nacionalidad).HasMaxLength(25);
            entity.Property(e => e.NetoPagar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NoExtDf).HasMaxLength(50);
            entity.Property(e => e.NoIntDf).HasMaxLength(50);
            entity.Property(e => e.NoTarjeta).HasMaxLength(10);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.NombreContacto).HasMaxLength(50);
            entity.Property(e => e.Nss)
                .HasMaxLength(11)
                .HasColumnName("NSS");
            entity.Property(e => e.NumeroExt).HasMaxLength(50);
            entity.Property(e => e.NumeroInt).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.Recontratable).HasMaxLength(2);
            entity.Property(e => e.Rfc).HasMaxLength(13);
            entity.Property(e => e.RutaCsf).HasMaxLength(100);
            entity.Property(e => e.Sd)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SD");
            entity.Property(e => e.Sdcotizacion).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sdi).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Sexo).HasMaxLength(10);
            entity.Property(e => e.Sni).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(50);
            entity.Property(e => e.TelefonoContacto).HasMaxLength(50);
            entity.Property(e => e.TipoSangre).HasMaxLength(50);
            entity.Property(e => e.TipoVivienda).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
