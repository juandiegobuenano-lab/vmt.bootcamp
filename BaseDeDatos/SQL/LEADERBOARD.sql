
-- CREAR BASE DE DATOS
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LeaderBoard')
    CREATE DATABASE LeaderBoard;
GO

USE LeaderBoard;
GO


-- TABLA UsuarioTipos
CREATE TABLE UsuarioTipos(
    UsuarioTipoID INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
    Description NVARCHAR (100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO


-- TABLA Usuarios
CREATE TABLE Usuarios (
    UsuarioID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    TipoID INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Edad INT NULL,
    Correo NVARCHAR (150) NOT NULL,
    Telefono NVARCHAR(10) NOT NULL,
    Cedula NVARCHAR (10) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Usuario_Tipo 
    FOREIGN KEY (TipoID)
    REFERENCES UsuarioTipos (UsuarioTipoID)
);


-- TABLA ModuloTipo
CREATE TABLE ModuloTipo(
    ModuloTipoID INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
    Especialidad NVARCHAR(100) NOT NULL,
    Tecnologia NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);


-- TABLA Modulo
CREATE TABLE Modulo(
    ModuloID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    ProfesorID UNIQUEIDENTIFIER NOT NULL,
    TipoID INT NOT NULL,

    CONSTRAINT FK_Modulo_Tipo 
    FOREIGN KEY (TipoID)
    REFERENCES ModuloTipo (ModuloTipoID),

    CONSTRAINT FK_Modulo_Profesor 
    FOREIGN KEY (ProfesorID)
    REFERENCES Usuarios (UsuarioID)
);


-- TABLA Participaciones
CREATE TABLE Participaciones(
    ParticipacionesID INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
    UsuarioID UNIQUEIDENTIFIER NOT NULL,
    ModuloID UNIQUEIDENTIFIER NOT NULL,
    Puntos DECIMAL(5,2) NOT NULL DEFAULT 0,
    FechaParticipacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Participaciones_Modulo 
    FOREIGN KEY (ModuloID)
    REFERENCES Modulo (ModuloID),

    CONSTRAINT FK_Participaciones_Estudiante 
    FOREIGN KEY (UsuarioID)
    REFERENCES Usuarios (UsuarioID)
);


-- INSERT UsuarioTipos
INSERT INTO UsuarioTipos(Description)
VALUES
('ESTUDIANTE'),
('PROFESOR');


-- INSERT ModuloTipo
INSERT INTO ModuloTipo(Especialidad, Tecnologia)
VALUES
('Motor Base/Datos','SQL Server'),
('Framework', 'Angular'),
('Framework', '.NET'),
('Framework', 'NodeJS');


-- VARIABLES
DECLARE @SQLServerModuloTipo INT= (SELECT ModuloTipoID FROM ModuloTipo WHERE Tecnologia='SQL Server');
DECLARE @AngularModuloTipo INT= (SELECT ModuloTipoID FROM ModuloTipo WHERE Tecnologia='Angular');

DECLARE @JuanVelezUserID UNIQUEIDENTIFIER = NEWID();
DECLARE @CarlosUserID UNIQUEIDENTIFIER = NEWID();
DECLARE @MariaUserID UNIQUEIDENTIFIER = NEWID();


-- INSERT USUARIOS
INSERT INTO Usuarios(UsuarioID,TipoID, Name,Edad, Telefono, Correo, Cedula)
VALUES
(@JuanVelezUserID,1, 'Juan Velez',20,'0996101033','juanbuenano@gmail.com','2000082962'),
(@MariaUserID,1, 'Maria Lopez',23,'0996202088','maria@gmail.com','2000082964'),
(@CarlosUserID,2, 'Carlos Ruiz',45,'0993332088','carlos@gmail.com','2453082964');


-- INSERT MODULOS
INSERT INTO Modulo(Name, TipoID, ProfesorID)
VALUES
('SQL Server Basics',@SQLServerModuloTipo,@CarlosUserID),
('Angular Fundamentals',@AngularModuloTipo,@CarlosUserID);


-- CONSTRAINTS UNIQUE
ALTER TABLE Usuarios 
ADD CONSTRAINT Uq_Usuarios_Correo UNIQUE (Correo);

ALTER TABLE Usuarios 
ADD CONSTRAINT Uq_Usuarios_Cedula UNIQUE (Cedula);

ALTER TABLE Usuarios 
ADD CONSTRAINT Uq_Usuarios_Telefono UNIQUE (Telefono);


-- UPDATE
UPDATE Usuarios
SET Correo ='juanbuenanovelez@gmail.com',
    Cedula='0000000000',
    Telefono='0982038493'
WHERE UsuarioID = @JuanVelezUserID;



CREATE OR ALTER PROCEDURE sp_listar_usuarios
AS
BEGIN
	SELECT * FROM Usuarios
	SELECT * FROM UsuarioTipos
END
GO


CREATE OR ALTER PROCEDURE sp_Crear_Usuarios
@nombre NVARCHAR(100),
@TipoId int,
@edad int,
@correo nvarchar(100),
@numero_cell nvarchar(32),
@cedula nvarchar(10)
AS
BEGIN
	DECLARE @IDusuario UNIQUEIDENTIFIER = NEWID()

	INSERT Usuarios(UsuarioID,Nombre, TipoID, edad,correo,Telefono,Cedula)
	Values
	(@nombre,@TipoId, @edad,@correo,@numero_cell,@cedula)
	RETURN @IDusuario 
	END 
	GO