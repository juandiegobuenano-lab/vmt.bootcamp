-- =========================================
-- CREAR BASE DE DATOS
-- =========================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Prueba')
    CREATE DATABASE Prueba;
GO

USE Prueba;
GO


-- =========================================
-- TABLA ROLES
-- =========================================
CREATE TABLE Roles(
    RoleID INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
    Code NVARCHAR(10) NOT NULL,
    ShowName NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO


-- =========================================
-- TABLA TIPOS DE ESTADO DE USUARIO
-- =========================================
CREATE TABLE UserStatusType(
    UserStatusTypeID INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
    Code NVARCHAR(20) NOT NULL,
    ShowName NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO


-- =========================================
-- INSERTAR ESTADOS
-- =========================================
INSERT INTO UserStatusType(Code, ShowName)
VALUES 
('ONLINE',        'EN LINEA'),
('NOT_DISTURB',   'NO MOLESTAR'),
('IDLE',          'AUSENTE'),
('GHOST',         'INVISIBLE');
GO


-- =========================================
-- TABLA USUARIOS
-- =========================================
CREATE TABLE Usuario(
    UserId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(32) NOT NULL,
    DisplayName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(200) NULL,

    StatusType INT NOT NULL DEFAULT 1,
    StatusContent NVARCHAR(50) NULL DEFAULT ('HEY!'),

    AvatarURL NVARCHAR(255) NULL,
    BannerURL NVARCHAR(255) NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Usuario_StatusType
        FOREIGN KEY (StatusType)
        REFERENCES UserStatusType(UserStatusTypeID)
);
GO


-- =========================================
-- TABLA COLLECTIONS
-- =========================================
CREATE TABLE Collections(
    CollectionID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(100) NOT NULL DEFAULT('THIS IS MY COLLECTION!'),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    DeleteAt DATETIME2 NULL
);
GO


-- =========================================
-- TABLA ITEMS
-- =========================================
CREATE TABLE Items(
    ItemID INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO


-- =========================================
-- INSERTAR ITEMS
-- =========================================
INSERT INTO Items(Name)
VALUES
('GTA VI'),
('REQUIEM');
GO


-- =========================================
-- TABLA INTERMEDIA COLLECTIONS - ITEMS
-- RELACION MUCHOS A MUCHOS
-- =========================================
CREATE TABLE CollectionsItems(
    CollectionID UNIQUEIDENTIFIER NOT NULL,
    ItemID INT NOT NULL,

    CONSTRAINT PK_CollectionsItems 
        PRIMARY KEY(CollectionID, ItemID),

    CONSTRAINT FK_CollectionsItems_Collection
        FOREIGN KEY (CollectionID)
        REFERENCES Collections(CollectionID),

    CONSTRAINT FK_CollectionsItems_Item
        FOREIGN KEY (ItemID)
        REFERENCES Items(ItemID)
        ON DELETE CASCADE
);
GO


-- =========================================
-- INSERTAR COLECCION
-- =========================================
INSERT INTO Collections(Name, Description)
VALUES
('MY GAMES','GAMES');
GO


-- =========================================
-- CONSULTAS
-- =========================================
SELECT * 
FROM Collections
WHERE DeleteAt IS NULL;

SELECT * FROM Items;
GO


-- =========================================
-- VARIABLES PARA INSERTAR RELACIONES
-- =========================================
DECLARE @CollectionID UNIQUEIDENTIFIER = (SELECT TOP 1 CollectionID FROM Collections);

DECLARE @ItemGTAVIId INT = (SELECT ItemID FROM Items WHERE Name = 'GTA VI');
DECLARE @ItemRequiemId INT = (SELECT ItemID FROM Items WHERE Name = 'REQUIEM');

INSERT INTO CollectionsItems (CollectionID, ItemID)
VALUES
(@CollectionID, @ItemGTAVIId),
(@CollectionID, @ItemRequiemId);
GO


-- =========================================
-- BUSQUEDAS
-- =========================================
SELECT * 
FROM Items
WHERE Name = 'GTA VI' OR Name = 'REQUIEM';

DECLARE @ItemBuscar NVARCHAR(50)='GTA VI';

SELECT * 
FROM Items
WHERE Name = @ItemBuscar;
GO


-- =========================================
-- INDICE PARA BUSQUEDA RAPIDA
-- =========================================
CREATE INDEX IX_Items_Name 
ON Items (Name);
GO


-- =========================================
-- TABLA RELACION USERS - ROLES
-- =========================================
CREATE TABLE UsersRoles(
    UserId UNIQUEIDENTIFIER NOT NULL,
    RoleId INT NOT NULL,

    CONSTRAINT PK_UsersRoles 
        PRIMARY KEY(UserId, RoleId),

    CONSTRAINT FK_UsersRoles_User
        FOREIGN KEY (UserId)
        REFERENCES Usuario(UserId),

    CONSTRAINT FK_UsersRoles_Role
        FOREIGN KEY (RoleId)
        REFERENCES Roles(RoleId)
);
GO









