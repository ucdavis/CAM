CREATE TABLE [dbo].[OrganizationalUnits]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Path] VARCHAR(MAX) NULL, 
    [SiteId] VARCHAR(5) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_OrganizationalUnits_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
)
