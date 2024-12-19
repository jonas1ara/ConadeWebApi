CREATE TABLE Empleados (
    IdEmpleado INT PRIMARY KEY IDENTITY(1,1),
    IdUnidadNegocio INT NOT NULL,
    IdCentroCostos INT,
    IdArea INT,
    IdRegistroPatronal INT,
    IdEntidad INT NOT NULL,
    IdPlaza INT,
    IdUbicacion INT,
    IdTipoRegimenSat INT,
    IdTipoJornadaSat INT,
    ClaveEmpleado NVARCHAR(10) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    ApellidoPaterno NVARCHAR(50) NOT NULL,
    ApellidoMaterno NVARCHAR(50),
    Sexo NVARCHAR(10),
    EstadoCivil NVARCHAR(20),
    FechaNacimiento DATE,
    SD DECIMAL(18, 2),
    Sdcotizacion DECIMAL(18, 2),
    Sdi DECIMAL(18, 4),
    NetoPagar DECIMAL(18, 2),
    IdBancoTrad INT,
    CuentaBancariaTrad NVARCHAR(25),
    CuentaInterbancariaTrad NVARCHAR(25),
    Curp NVARCHAR(18),
    Rfc NVARCHAR(13),
    NSS NVARCHAR(11),
    Foto NVARCHAR(200),
    CorreoElectronico NVARCHAR(50),
    CorreoElectronicoInstitucional NVARCHAR(50),
    Password VARBINARY(MAX),
    FechaReconocimientoAntiguedad DATE,
    FechaIngreso DATE,
    FechaAltaSs DATE,
    FechaBaja DATE,
    MotivoBaja NVARCHAR(500),
    Recontratable NVARCHAR(2),
    IdTipoContrato INT,
    IdPerfil INT,
    IdEstatus INT NOT NULL,
    IdCaptura INT NOT NULL,
    FechaCaptura DATETIME NOT NULL,
    IdBaja INT,
    FechaBajaSistema DATETIME,
    IdModificacion INT,
    FechaModificacion DATETIME,
    Contrasena VARBINARY(MAX),
    PassKiosko VARBINARY(MAX),
    Telefono NVARCHAR(50),
    Celular NVARCHAR(50),
    Nacionalidad NVARCHAR(25),
    IdSindicato INT,
    CodigoSindicato NVARCHAR(50),
    AfiliacionSindical NVARCHAR(50),
    IdCodigoPostal INT,
    Calle NVARCHAR(200),
    NumeroExt NVARCHAR(50),
    NumeroInt NVARCHAR(50),
    Sni NVARCHAR(50),
    FechaSni DATE,
    IdCodigoPostalDf INT,
    CalleDf NVARCHAR(100),
    NoExtDf NVARCHAR(50),
    NoIntDf NVARCHAR(50),
    ColoniaDf NVARCHAR(150),
    AlcaldiaDf NVARCHAR(150),
    EntidadDf NVARCHAR(150),
    Cpdf NVARCHAR(5),
    RutaCsf NVARCHAR(100),
    EfectoDesde DATE,
    EfectoHasta DATE,
    FechaCalculoRetroactivo DATE,
    AnionEfectivos INT,
    QuincenaDeAniversario INT,
    IdEmpleadoAnterior INT,
    BanderaReactivado INT,
    RetroactivoPagado INT,
    IdGradoAcademico INT,
    Observaciones NVARCHAR(300),
    IdTipoMovimiento INT,
    IdLugarNacimiento INT,
    Horario NVARCHAR(100),
    NoTarjeta NVARCHAR(10),
    IdTipoPago INT,
    TipoSangre NVARCHAR(50),
    TipoVivienda NVARCHAR(50),
    NombreContacto NVARCHAR(50),
    TelefonoContacto NVARCHAR(50),
    Discapacidad NVARCHAR(50),
    Etnia NVARCHAR(50),
    Compensacion DECIMAL(18, 2),
    IdRecaudacion INT,
    IdTipoRetencion INT,
    IdRHNET NVARCHAR(50),
    IdSHCP NVARCHAR(50),
    ID_RUSP NVARCHAR(50)
);

