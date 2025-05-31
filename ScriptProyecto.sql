CREATE DATABASE MAPIsecurity;
GO

USE MAPIsecurity;
GO

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY,
    NombreUsuario NVARCHAR(50) UNIQUE NOT NULL,
    Contrasena NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Reportes (
    Id INT PRIMARY KEY IDENTITY,
    UsuarioId INT FOREIGN KEY REFERENCES Usuarios(Id),
    Tipo NVARCHAR(50),
    Descripcion NVARCHAR(500),
    Latitud FLOAT,
    Longitud FLOAT,
    Fecha DATETIME
);