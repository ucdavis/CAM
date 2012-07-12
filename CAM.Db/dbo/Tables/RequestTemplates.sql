CREATE TABLE [dbo].[RequestTemplates] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
	[Description] varchar(max),
    [SiteId] VARCHAR (5)  NULL,
	[UnitId] int not null,
    [NeedsEmail] BIT NOT NULL DEFAULT 1, 
    [AdditionalFolders] VARCHAR(100) NULL, 
    [HireType] VARCHAR(15) NULL, 
    [HardwareType] VARCHAR(15) NULL, 
    [EmployeeType] VARCHAR(15) NULL, 
    [DistributionLists] VARCHAR(MAX) NULL, 
	[OrganizationalUnitId] INT NULL, 
    CONSTRAINT [PK_RequestTemplates] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_RequestTemplates_Sites] FOREIGN KEY (SiteId) REFERENCES [Sites]([Id]), 
    CONSTRAINT [FK_RequestTemplates_Units] FOREIGN KEY ([UnitId]) REFERENCES [Units]([Id]),
	CONSTRAINT [FK_RequestTemplates_OrganizationalUnits] FOREIGN KEY ([OrganizationalUnitId]) REFERENCES [OrganizationalUnits]([Id]) 
);