INSERT INTO Empleados (
    IdUnidadNegocio, IdCentroCostos, IdArea, IdRegistroPatronal, IdEntidad, 
    IdPlaza, IdUbicacion, IdTipoRegimenSat, IdTipoJornadaSat, ClaveEmpleado, 
    Nombre, ApellidoPaterno, ApellidoMaterno, Sexo, EstadoCivil, 
    FechaNacimiento, SD, Sdcotizacion, Sdi, NetoPagar, IdBancoTrad, 
    CuentaBancariaTrad, CuentaInterbancariaTrad, Curp, Rfc, NSS, 
    Foto, CorreoElectronico, CorreoElectronicoInstitucional, Password, 
    FechaReconocimientoAntiguedad, FechaIngreso, FechaAltaSs, FechaBaja, 
    MotivoBaja, Recontratable, IdTipoContrato, IdPerfil, IdEstatus, 
    IdCaptura, FechaCaptura, IdBaja, FechaBajaSistema, IdModificacion, 
    FechaModificacion, Contrasena, PassKiosko, Telefono, Celular, 
    Nacionalidad, IdSindicato, CodigoSindicato, AfiliacionSindical, IdCodigoPostal, 
    Calle, NumeroExt, NumeroInt, Sni, FechaSni, IdCodigoPostalDf, CalleDf, 
    NoExtDf, NoIntDf, ColoniaDf, AlcaldiaDf, EntidadDf, Cpdf, RutaCsf, 
    EfectoDesde, EfectoHasta, FechaCalculoRetroactivo, AnionEfectivos, QuincenaDeAniversario, 
    IdEmpleadoAnterior, BanderaReactivado, RetroactivoPagado, IdGradoAcademico, Observaciones, 
    IdTipoMovimiento, IdLugarNacimiento, Horario, NoTarjeta, IdTipoPago, 
    TipoSangre, TipoVivienda, NombreContacto, TelefonoContacto, Discapacidad, 
    Etnia, Compensacion, IdRecaudacion, IdTipoRetencion, IdRHNET, 
    IdSHCP, ID_RUSP
) VALUES
(1, 101, 2, 1, 1, 3, 4, 5, 6, 'EMP001', 'Juan', 'P�rez', 'Lopez', 'M', 'Soltero', 
 '1990-05-01', 5000, 6000, 7000, 10000, 1, '1234567890', '0987654321', 'CURP001', 'RFC001', 'NSS001', 
 'foto1.jpg', 'juan.perez@example.com', 'juan.perez@empresa.com', 0x1234, 
 '2022-01-01', '2022-02-01', '2022-03-01', NULL, NULL, 'S', 1, 2, 1, 
 100, '2024-01-01', NULL, '2024-02-01', 1001, '2024-03-01', NULL, 
 NULL, '555-1234', '555-5678', 'Mexicana', 101, 'SIND01', 'AF001', 3000, 
 'Calle Ficticia', '123', '456', 'SNI001', '2024-01-01', 3011, 'Calle DF', 
 '12', '13', 'Colonia DF', 'Alcald�a DF', 'Entidad DF', 'CP001', 'ruta/csf', 
 '2024-01-01', '2024-12-31', '2024-05-01', 1, 1, 
 NULL, 0, 0, 1, NULL, 1, NULL, NULL, 'Tarjeta001', 1, 
 'A+', 'Casa', 'Laura P�rez', '555-1234', 'Ninguna', 
 'Mestiza', 1000, 1, 2, 'RHNET001', 'SHCP001', 'RUSP001'),

