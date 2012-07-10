CREATE TABLE [dbo].[RequestTemplatesXAvailableSecurityGroups]
(
	RequestTemplateId int not null,
	SecurityGroupId	int not null, 
    CONSTRAINT [PK_RequestTemplatesXAvailableSecurityGroups] PRIMARY KEY ([RequestTemplateId], [SecurityGroupId]), 
    CONSTRAINT [FK_RequestTemplatesXAvailableSecurityGroups_RequestTemplates] FOREIGN KEY ([RequestTemplateId]) REFERENCES [RequestTemplates]([Id]), 
    CONSTRAINT [FK_RequestTemplatesXAvailableSecurityGroups_SecurityGroups] FOREIGN KEY ([SecurityGroupId]) REFERENCES [SecurityGroups]([Id])
)
