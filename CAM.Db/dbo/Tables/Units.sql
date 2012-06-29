CREATE TABLE [dbo].[Units]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SiteId] VARCHAR(5) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Units_Site] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
)