(2, 102, 3, 2, 2, 5, 6, 7, 8, 'EMP002', 'Maria', 'Gomez', 'Martinez', 'F', 'Casada', 
 '1985-04-12', 7000, 8000, 9000, 12000, 2, '2345678901', '1234567890', 'CURP002', 'RFC002', 'NSS002', 
 'foto2.jpg', 'maria.gomez@example.com', 'maria.gomez@empresa.com', 0x2345, 
 '2021-05-01', '2022-06-01', '2022-07-01', NULL, NULL, 'N', 2, 3, 2, 
 101, '2024-01-02', NULL, '2024-02-02', 1002, '2024-03-02', NULL, 
 NULL, '555-2345', '555-6789', 'Mexicana', 102, 'SIND02', 'AF002', 4000, 
 'Calle Real', '789', '101', 'SNI002', '2024-02-01', 3012, 'Calle DF2', 
 '14', '15', 'Colonia DF2', 'Alcald�a DF2', 'Entidad DF2', 'CP002', 'ruta/csf2', 
 '2024-02-01', '2024-12-31', '2024-06-01', 2, 1, 
 NULL, 1, 0, 2, NULL, 2, NULL, NULL, 'Tarjeta002', 2, 
 'B+', 'Departamento', 'Carlos G�mez', '555-2345', 'Ninguna', 
 'Ind�gena', 1200, 2, 3, 'RHNET002', 'SHCP002', 'RUSP002'),

(3, 103, 4, 3, 3, 6, 7, 8, 9, 'EMP003', 'Luis', 'Mart�nez', 'Hern�ndez', 'M', 'Viudo', 
 '1992-07-23', 8000, 9000, 10000, 14000, 3, '3456789012', '2345678901', 'CURP003', 'RFC003', 'NSS003', 
 'foto3.jpg', 'luis.martinez@example.com', 'luis.martinez@empresa.com', 0x3456, 
 '2020-01-01', '2020-02-01', '2020-03-01', NULL, NULL, 'S', 3, 4, 1, 
 102, '2024-01-03', NULL, '2024-02-03', 1003, '2024-03-03', NULL, 
 NULL, '555-3456', '555-7890', 'Mexicana', 103, 'SIND03', 'AF003', 5000, 
 'Calle Azul', '101', '102', 'SNI003', '2024-03-01', 3013, 'Calle DF3', 
 '16', '17', 'Colonia DF3', 'Alcald�a DF3', 'Entidad DF3', 'CP003', 'ruta/csf3', 
 '2024-03-01', '2024-12-31', '2024-07-01', 3, 2, 
 NULL, 1, 0, 3, NULL, 3, NULL, NULL, 'Tarjeta003', 3, 
 'O-', 'Departamento', 'Sandra Mart�nez', '555-3456', 'Ninguna', 
 'Afrodescendiente', 1300, 3, 4, 'RHNET003', 'SHCP003', 'RUSP003'),

(4, 104, 5, 4, 4, 7, 8, 9, 10, 'EMP004', 'Ana', 'Lopez', 'Garcia', 'F', 'Divorciada', 
 '1988-09-15', 6000, 7000, 8000, 11000, 4, '4567890123', '3456789012', 'CURP004', 'RFC004', 'NSS004', 
 'foto4.jpg', 'ana.lopez@example.com', 'ana.lopez@empresa.com', 0x4567, 
 '2021-03-01', '2021-04-01', '2021-05-01', NULL, NULL, 'S', 4, 5, 2, 
 103, '2024-01-04', NULL, '2024-02-04', 1004, '2024-03-04', NULL, 
 NULL, '555-4567', '555-8901', 'Mexicana', 104, 'SIND04', 'AF004', 6000, 
 'Calle Verde', '234', '235', 'SNI004', '2024-04-01', 3014, 'Calle DF4', 
 '18', '19', 'Colonia DF4', 'Alcald�a DF4', 'Entidad DF4', 'CP004', 'ruta/csf4', 
 '2024-04-01', '2024-12-31', '2024-08-01', 4, 3, 
 NULL, 1, 0, 4, NULL, 4, NULL, NULL, 'Tarjeta004', 4, 
 'A+', 'Departamento', 'Raul Lopez', '555-4567', 'Ninguna', 
 'Mestiza', 1400, 4, 5, 'RHNET004', 'SHCP004', 'RUSP004'),

