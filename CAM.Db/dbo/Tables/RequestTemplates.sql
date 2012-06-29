CREATE TABLE [dbo].[RequestTemplates] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
    [SiteId] VARCHAR (5)  NULL,
	[UnitId] int not null,
    [NeedsEmail] BIT NOT NULL DEFAULT 1, 
    [DefaultSave] VARCHAR(50) NULL, 
    CONSTRAINT [PK_RequestTemplates] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_RequestTemplates_Sites] FOREIGN KEY (SiteId) REFERENCES [Sites]([Id]), 
    CONSTRAINT [FK_RequestTemplates_Units] FOREIGN KEY ([UnitId]) REFERENCES [Units]([Id])
);

