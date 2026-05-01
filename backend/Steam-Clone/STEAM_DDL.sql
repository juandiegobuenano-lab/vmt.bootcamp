-- CREAR BASE DE DATOS
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'STEAM')
    CREATE DATABASE STEAM;
GO

USE STEAM
GO


-- TABLA STATUS
DROP TABLE IF EXISTS Status;

CREATE TABLE Status(
    StatusID INT NOT NULL PRIMARY KEY, -- 👈 sin IDENTITY
    Code NVARCHAR(20) NOT NULL,
    ShowName NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

INSERT INTO Status (StatusID, Code, ShowName)
VALUES 
(1, 'ACTIVE', 'Activo'),
(0, 'INACTIVE', 'Desactivo');

ALTER TABLE Users
ADD CONSTRAINT FK_Users_Status
FOREIGN KEY (StatusID) REFERENCES Status(StatusID);




--TABLA Usuario
CREATE TABLE Users (
    UserID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) UNIQUE,
    Password NVARCHAR(255),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    DeletedAt DATETIME2,
    StatusID INT,
    FOREIGN KEY (StatusID) REFERENCES Status(StatusID)
);

-- TABLA AMIGOS
CREATE TABLE Friends (
    UserID UNIQUEIDENTIFIER,
    FriendID UNIQUEIDENTIFIER,
    AddedAt DATETIME2 DEFAULT SYSUTCDATETIME(),

    PRIMARY KEY (UserID, FriendID),

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (FriendID) REFERENCES Users(UserID)
);

--TABLA DESARROLLADOR
CREATE TABLE Developer (
    DeveloperID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    DeveloperName NVARCHAR(150),
    Email NVARCHAR(150),
	Password NVARCHAR(255),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    DeletedAt DATETIME2
);
-- TABLA EDITOR
CREATE TABLE Publishers (
    PublisherID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    PublisherName NVARCHAR(150),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    DeletedAt DATETIME2
);

--TABLA JUEGOS
CREATE TABLE Games (
    GameID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    Title NVARCHAR(200),
    Description NVARCHAR(1000),
    ReleaseDate DATE,
    Price DECIMAL(10,2),

    DeveloperID UNIQUEIDENTIFIER,
    PublisherID UNIQUEIDENTIFIER,

    DeletedAt DATETIME2,

    FOREIGN KEY (DeveloperID) REFERENCES Developer(DeveloperID),
    FOREIGN KEY (PublisherID) REFERENCES Publishers(PublisherID)
);