(5, 105, 6, 5, 5, 8, 9, 10, 11, 'EMP005', 'Carlos', 'Hernandez', 'Ramirez', 'M', 'Soltero', 
 '1994-02-10', 9000, 10000, 11000, 15000, 5, '5678901234', '4567890123', 'CURP005', 'RFC005', 'NSS005', 
 'foto5.jpg', 'carlos.hernandez@example.com', 'carlos.hernandez@empresa.com', 0x5678, 
 '2022-07-01', '2022-08-01', '2022-09-01', NULL, NULL, 'S', 5, 6, 3, 
 104, '2024-01-05', NULL, '2024-02-05', 1005, '2024-03-05', NULL, 
 NULL, '555-5678', '555-9012', 'Mexicana', 105, 'SIND05', 'AF005', 7000, 
 'Calle Amarilla', '345', '346', 'SNI005', '2024-05-01', 3015, 'Calle DF5', 
 '20', '21', 'Colonia DF5', 'Alcald�a DF5', 'Entidad DF5', 'CP005', 'ruta/csf5', 
 '2024-05-01', '2024-12-31', '2024-09-01', 5, 4, 
 NULL, 1, 0, 5, NULL, 5, NULL, NULL, 'Tarjeta005', 5, 
 'B+', 'Departamento', 'Gerardo Hernandez', '555-5678', 'Ninguna', 
 'Afrodescendiente', 1500, 5, 6, 'RHNET005', 'SHCP005', 'RUSP005'),

 (6, 106, 7, 6, 6, 9, 10, 11, 12, 'EMP006', 'JOSE', 'MARTINEZ', 'GONZALEZ', 'M', 'Soltero', 
 '1993-03-18', 10000, 11000, 12000, 16000, 6, '6789012345', '5678901234', 'CURP006', 'RFC006', 'NSS006', 
 'foto6.jpg', NULL, NULL, 0x6789, 
 '2023-01-01', '2023-02-01', '2023-03-01', NULL, NULL, 'S', 6, 7, 4, 
 105, '2024-01-06', NULL, '2024-02-06', 1006, '2024-03-06', NULL, 
 NULL, '555-6789', '555-0123', 'Mexicana', 106, 'SIND06', 'AF006', 8000, 
 'Calle Naranja', '456', '457', 'SNI006', '2024-06-01', 3016, 'Calle DF6', 
 '22', '23', 'Colonia DF6', 'Alcald�a DF6', 'Entidad DF6', 'CP006', 'ruta/csf6', 
 '2024-06-01', '2024-12-31', '2024-10-01', 6, 5, 
 NULL, 1, 0, 6, NULL, 6, NULL, NULL, 'Tarjeta006', 6, 
 'AB+', 'Casa', 'Rosa Martinez', '555-6789', 'Ninguna', 
 'Mestiza', 1600, 6, 7, 'RHNET006', 'SHCP006', 'RUSP006'),

(7, 107, 8, 7, 7, 10, 11, 12, 13, 'EMP007', 'CARLA', 'RODRIGUEZ', 'VARGAS', 'F', 'Casada', 
 '1987-11-20', 11000, 12000, 13000, 18000, 7, '7890123456', '6789012345', 'CURP007', 'RFC007', 'NSS007', 
 'foto7.jpg', NULL, NULL, 0x7890, 
 '2020-04-01', '2020-05-01', '2020-06-01', NULL, NULL, 'S', 7, 8, 5, 
 106, '2024-01-07', NULL, '2024-02-07', 1007, '2024-03-07', NULL, 
 NULL, '555-7890', '555-3456', 'Mexicana', 107, 'SIND07', 'AF007', 9000, 
 'Calle Lila', '567', '568', 'SNI007', '2024-07-01', 3017, 'Calle DF7', 
 '24', '25', 'Colonia DF7', 'Alcald�a DF7', 'Entidad DF7', 'CP007', 'ruta/csf7', 
 '2024-07-01', '2024-12-31', '2024-11-01', 7, 6, 
 NULL, 1, 0, 7, NULL, 7, NULL, NULL, 'Tarjeta007', 7, 
 'O+', 'Casa', 'Miguel Rodriguez', '555-7890', 'Ninguna', 
 'Afrodescendiente', 1700, 7, 8, 'RHNET007', 'SHCP007', 'RUSP007');

SELECT * FROM Empleados;

DROP TABLE Empleados;


