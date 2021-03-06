﻿CREATE TABLE [dbo].[Requests]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [PositionTitle] VARCHAR(100) NOT NULL, 
    [DepartmentName] VARCHAR(100) NOT NULL, 
    [UnitName] VARCHAR(100) NULL, 
    [OfficeLocation] VARCHAR(100) NOT NULL, 
    [Room] VARCHAR(20) NULL, 
    [ContactPhone] VARCHAR(50) NULL, 
	[OfficePhone] VARCHAR(50) NULL,
    [Start] DATE NOT NULL, 
    [End] DATE NULL, 
    [HireType] VARCHAR(50) NULL, 
    [HardwareType] VARCHAR(50) NULL, 
    [EmployeeType] VARCHAR(50) NULL, 
    [NeedsEmail] BIT NOT NULL DEFAULT  0, 
    [AdditionalFolders] VARCHAR(100) NULL, 
    [UnitId] INT NULL, 
    [SiteId] VARCHAR(5) NOT NULL, 
    [Pending] BIT NOT NULL DEFAULT 1, 
    [Approved] BIT NULL, 
    [CreatedBy] VARCHAR(15) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
	[Manager] varchar(15),
    [OrganizationalUnitId] INT NULL, 
	[HomeDirectory] VARCHAR(100) NULL, 
    [HomeDrive] VARCHAR(5) NULL, 
    CONSTRAINT [FK_Requests_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id]), 
    CONSTRAINT [FK_Requests_Units] FOREIGN KEY ([UnitId]) REFERENCES [Units]([Id]), 
    CONSTRAINT [FK_Requests_OrganizationalUnits] FOREIGN KEY ([OrganizationalUnitId]) REFERENCES [OrganizationalUnits]([Id]) 
)