-- TABLA LOGROS:
CREATE TABLE Achievements (
    AchievementID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(200),
    Description NVARCHAR(500),
    GameID UNIQUEIDENTIFIER,

    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA LOGROS DE USUARIO:
CREATE TABLE UserAchievements (
    UserAchievementID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    AchievementID INT,
    UnlockedAt DATETIME2,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (AchievementID) REFERENCES Achievements(AchievementID)
);

--TABLA DE RESEÑA
CREATE TABLE Reviews (
    ReviewID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    IsRecommended BIT,
    Comment NVARCHAR(1000),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE RESPUESTAS DE RESEÑAS
CREATE TABLE ReviewAnswers (
	ReviewAnswersId INT NOT NULL IDENTITY(1,1),
    ReviewId INT,
    UserID UNIQUEIDENTIFIER,
	Comment NVARCHAR(1000),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,

    PRIMARY KEY (ReviewId, UserID),

    FOREIGN KEY (ReviewId) REFERENCES Reviews(ReviewID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- TABLA DE LIBRERIA
CREATE TABLE Library (
    LibraryID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    PurchasePrice DECIMAL(10,2),
    PurchaseDate DATETIME2,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA LISTA DE DESEOS
CREATE TABLE Wishlist (
    WishlistID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    AddedAt DATETIME2 DEFAULT SYSUTCDATETIME(),

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE SESIONES

CREATE TABLE Sessions (
    SessionID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    StartTime DATETIME2,
    EndTime DATETIME2,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE DLCs
CREATE TABLE DLCs (
    DLCID INT IDENTITY(1,1) PRIMARY KEY,
    DLCName NVARCHAR(200),
    Price DECIMAL(10,2),
    AddedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    GameID UNIQUEIDENTIFIER,

    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE GENEROS
CREATE TABLE Genre (
    GenreID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100)
);

--TABLA DE GENERO DE JUEGOS:
CREATE TABLE Game_Genre (
    GameGenreID INT IDENTITY(1,1) PRIMARY KEY,
    GameID UNIQUEIDENTIFIER,
    GenreID INT,

    FOREIGN KEY (GameID) REFERENCES Games(GameID),
    FOREIGN KEY (GenreID) REFERENCES Genre(GenreID)
);

-- TABLA DE OFERTAS:
CREATE TABLE Offers (
    OfferID INT IDENTITY(1,1) PRIMARY KEY,
    GameID UNIQUEIDENTIFIER,
    Discount INT,
    StartDate DATETIME2,
    EndDate DATETIME2,

    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Status' AND xtype='U')
BEGIN
    CREATE TABLE Status (
        StatusId INT NOT NULL PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL
    );
END


--ACTUALIZACION 
USE [STEAM];
GO

SET NOCOUNT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    -------------------------------------------------------------------------
    -- 0. LOCALIZAR IDS DUPLICADOS DE ESTADOS EQUIVALENTES
    -------------------------------------------------------------------------
    DECLARE @ActiveDuplicateIds TABLE (StatusID INT PRIMARY KEY);
    DECLARE @InactiveDuplicateIds TABLE (StatusID INT PRIMARY KEY);

    INSERT INTO @ActiveDuplicateIds (StatusID)
    SELECT StatusID
    FROM dbo.Status
    WHERE StatusID <> 1
      AND (
            UPPER(ISNULL(Code, '')) = 'ACTIVE'
         OR UPPER(ISNULL(ShowName, '')) IN ('ACTIVO', 'ACTIVE')
      );

    INSERT INTO @InactiveDuplicateIds (StatusID)
    SELECT StatusID
    FROM dbo.Status
    WHERE StatusID <> 0
      AND (
            UPPER(ISNULL(Code, '')) = 'INACTIVE'
         OR UPPER(ISNULL(ShowName, '')) IN ('DESACTIVO', 'INACTIVE')
      );

    -------------------------------------------------------------------------
    -- 1. ASEGURAR STATUS BASE 0 Y 1
    -------------------------------------------------------------------------
    SET IDENTITY_INSERT dbo.Status ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Status WHERE StatusID = 0)
    BEGIN
        INSERT INTO dbo.Status (StatusID, Code, ShowName)
        VALUES (0, 'INACTIVE', 'Desactivo');
    END
    ELSE
    BEGIN
        UPDATE dbo.Status
        SET Code = 'INACTIVE',
            ShowName = 'Desactivo'
        WHERE StatusID = 0;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Status WHERE StatusID = 1)
    BEGIN
        INSERT INTO dbo.Status (StatusID, Code, ShowName)
        VALUES (1, 'ACTIVE', 'Activo');
    END
    ELSE
    BEGIN
        UPDATE dbo.Status
        SET Code = 'ACTIVE',
            ShowName = 'Activo'
        WHERE StatusID = 1;
    END

    SET IDENTITY_INSERT dbo.Status OFF;

    -------------------------------------------------------------------------
    -- 2. REASIGNAR USUARIOS QUE APUNTAN A IDS DUPLICADOS
    -------------------------------------------------------------------------
    UPDATE U
    SET U.StatusID = 1
    FROM dbo.Users U
    INNER JOIN @ActiveDuplicateIds D ON U.StatusID = D.StatusID;

    UPDATE U
    SET U.StatusID = 0
    FROM dbo.Users U
    INNER JOIN @InactiveDuplicateIds D ON U.StatusID = D.StatusID;

    -------------------------------------------------------------------------
    -- 3. NORMALIZAR USUARIOS SEGUN DeletedAt
    -------------------------------------------------------------------------
    UPDATE dbo.Users
    SET StatusID = 1
    WHERE DeletedAt IS NULL
      AND (StatusID IS NULL OR StatusID <> 1);

    UPDATE dbo.Users
    SET StatusID = 0
    WHERE DeletedAt IS NOT NULL
      AND (StatusID IS NULL OR StatusID <> 0);

    -------------------------------------------------------------------------
    -- 4. ELIMINAR ESTADOS DUPLICADOS YA SIN REFERENCIAS
    -------------------------------------------------------------------------
    DELETE S
    FROM dbo.Status S
    INNER JOIN @ActiveDuplicateIds D ON S.StatusID = D.StatusID
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Users U
        WHERE U.StatusID = S.StatusID
    );

    DELETE S
    FROM dbo.Status S
    INNER JOIN @InactiveDuplicateIds D ON S.StatusID = D.StatusID
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Users U
        WHERE U.StatusID = S.StatusID
    );

    -------------------------------------------------------------------------
    -- 5. ACTUALIZAR/CREAR EMAIL TEMPLATES
    -------------------------------------------------------------------------
    IF EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'COLLABORATOR_REGISTER')
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Name = 'USER_REGISTER',
            Subject = 'Bienvenido a Steam Clone',
            Body = 'Tu cuenta fue creada correctamente.'
        WHERE Name = 'COLLABORATOR_REGISTER';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'USER_REGISTER')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('USER_REGISTER', 'Bienvenido a Steam Clone', 'Tu cuenta fue creada correctamente.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Bienvenido a Steam Clone',
            Body = 'Tu cuenta fue creada correctamente.'
        WHERE Name = 'USER_REGISTER';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_LOGIN_SUCCESS')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_LOGIN_SUCCESS', 'Inicio de sesion correcto', 'Tu inicio de sesion fue exitoso el {{datetime}}.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Inicio de sesion correcto',
            Body = 'Tu inicio de sesion fue exitoso el {{datetime}}.'
        WHERE Name = 'AUTH_LOGIN_SUCCESS';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_LOGIN_FAILED')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_LOGIN_FAILED', 'Intento de inicio de sesion fallido', 'Se detecto un intento fallido de inicio de sesion en tu cuenta.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Intento de inicio de sesion fallido',
            Body = 'Se detecto un intento fallido de inicio de sesion en tu cuenta.'
        WHERE Name = 'AUTH_LOGIN_FAILED';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_REGISTER_INIT')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_REGISTER_INIT', 'Token de registro', 'Usa este token para completar tu registro: {{token}}');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Token de registro',
            Body = 'Usa este token para completar tu registro: {{token}}'
        WHERE Name = 'AUTH_REGISTER_INIT';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_RECOVER_PASSWORD_OTP')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_RECOVER_PASSWORD_OTP', 'Codigo de recuperacion', 'Tu codigo OTP para recuperar la contrasena es: {{code}}');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Codigo de recuperacion',
            Body = 'Tu codigo OTP para recuperar la contrasena es: {{code}}'
        WHERE Name = 'AUTH_RECOVER_PASSWORD_OTP';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_PASSWORD_CHANGED')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_PASSWORD_CHANGED', 'Contrasena actualizada', 'Tu contrasena fue actualizada correctamente.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Contrasena actualizada',
            Body = 'Tu contrasena fue actualizada correctamente.'
        WHERE Name = 'AUTH_PASSWORD_CHANGED';
    END

    COMMIT TRANSACTION;
    PRINT 'Actualizacion completada correctamente.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    BEGIN TRY
        SET IDENTITY_INSERT dbo.Status OFF;
    END TRY
    BEGIN CATCH
    END CATCH

    --THROW;
END CATCH;
GO

SELECT StatusID, Code, ShowName
FROM dbo.Status
ORDER BY StatusID;

SELECT UserId, Email, UserName, StatusID, DeletedAt
FROM dbo.Users
ORDER BY Email;

SELECT EmailTemplateId, Name, Subject, Body, CreatedAt
FROM dbo.EmailTemplates
ORDER BY Name;
GO

