-- DROP

DROP TABLE Mantenimiento;
DROP TABLE UsoInmobiliario;
DROP TABLE ServicioTransporte;
DROP TABLE ServicioPostal;

DROP TABLE CatArea;

DROP TABLE Usuario;
DROP TABLE Area;


-- Script actualizado

-- 1. Crear tabla Área
CREATE TABLE Area (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(255) UNIQUE NOT NULL
);

-- 2. Crear tabla Usuario
CREATE TABLE Usuario (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(255),
    NombreUsuario NVARCHAR(255) NOT NULL,
    Contrasena NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(50) NOT NULL,
    AreaID INT,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaUltimoAcceso DATETIME,
    IdEmpleado INT,
    FOREIGN KEY (AreaID) REFERENCES Area(ID)
);

CREATE PROCEDURE sp_AltaUsuarioDesdeEmpleado
    @IdEmpleado INT,
    @Nombre NVARCHAR(255),
    @NombreUsuario NVARCHAR(255),
    @Contrasena NVARCHAR(255),
    @Rol NVARCHAR(50),
    @NuevoUsuarioId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Verificar si el empleado existe
        IF NOT EXISTS (SELECT 1 FROM nominaOsimulacion.dbo.Empleados WHERE IdEmpleado = @IdEmpleado)
        BEGIN
            RAISERROR('El empleado no existe en la base de datos de Nómina.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Verificar si el nombre de usuario ya está registrado
        IF EXISTS (SELECT 1 FROM CONADE1.dbo.Usuario WHERE NombreUsuario = @NombreUsuario)
        BEGIN
            RAISERROR('El nombre de usuario ya está registrado.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Insertar el nuevo usuario en la tabla Usuario
        INSERT INTO CONADE1.dbo.Usuario (Nombre, NombreUsuario, Contrasena, Rol, IdEmpleado)
        VALUES (@Nombre, @NombreUsuario, @Contrasena, @Rol, @IdEmpleado);

        -- Obtener el ID del nuevo usuario insertado
        SET @NuevoUsuarioId = SCOPE_IDENTITY();

        -- Traer los datos del empleado relacionado
        SELECT * FROM nominaOsimulacion.dbo.Empleados AS e
        WHERE e.IdEmpleado = @IdEmpleado;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;

DROP PROCEDURE sp_AltaUsuarioDesdeEmpleado

-- 3. Crear tabla CatArea
CREATE TABLE CatArea (
    IdArea INT PRIMARY KEY IDENTITY(1,1),
    AreaId INT,
    IdCliente INT,
    Clave NVARCHAR(50),
    NombreArea NVARCHAR(100),
    FuenteFinanciamiento DECIMAL(18, 2),
    IdEstatus INT,
    IdCaptura INT,
    FechaCaptura DATETIME DEFAULT GETDATE(),
    IdModificacion INT,
    FechaModificacion DATETIME,
    Comentarios NVARCHAR(MAX),
    FOREIGN KEY (AreaId) REFERENCES Area(ID)
);

-- Trigger para generar el código con prefijo 'cat_XXXX'
CREATE TRIGGER TRG_CatArea
ON CatArea
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Inserted)
    BEGIN
        UPDATE CA
        SET CA.Clave = 'cat_' + RIGHT('0000' + CAST(I.IdArea AS NVARCHAR), 4)
        FROM CatArea CA
        INNER JOIN Inserted I ON CA.IdArea = I.IdArea
        WHERE I.Clave IS NULL;

        UPDATE CA
        SET CA.FechaModificacion = GETDATE()
        FROM CatArea CA
        INNER JOIN Inserted I ON CA.IdArea = I.IdArea;
    END
END;

-- 4. Crear tabla UsoInmobiliario
CREATE TABLE UsoInmobiliario (
    ID INT PRIMARY KEY IDENTITY(1,1),
    NumeroDeSerie NVARCHAR(20) UNIQUE NOT NULL,
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    AreaSolicitante INT NOT NULL,
    UsuarioSolicitante INT NOT NULL,
    TipoSolicitud NVARCHAR(255) NOT NULL CHECK (TipoSolicitud IN ('Uso Inmobiliario')),  -- Cambié aquí
    Sala NVARCHAR(255) NOT NULL,
    CatalogoID INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE,
    HorarioInicio TIME NOT NULL,
    HorarioFin TIME NOT NULL,
	AreaID INT,
    Estado NVARCHAR(50) NOT NULL CHECK (Estado IN ('Solicitada', 'Atendida', 'Rechazada')),
    Observaciones NVARCHAR(MAX),
    FOREIGN KEY (AreaSolicitante) REFERENCES CatArea(IdArea),
    FOREIGN KEY (UsuarioSolicitante) REFERENCES Usuario(ID),
    FOREIGN KEY (CatalogoID) REFERENCES CatArea(IdArea),
	FOREIGN KEY (AreaID) REFERENCES Area(ID)

);

-- 5. Crear tabla Mantenimiento
CREATE TABLE Mantenimiento (
    ID INT PRIMARY KEY IDENTITY(1,1),
    NumeroDeSerie NVARCHAR(20) UNIQUE NOT NULL,
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    AreaSolicitante INT NOT NULL,
    UsuarioSolicitante INT NOT NULL,
    TipoSolicitud NVARCHAR(255) NOT NULL CHECK (TipoSolicitud IN ('Mantenimiento')),  -- Cambié aquí
    TipoServicio NVARCHAR(255) NOT NULL CHECK (TipoServicio IN ('Preventivo', 'Correctivo')),
    CatalogoID INT NOT NULL,
	DescripcionServicio NVARCHAR(MAX),
    FechaInicio DATETIME NOT NULL,
    FechaEntrega DATETIME,
	AreaID INT,
    Estado NVARCHAR(50) NOT NULL CHECK (Estado IN ('Solicitada', 'Atendida', 'Rechazada')),
	Observaciones NVARCHAR(500),
    FOREIGN KEY (AreaSolicitante) REFERENCES CatArea(IdArea),
    FOREIGN KEY (UsuarioSolicitante) REFERENCES Usuario(ID),
    FOREIGN KEY (CatalogoID) REFERENCES CatArea(IdArea),
	FOREIGN KEY (AreaID) REFERENCES Area(ID)

);

-- 6. Crear tabla ServicioPostal
CREATE TABLE ServicioPostal (
    ID INT PRIMARY KEY IDENTITY(1,1),
    NumeroDeSerie NVARCHAR(20) UNIQUE NOT NULL,
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    AreaSolicitante INT NOT NULL,
    UsuarioSolicitante INT NOT NULL,
    TipoDeServicio NVARCHAR(50) NOT NULL CHECK (TipoDeServicio IN ('Llevar', 'Recoger', 'Llevar y Recoger')),
	TipoSolicitud NVARCHAR(255) NOT NULL CHECK (TipoSolicitud IN ('Servicio Postal')),  -- Cambié aquí
	CatalogoID INT NOT NULL,
    FechaEnvio DATE NOT NULL,
    FechaRecepcionMaxima DATE NOT NULL,
    DescripcionServicio NVARCHAR(500),
	AreaID INT,
    Estado NVARCHAR(50) NOT NULL CHECK (Estado IN ('Solicitada', 'Atendida', 'Rechazada')),
    Observaciones NVARCHAR(MAX),
    FOREIGN KEY (AreaSolicitante) REFERENCES CatArea(IdArea),
    FOREIGN KEY (UsuarioSolicitante) REFERENCES Usuario(ID),
    FOREIGN KEY (CatalogoID) REFERENCES CatArea(IdArea),
	FOREIGN KEY (AreaID) REFERENCES Area(ID)

);

-- 7. Crear tabla ServicioTransporte
CREATE TABLE ServicioTransporte (
    ID INT PRIMARY KEY IDENTITY(1,1),
    NumeroDeSerie NVARCHAR(20) UNIQUE NOT NULL,
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    AreaSolicitante INT NOT NULL,
    UsuarioSolicitante INT NOT NULL,
    TipoDeServicio NVARCHAR(50) NOT NULL CHECK (TipoDeServicio IN ('Llevar', 'Recoger', 'Llevar y Recoger')),
	TipoSolicitud NVARCHAR(255) NOT NULL CHECK (TipoSolicitud IN ('Servicio Transporte')),
	CatalogoID INT NOT NULL,
    FechaTransporte DATE NOT NULL,
	FechaTransporteVuelta DATE,
    Origen NVARCHAR(255) NOT NULL,
    Destino NVARCHAR(255) NOT NULL,
    DescripcionServicio NVARCHAR(500),
	AreaID INT,
    Estado NVARCHAR(50) NOT NULL CHECK (Estado IN ('Solicitada', 'Atendida', 'Rechazada')),
    Observaciones NVARCHAR(MAX),
    FOREIGN KEY (AreaSolicitante) REFERENCES CatArea(IdArea),
    FOREIGN KEY (UsuarioSolicitante) REFERENCES Usuario(ID),
    FOREIGN KEY (CatalogoID) REFERENCES CatArea(IdArea),
	FOREIGN KEY (AreaID) REFERENCES Area(ID)
);

--- Prueba de la base de datos
-- Insertar áreas
INSERT INTO Area (Nombre) 
VALUES ('Servicio Postal'), ('Servicio de Transporte'), ('Uso Auditorios'), ('Mantenimiento');

-- Insertar áreas en CatArea
INSERT INTO CatArea (AreaId, Clave, NombreArea, FuenteFinanciamiento, IdEstatus, IdCaptura, IdModificacion, FechaModificacion, Comentarios) VALUES 
(1, 'A001', 'CENAR', 10000.00, 1, 1, 2, GETDATE(), 'Área principal de administración y recursos'),  -- Relación con la zona 1
(2, 'L002', 'Pista atletismo Polanco', 5000.00, 2, 1, 3, GETDATE(), 'Pista para eventos y entrenamientos'),  -- Relación con la zona 2
(3, 'A003', 'Auditoría', 2000.00, 1, 1, 2, GETDATE(), 'Área encargada de la auditoría interna'),  -- Relación con la zona 3
(4, 'S004', 'Centro Acuático de alto rendimiento', 1500.00, 1, 1, 3, GETDATE(), 'Centro para entrenamientos de natación y deportes acuáticos'),  -- Relación con la zona 4
(1, 'T005', 'Cancha de Beisbol Ensenada', 3000.00, 1, 1, 2, GETDATE(), 'Cancha de beisbol para entrenamiento y competencias'),  -- Relación con la zona 1
(2, 'L006', 'Pista de Atletismo Guadalajara', 6000.00, 1, 2, 3, GETDATE(), 'Pista de atletismo para entrenamientos y eventos'),  -- Relación con la zona 2
(3, 'A007', 'Auditoría Interna', 4000.00, 2, 2, 2, GETDATE(), 'Auditoría enfocada en procesos internos de la organización'),  -- Relación con la zona 3
(4, 'S008', 'Auditorio Central', 5000.00, 1, 2, 3, GETDATE(), 'Auditorio para eventos y presentaciones internas');  -- Relación con la zona 4


-- Insertar datos en la tabla UsoInmobiliario
INSERT INTO UsoInmobiliario (NumeroDeSerie, FechaSolicitud, AreaSolicitante, UsuarioSolicitante, TipoSolicitud, Sala, CatalogoID, FechaInicio, FechaFin, HorarioInicio, HorarioFin, AreaID, Estado, Observaciones)
VALUES
('U12345', '2024-12-01', 1, 2, 'Uso Inmobiliario', 'Sala de Juntas 1', 1, '2024-12-02', '2024-12-02', '09:00', '11:00', 3, 'Solicitada', NULL),
('U12346', '2024-12-02', 2, 2, 'Uso Inmobiliario', 'Sala de Conferencias', 2, '2024-12-03', '2024-12-03', '10:00', '12:00', 3, 'Solicitada', NULL),
('U12347', '2024-12-03', 3, 5, 'Uso Inmobiliario', 'Sala de Reuniones', 3, '2024-12-04', '2024-12-04', '11:00', '13:00', 3, 'Solicitada', NULL),
('U12348', '2024-12-04', 4, 2, 'Uso Inmobiliario', 'Auditorio', 4, '2024-12-05', '2024-12-05', '12:00', '14:00', 3, 'Solicitada', NULL),
('U12349', '2024-12-05', 5, 3, 'Uso Inmobiliario', 'Sala de Conferencias', 5, '2024-12-06', '2024-12-06', '13:00', '15:00', 3, 'Solicitada', NULL),
('U12350', '2024-12-06', 6, 5, 'Uso Inmobiliario', 'Sala de Juntas 2', 6, '2024-12-07', '2024-12-07', '14:00', '16:00', 3, 'Solicitada', NULL),
('U12351', '2024-12-07', 7, 2, 'Uso Inmobiliario', 'Sala de Reuniones', 7, '2024-12-08', '2024-12-08', '15:00', '17:00', 3, 'Solicitada', NULL),
('U12352', '2024-12-08', 8, 3, 'Uso Inmobiliario', 'Auditorio', 1, '2024-12-09', '2024-12-09', '16:00', '18:00', 3, 'Solicitada', NULL),
('U12353', '2024-12-09', 1, 5, 'Uso Inmobiliario', 'Sala de Juntas 1', 2, '2024-12-10', '2024-12-10', '17:00', '19:00', 3, 'Solicitada', NULL),
('U12354', '2024-12-10', 2, 2, 'Uso Inmobiliario', 'Sala de Conferencias', 3, '2024-12-11', '2024-12-11', '09:00', '11:00', 3, 'Solicitada', NULL),
('U12355', '2024-12-11', 3, 3, 'Uso Inmobiliario', 'Sala de Reuniones', 4, '2024-12-12', '2024-12-12', '10:00', '12:00', 3, 'Solicitada', NULL),
('U12356', '2024-12-12', 4, 5, 'Uso Inmobiliario', 'Auditorio', 5, '2024-12-13', '2024-12-13', '11:00', '13:00', 3, 'Solicitada', NULL),
('U12357', '2024-12-13', 5, 2, 'Uso Inmobiliario', 'Sala de Juntas 2', 6, '2024-12-14', '2024-12-14', '12:00', '14:00', 3, 'Solicitada', NULL),
('U12358', '2024-12-14', 6, 3, 'Uso Inmobiliario', 'Sala de Reuniones', 7, '2024-12-15', '2024-12-15', '13:00', '15:00', 3, 'Solicitada', NULL),
('U12359', '2024-12-15', 7, 5, 'Uso Inmobiliario', 'Auditorio', 1, '2024-12-16', '2024-12-16', '14:00', '16:00', 3, 'Solicitada', NULL),
('U12360', '2024-12-16', 8, 2, 'Uso Inmobiliario', 'Sala de Juntas 1', 2, '2024-12-17', '2024-12-17', '15:00', '17:00', 3, 'Solicitada', NULL),
('U12361', '2024-12-17', 1, 3, 'Uso Inmobiliario', 'Sala de Conferencias', 3, '2024-12-18', '2024-12-18', '16:00', '18:00', 3, 'Solicitada', NULL),
('U12362', '2024-12-18', 2, 5, 'Uso Inmobiliario', 'Sala de Reuniones', 4, '2024-12-19', '2024-12-19', '17:00', '19:00', 3, 'Solicitada', NULL),
('U12363', '2024-12-19', 3, 2, 'Uso Inmobiliario', 'Auditorio', 5, '2024-12-20', '2024-12-20', '09:00', '11:00', 3, 'Solicitada', NULL),
('U12364', '2024-12-20', 4, 3, 'Uso Inmobiliario', 'Sala de Juntas 2', 6, '2024-12-21', '2024-12-21', '10:00', '12:00', 3, 'Solicitada', NULL),
('U12365', '2024-12-21', 5, 5, 'Uso Inmobiliario', 'Sala de Reuniones', 7, '2024-12-22', '2024-12-22', '11:00', '13:00', 3, 'Solicitada', NULL),
('U12366', '2024-12-22', 6, 2, 'Uso Inmobiliario', 'Auditorio', 1, '2024-12-23', '2024-12-23', '12:00', '14:00', 3, 'Solicitada', NULL),
('U12367', '2024-12-23', 7, 3, 'Uso Inmobiliario', 'Sala de Juntas 1', 2, '2024-12-24', '2024-12-24', '13:00', '15:00', 3, 'Solicitada', NULL),
('U12368', '2024-12-24', 8, 5, 'Uso Inmobiliario', 'Sala de Conferencias', 3, '2024-12-25', '2024-12-25', '14:00', '16:00', 3, 'Solicitada', NULL),
('U12369', '2024-12-25', 1, 2, 'Uso Inmobiliario', 'Sala de Reuniones', 4, '2024-12-26', '2024-12-26', '15:00', '17:00', 3, 'Solicitada', NULL);


-- Insertar datos en la tabla Mantenimiento
INSERT INTO Mantenimiento (NumeroDeSerie, FechaSolicitud, AreaSolicitante, UsuarioSolicitante, TipoSolicitud, TipoServicio, CatalogoID, DescripcionServicio, FechaInicio, FechaEntrega, AreaID, Estado, Observaciones)
VALUES
('M12345', '2024-12-01', 1, 2, 'Mantenimiento', 'Preventivo', 1, 'Mantenimiento preventivo de equipos', '2024-12-05', '2024-12-06', 4, 'Solicitada', NULL),
('M12346', '2024-12-02', 2, 2, 'Mantenimiento', 'Correctivo', 2, 'Reparación de servidores', '2024-12-07', '2024-12-08', 4, 'Solicitada', NULL),
('M12347', '2024-12-03', 3, 5, 'Mantenimiento', 'Preventivo', 3, 'Monitoreo de sistemas críticos', '2024-12-09', '2024-12-10', 3, 'Solicitada', NULL),
('M12348', '2024-12-04', 4, 2, 'Mantenimiento', 'Correctivo', 4, 'Ajuste de red', '2024-12-11', '2024-12-12', 3, 'Solicitada', NULL),
('M12349', '2024-12-05', 5, 3, 'Mantenimiento', 'Preventivo', 5, 'Limpieza de sistemas', '2024-12-13', '2024-12-14', 3, 'Solicitada', NULL),
('M12350', '2024-12-06', 6, 5, 'Mantenimiento', 'Preventivo', 6, 'Verificación de seguridad', '2024-12-15', '2024-12-16', 3, 'Solicitada', NULL),
('M12351', '2024-12-07', 7, 2, 'Mantenimiento', 'Correctivo', 7, 'Reparación de impresoras', '2024-12-17', '2024-12-18', 3, 'Solicitada', NULL),
('M12352', '2024-12-08', 8, 3, 'Mantenimiento', 'Preventivo', 1, 'Chequeo de sistemas de respaldo', '2024-12-19', '2024-12-20', 3, 'Solicitada', NULL),
('M12353', '2024-12-09', 1, 5, 'Mantenimiento', 'Preventivo', 2, 'Inspección de redes', '2024-12-21', '2024-12-22', 3, 'Solicitada', NULL),
('M12354', '2024-12-10', 2, 2, 'Mantenimiento', 'Correctivo', 3, 'Reparación de hardware', '2024-12-23', '2024-12-24', 3, 'Solicitada', NULL),
('M12355', '2024-12-11', 3, 3, 'Mantenimiento', 'Preventivo', 4, 'Actualización de software', '2024-12-25', '2024-12-26', 3, 'Solicitada', NULL),
('M12356', '2024-12-12', 4, 5, 'Mantenimiento', 'Preventivo', 5, 'Monitoreo de bases de datos', '2024-12-27', '2024-12-28', 3, 'Solicitada', NULL),
('M12357', '2024-12-13', 5, 2, 'Mantenimiento', 'Correctivo', 6, 'Solución de problemas de red', '2024-12-29', '2024-12-30', 3, 'Solicitada', NULL),
('M12358', '2024-12-14', 6, 3, 'Mantenimiento', 'Preventivo', 7, 'Verificación de sistemas de seguridad', '2024-12-31', '2025-01-01', 3, 'Solicitada', NULL),
('M12359', '2024-12-15', 7, 5, 'Mantenimiento', 'Correctivo', 1, 'Reparación de sistemas HVAC', '2025-01-02', '2025-01-03', 3, 'Solicitada', NULL),
('M12360', '2024-12-16', 8, 2, 'Mantenimiento', 'Preventivo', 2, 'Inspección de energía', '2025-01-04', '2025-01-05', 3, 'Solicitada', NULL),
('M12361', '2024-12-17', 1, 3, 'Mantenimiento', 'Preventivo', 3, 'Revisión de UPS', '2025-01-06', '2025-01-07', 3, 'Solicitada', NULL),
('M12362', '2024-12-18', 2, 5, 'Mantenimiento', 'Correctivo', 4, 'Solución de problemas de software', '2025-01-08', '2025-01-09', 3, 'Solicitada', NULL),
('M12363', '2024-12-19', 3, 2, 'Mantenimiento', 'Preventivo', 5, 'Análisis de rendimiento de servidores', '2025-01-10', '2025-01-11', 3, 'Solicitada', NULL),
('M12364', '2024-12-20', 4, 3, 'Mantenimiento', 'Preventivo', 6, 'Limpieza de sistemas informáticos', '2025-01-12', '2025-01-13', 3, 'Solicitada', NULL);


-- Insertar datos en la tabla ServicioPostal
INSERT INTO ServicioPostal (NumeroDeSerie, FechaSolicitud, AreaSolicitante, UsuarioSolicitante, TipoDeServicio, TipoSolicitud, CatalogoID, FechaEnvio, FechaRecepcionMaxima, DescripcionServicio, AreaID, Estado, Observaciones)
VALUES
('S12345', '2024-12-01', 1, 1, 'Llevar', 'Servicio Postal', 1, '2024-12-02', '2024-12-03', 'Envío de documentos importantes', 1, 'Solicitada', NULL),
('S12346', '2024-12-02', 2, 2, 'Recoger', 'Servicio Postal', 2, '2024-12-04', '2024-12-05', 'Recogida de paquetes', 1, 'Solicitada', NULL),
('S12347', '2024-12-03', 3, 5, 'Llevar', 'Servicio Postal', 3, '2024-12-05', '2024-12-06', 'Envío de contratos', 3, 'Solicitada', NULL),
('S12348', '2024-12-04', 4, 2, 'Recoger', 'Servicio Postal', 4, '2024-12-07', '2024-12-08', 'Recogida de documentos legales', 3, 'Solicitada', NULL),
('S12349', '2024-12-05', 5, 3, 'Llevar', 'Servicio Postal', 5, '2024-12-09', '2024-12-10', 'Envío de muestras de producto', 3, 'Solicitada', NULL),
('S12350', '2024-12-06', 6, 5, 'Recoger', 'Servicio Postal', 6, '2024-12-11', '2024-12-12', 'Recogida de correspondencia', 3, 'Solicitada', NULL),
('S12351', '2024-12-07', 7, 2, 'Llevar', 'Servicio Postal', 7, '2024-12-13', '2024-12-14', 'Envío de paquetes promocionales', 3, 'Solicitada', NULL),
('S12352', '2024-12-08', 8, 3, 'Recoger', 'Servicio Postal', 1, '2024-12-15', '2024-12-16', 'Recogida de facturas', 3, 'Solicitada', NULL),
('S12353', '2024-12-09', 1, 5, 'Llevar', 'Servicio Postal', 2, '2024-12-17', '2024-12-18', 'Envío de documentación interna', 3, 'Solicitada', NULL),
('S12354', '2024-12-10', 2, 2, 'Recoger', 'Servicio Postal', 3, '2024-12-19', '2024-12-20', 'Recogida de informes', 3, 'Solicitada', NULL),
('S12355', '2024-12-11', 3, 3, 'Llevar', 'Servicio Postal', 4, '2024-12-21', '2024-12-22', 'Envío de materiales de formación', 3, 'Solicitada', NULL),
('S12356', '2024-12-12', 4, 5, 'Recoger', 'Servicio Postal', 5, '2024-12-23', '2024-12-24', 'Recogida de muestras de producto', 3, 'Solicitada', NULL),
('S12357', '2024-12-13', 5, 2, 'Llevar', 'Servicio Postal', 6, '2024-12-25', '2024-12-26', 'Envío de correspondencia legal', 3, 'Solicitada', NULL),
('S12358', '2024-12-14', 6, 3, 'Recoger', 'Servicio Postal', 7, '2024-12-27', '2024-12-28', 'Recogida de contratos firmados', 3, 'Solicitada', NULL),
('S12359', '2024-12-15', 7, 5, 'Llevar', 'Servicio Postal', 1, '2024-12-29', '2024-12-30', 'Envío de documentación de proyecto', 3, 'Solicitada', NULL),
('S12360', '2024-12-16', 8, 2, 'Recoger', 'Servicio Postal', 2, '2024-12-31', '2025-01-01', 'Recogida de documentos administrativos', 3, 'Solicitada', NULL),
('S12361', '2024-12-17', 1, 3, 'Llevar', 'Servicio Postal', 3, '2025-01-02', '2025-01-03', 'Envío de facturas', 3, 'Solicitada', NULL),
('S12362', '2024-12-18', 2, 5, 'Recoger', 'Servicio Postal', 4, '2025-01-04', '2025-01-05', 'Recogida de correspondencia interna', 3, 'Solicitada', NULL),
('S12363', '2024-12-19', 3, 2, 'Llevar', 'Servicio Postal', 5, '2025-01-06', '2025-01-07', 'Envío de documentos financieros', 3, 'Solicitada', NULL),
('S12364', '2024-12-20', 4, 3, 'Recoger', 'Servicio Postal', 6, '2025-01-08', '2025-01-09', 'Recogida de materiales de marketing', 3, 'Solicitada', NULL),
('S12365', '2024-12-21', 5, 5, 'Llevar', 'Servicio Postal', 7, '2025-01-10', '2025-01-11', 'Envío de invitaciones', 3, 'Solicitada', NULL),
('S12366', '2024-12-22', 6, 2, 'Recoger', 'Servicio Postal', 1, '2025-01-12', '2025-01-13', 'Recogida de archivos físicos', 3, 'Solicitada', NULL),
('S12367', '2024-12-23', 7, 3, 'Llevar', 'Servicio Postal', 2, '2025-01-14', '2025-01-15', 'Envío de boletines internos', 3, 'Solicitada', NULL),
('S12368', '2024-12-24', 8, 5, 'Recoger', 'Servicio Postal', 3, '2025-01-16', '2025-01-17', 'Recogida de documentación para auditoría', 3, 'Solicitada', NULL),
('S12369', '2024-12-25', 1, 2, 'Llevar', 'Servicio Postal', 4, '2025-01-18', '2025-01-19', 'Envío de contratos a clientes', 3, 'Solicitada', NULL);


-- Insertar datos en la tabla ServicioTransporte
INSERT INTO ServicioTransporte (NumeroDeSerie, FechaSolicitud, AreaSolicitante, UsuarioSolicitante, TipoDeServicio, TipoSolicitud, CatalogoID, FechaTransporte, Origen, Destino, AreaID, Estado, Observaciones)
VALUES
('T12345', '2024-12-01', 1, 2, 'Llevar', 'Servicio Transporte', 1, '2024-12-03', 'Oficinas Centrales', 'Sucursal A', 2, 'Solicitada', NULL),
('T12346', '2024-12-02', 2, 2, 'Recoger', 'Servicio Transporte', 2, '2024-12-04', 'Sucursal B', 'Oficinas Centrales', 2, 'Solicitada', NULL),
('T12347', '2024-12-03', 3, 5, 'Llevar', 'Servicio Transporte', 3, '2024-12-05', 'Sucursal C', 'Sucursal D', 3, 'Solicitada', NULL),
('T12348', '2024-12-04', 4, 2, 'Recoger', 'Servicio Transporte', 4, '2024-12-06', 'Sucursal E', 'Sucursal F', 3, 'Solicitada', NULL),
('T12349', '2024-12-05', 5, 3, 'Llevar', 'Servicio Transporte', 5, '2024-12-07', 'Sucursal G', 'Sucursal H', 3, 'Solicitada', NULL),
('T12350', '2024-12-06', 6, 5, 'Recoger', 'Servicio Transporte', 6, '2024-12-08', 'Oficinas Centrales', 'Sucursal I', 3, 'Solicitada', NULL),
('T12351', '2024-12-07', 7, 2, 'Llevar', 'Servicio Transporte', 7, '2024-12-09', 'Sucursal J', 'Sucursal K', 3, 'Solicitada', NULL),
('T12352', '2024-12-08', 8, 3, 'Recoger', 'Servicio Transporte', 1, '2024-12-10', 'Sucursal L', 'Oficinas Centrales', 3, 'Solicitada', NULL),
('T12353', '2024-12-09', 1, 5, 'Llevar', 'Servicio Transporte', 2, '2024-12-11', 'Sucursal M', 'Sucursal N', 3, 'Solicitada', NULL),
('T12354', '2024-12-10', 2, 2, 'Recoger', 'Servicio Transporte', 3, '2024-12-12', 'Sucursal O', 'Oficinas Centrales', 3, 'Solicitada', NULL),
('T12355', '2024-12-11', 3, 3, 'Llevar', 'Servicio Transporte', 4, '2024-12-13', 'Sucursal P', 'Sucursal Q', 3, 'Solicitada', NULL),
('T12356', '2024-12-12', 4, 5, 'Recoger', 'Servicio Transporte', 5, '2024-12-14', 'Sucursal R', 'Oficinas Centrales', 3, 'Solicitada', NULL),
('T12357', '2024-12-13', 5, 2, 'Llevar', 'Servicio Transporte', 6, '2024-12-15', 'Sucursal S', 'Sucursal T', 3, 'Solicitada', NULL),
('T12358', '2024-12-14', 6, 3, 'Recoger', 'Servicio Transporte', 7, '2024-12-16', 'Sucursal U', 'Oficinas Centrales', 3, 'Solicitada', NULL),
('T12359', '2024-12-15', 7, 5, 'Llevar', 'Servicio Transporte', 1, '2024-12-17', 'Oficinas Centrales', 'Sucursal V', 3, 'Solicitada', NULL),
('T12360', '2024-12-16', 8, 2, 'Recoger', 'Servicio Transporte', 2, '2024-12-18', 'Sucursal W', 'Sucursal X', 3, 'Solicitada', NULL),
('T12361', '2024-12-17', 1, 3, 'Llevar', 'Servicio Transporte', 3, '2024-12-19', 'Sucursal Y', 'Sucursal Z', 3, 'Solicitada', NULL),
('T12362', '2024-12-18', 2, 5, 'Recoger', 'Servicio Transporte', 4, '2024-12-20', 'Sucursal AA', 'Sucursal BB', 3, 'Solicitada', NULL),
('T12363', '2024-12-19', 3, 2, 'Llevar', 'Servicio Transporte', 5, '2024-12-21', 'Sucursal CC', 'Sucursal DD', 3, 'Solicitada', NULL),
('T12364', '2024-12-20', 4, 3, 'Recoger', 'Servicio Transporte', 6, '2024-12-22', 'Sucursal EE', 'Sucursal FF', 3, 'Solicitada', NULL),
('T12365', '2024-12-21', 5, 5, 'Llevar', 'Servicio Transporte', 7, '2024-12-23', 'Sucursal GG', 'Sucursal HH', 3, 'Solicitada', NULL),
('T12366', '2024-12-22', 6, 2, 'Recoger', 'Servicio Transporte', 1, '2024-12-24', 'Sucursal II', 'Oficinas Centrales', 3, 'Solicitada', NULL),
('T12367', '2024-12-23', 7, 3, 'Llevar', 'Servicio Transporte', 2, '2024-12-26', 'Sucursal JJ', 'Sucursal KK', 3, 'Solicitada', NULL),
('T12368', '2024-12-24', 8, 5, 'Recoger', 'Servicio Transporte', 3, '2024-12-27', 'Sucursal LL', 'Sucursal MM', 3, 'Solicitada', NULL),
('T12369', '2024-12-25', 1, 2, 'Llevar', 'Servicio Transporte', 4, '2024-12-28', 'Oficinas Centrales', 'Sucursal NN', 3, 'Solicitada', NULL),
('T12370', '2024-12-26', 2, 3, 'Recoger', 'Servicio Transporte', 5, '2024-12-29', 'Sucursal OO', 'Sucursal PP', 3, 'Solicitada', NULL);


-- Filtros de prueba


SELECT * FROM CatArea;

SELECT * FROM Mantenimiento;

SELECT * FROM UsoInmobiliario;

SELECT * FROM Usuario;

SELECT * FROM Area;

SELECT * FROM ServicioPostal;

SELECT * FROM ServicioTransporte;


SELECT *
FROM Solicitudes
WHERE AreaSolicitante = @areaId



-- Obtener todos los usuarios con rol 'Usuario'
SELECT * 
FROM Mantenimiento
WHERE UsuarioSolicitante = 1;


-- Obtener todas las solicitudes de Uso Inmobiliario con estado 'Solicitada'
SELECT * 
FROM UsoInmobiliario
WHERE Estado = 'Solicitada';


-- Obtener todos los mantenimientos de tipo 'Correctivo'
SELECT * 
FROM Mantenimiento
WHERE TipoServicio = 'Correctivo';

-- Obtener todos los servicios postales de tipo 'Llevar'
SELECT * 
FROM ServicioPostal
WHERE TipoDeServicio = 'Llevar';


-- Obtener todos los servicios de transporte con estado 'Atendida'
SELECT * 
FROM ServicioTransporte
WHERE Estado = 'Atendida';

-- Obtener todas las áreas cuyo financiamiento sea mayor a 10000
SELECT * 
FROM CatArea
WHERE FuenteFinanciamiento > 10000;

-- Obtener todas las solicitudes de Uso Inmobiliario realizadas en '2024-12-01'
SELECT * 
FROM UsoInmobiliario
WHERE FechaSolicitud = '2024-12-01';

-- Obtener todos los usuarios de la 'Área de Tecnología' (suponiendo AreaID = 2)
SELECT * 
FROM Usuario
WHERE AreaID = 2;

-- Obtener todos los servicios postales enviados después del 2024-12-02
SELECT * 
FROM ServicioPostal
WHERE FechaEnvio > '2024-12-02';

SELECT * FROM Mantenimiento WHERE rol = Admin;




