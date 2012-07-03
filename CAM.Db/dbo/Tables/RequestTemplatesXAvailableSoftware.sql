CREATE TABLE [dbo].[RequestTemplatesXAvailableSoftware]
(
	[RequestTemplateId] INT NOT NULL , 
    [SoftwareId] INT NOT NULL, 
    PRIMARY KEY ([SoftwareId], [RequestTemplateId]), 
    CONSTRAINT [FK_RequestTemplatesXAvailableSoftware_RequestTemplates] FOREIGN KEY ([RequestTemplateId]) REFERENCES [RequestTemplates]([Id]), 
    CONSTRAINT [FK_RequestTemplatesXAvailableSoftware_Software] FOREIGN KEY ([SoftwareId]) REFERENCES [Software]([Id])
)
