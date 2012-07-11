CREATE TABLE [dbo].[RequestsXSecurityGroups]
(
	RequestId int not null,
	SecurityGroupId	int not null, 
    CONSTRAINT [PK_RequestXSecurityGroups] PRIMARY KEY ([RequestId], [SecurityGroupId]), 
    CONSTRAINT [FK_RequestXSecurityGroups_Requests] FOREIGN KEY ([RequestId]) REFERENCES [Requests]([Id]), 
    CONSTRAINT [FK_RequestXSecurityGroups_SecurityGroups] FOREIGN KEY ([SecurityGroupId]) REFERENCES [SecurityGroups]([Id])
)
