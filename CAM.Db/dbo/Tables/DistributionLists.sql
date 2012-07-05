CREATE TABLE [dbo].[DistributionLists]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
	[Description] varchar(max),
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [SiteId] VARCHAR(5) NOT NULL, 
    [SID] VARCHAR(MAX) NULL, 
    CONSTRAINT [FK_DistributionLists_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
)