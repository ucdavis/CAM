CREATE TABLE [dbo].[NetworkShares]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(100) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [SiteId] VARCHAR(5) NOT NULL, 
    [GroupId] VARCHAR(50) NULL, 
    [ForceSelect] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_NetworkShares_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
)
