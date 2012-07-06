CREATE TABLE [dbo].[RequestTemplatesXSecurityGroups]
(
	RequestTemplateId int not null,
	SecurityGroupId	int not null, 
    CONSTRAINT [PK_RequestTemplatesXSecurityGroups] PRIMARY KEY ([RequestTemplateId], [SecurityGroupId]), 
    CONSTRAINT [FK_RequestTemplatesXSecurityGroups_RequestTemplates] FOREIGN KEY ([RequestTemplateId]) REFERENCES [RequestTemplates]([Id]), 
    CONSTRAINT [FK_RequestTemplatesXSecurityGroups_SecurityGroups] FOREIGN KEY ([SecurityGroupId]) REFERENCES [SecurityGroups]([Id])
)
