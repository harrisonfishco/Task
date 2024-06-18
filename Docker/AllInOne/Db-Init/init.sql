CREATE DATABASE [Demo];
Go

CREATE LOGIN task WITH PASSWORD = 'sdfgsde!@ASDAS';
Go

USE [Demo];
Go

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'task')
BEGIN
    CREATE USER [task] FOR LOGIN [task]
    EXEC sp_addrolemember N'db_owner', N'task'
END;