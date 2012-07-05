CREATE TABLE [dbo].[SecurityGroups]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [SiteId] VARCHAR(5) NOT NULL, 
    CONSTRAINT [FK_SecurityGroups_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
)
