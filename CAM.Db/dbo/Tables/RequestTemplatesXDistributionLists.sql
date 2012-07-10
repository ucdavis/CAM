CREATE TABLE [dbo].[RequestTemplatesXDistributionLists]
(
	[RequestTemplateId] INT NOT NULL , 
    [DistributionListId] INT NOT NULL, 
    PRIMARY KEY ([DistributionListId], [RequestTemplateId]), 
    CONSTRAINT [FK_RequestTemplatesXDistributionLists_RequestTemplates] FOREIGN KEY ([RequestTemplateId]) REFERENCES [RequestTemplates]([Id]), 
)