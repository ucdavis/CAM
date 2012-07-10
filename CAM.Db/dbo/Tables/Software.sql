CREATE TABLE [dbo].[Software]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [SiteId] VARCHAR(5) NOT NULL, 
    [WebApplication] BIT NOT NULL DEFAULT 0, 
    [WebRoleManager] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Software_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
)
