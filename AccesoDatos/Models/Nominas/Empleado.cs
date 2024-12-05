using System;
using System.Collections.Generic;

namespace AccesoDatos.Models.Nominas;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public int IdUnidadNegocio { get; set; }

    public int? IdCentroCostos { get; set; }

    public int? IdArea { get; set; }

    public int? IdRegistroPatronal { get; set; }

    public int IdEntidad { get; set; }

    public int? IdPlaza { get; set; }

    public int? IdUbicacion { get; set; }

    public int? IdTipoRegimenSat { get; set; }

    public int? IdTipoJornadaSat { get; set; }

    public string ClaveEmpleado { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string? Sexo { get; set; }

    public string? EstadoCivil { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public decimal? Sd { get; set; }

    public decimal? Sdcotizacion { get; set; }

    public decimal? Sdi { get; set; }

    public decimal? NetoPagar { get; set; }

    public int? IdBancoTrad { get; set; }

    public string? CuentaBancariaTrad { get; set; }

    public string? CuentaInterbancariaTrad { get; set; }

    public string? Curp { get; set; }

    public string? Rfc { get; set; }

    public string? Nss { get; set; }

    public string? Foto { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? CorreoElectronicoInstitucional { get; set; }

    public byte[]? Password { get; set; }

    public DateOnly? FechaReconocimientoAntiguedad { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public DateOnly? FechaAltaSs { get; set; }

    public DateOnly? FechaBaja { get; set; }

    public string? MotivoBaja { get; set; }

    public string? Recontratable { get; set; }

    public int? IdTipoContrato { get; set; }

    public int? IdPerfil { get; set; }

    public int IdEstatus { get; set; }

    public int IdCaptura { get; set; }

    public DateTime FechaCaptura { get; set; }

    public int? IdBaja { get; set; }

    public DateTime? FechaBajaSistema { get; set; }

    public int? IdModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public byte[]? Contrasena { get; set; }

    public byte[]? PassKiosko { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public string? Nacionalidad { get; set; }

    public int? IdSindicato { get; set; }

    public string? CodigoSindicato { get; set; }

    public string? AfiliacionSindical { get; set; }

    public int? IdCodigoPostal { get; set; }

    public string? Calle { get; set; }

    public string? NumeroExt { get; set; }

    public string? NumeroInt { get; set; }

    public string? Sni { get; set; }

    public DateOnly? FechaSni { get; set; }

    public int? IdCodigoPostalDf { get; set; }

    public string? CalleDf { get; set; }

    public string? NoExtDf { get; set; }

    public string? NoIntDf { get; set; }

    public string? ColoniaDf { get; set; }

    public string? AlcaldiaDf { get; set; }

    public string? EntidadDf { get; set; }

    public string? Cpdf { get; set; }

    public string? RutaCsf { get; set; }

    public DateOnly? EfectoDesde { get; set; }

    public DateOnly? EfectoHasta { get; set; }

    public DateOnly? FechaCalculoRetroactivo { get; set; }

    public int? AnionEfectivos { get; set; }

    public int? QuincenaDeAniversario { get; set; }

    public int? IdEmpleadoAnterior { get; set; }

    public int? BanderaReactivado { get; set; }

    public int? RetroactivoPagado { get; set; }

    public int? IdGradoAcademico { get; set; }

    public string? Observaciones { get; set; }

    public int? IdTipoMovimiento { get; set; }

    public int? IdLugarNacimiento { get; set; }

    public string? Horario { get; set; }

    public string? NoTarjeta { get; set; }

    public int? IdTipoPago { get; set; }

    public string? TipoSangre { get; set; }

    public string? TipoVivienda { get; set; }

    public string? NombreContacto { get; set; }

    public string? TelefonoContacto { get; set; }

    public string? Discapacidad { get; set; }

    public string? Etnia { get; set; }

    public decimal? Compensacion { get; set; }

    public int? IdRecaudacion { get; set; }

    public int? IdTipoRetencion { get; set; }

    public string? IdRhnet { get; set; }

    public string? IdShcp { get; set; }

    public string? IdRusp { get; set; }
}
