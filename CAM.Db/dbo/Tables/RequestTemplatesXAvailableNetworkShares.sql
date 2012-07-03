CREATE TABLE [dbo].[RequestTemplatesXAvailableNetworkShares]
(
	[RequestTemplateId] INT NOT NULL , 
    [NetworkShareId] INT NOT NULL, 
    PRIMARY KEY ([NetworkShareId], [RequestTemplateId]), 
    CONSTRAINT [FK_RequestTemplatesXAvailableNetworkShares_RequestTemplates] FOREIGN KEY ([RequestTemplateId]) REFERENCES [RequestTemplates]([Id]), 
    CONSTRAINT [FK_RequestTemplatesXAvailableNetworkShares_NetworkShares] FOREIGN KEY ([NetworkShareId]) REFERENCES [NetworkShares]([Id])
)
