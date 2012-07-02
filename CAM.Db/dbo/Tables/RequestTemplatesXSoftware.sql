CREATE TABLE [dbo].[RequestTemplatesXSoftware]
(
	[RequestTemplateId] INT NOT NULL , 
    [SoftwareId] INT NOT NULL, 
    PRIMARY KEY ([SoftwareId], [RequestTemplateId]), 
    CONSTRAINT [FK_RequestTemplatesXSoftware_RequestTemplates] FOREIGN KEY ([RequestTemplateId]) REFERENCES [RequestTemplates]([Id]), 
    CONSTRAINT [FK_RequestTemplatesXSoftware_Software] FOREIGN KEY ([SoftwareId]) REFERENCES [Software]([Id])
)
