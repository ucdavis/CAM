CREATE TABLE [dbo].[RequestTemplatesXNetworkShares]
(
	[RequestTemplateId] INT NOT NULL , 
    [NetworkShareId] INT NOT NULL, 
    PRIMARY KEY ([NetworkShareId], [RequestTemplateId]), 
    CONSTRAINT [FK_RequestTemplatesXNetworkShares_RequestTemplates] FOREIGN KEY ([RequestTemplateId]) REFERENCES [RequestTemplates]([Id]), 
    CONSTRAINT [FK_RequestTemplatesXNetworkShares_NetworkShares] FOREIGN KEY ([NetworkShareId]) REFERENCES [NetworkShares]([Id])
)
