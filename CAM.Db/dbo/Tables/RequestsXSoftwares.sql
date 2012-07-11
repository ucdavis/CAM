CREATE TABLE [dbo].[RequestsXSoftware]
(
	[RequestId] INT NOT NULL , 
    [SoftwareId] INT NOT NULL, 
    PRIMARY KEY ([SoftwareId], [RequestId]), 
    CONSTRAINT [FK_RequestXSoftware_Requests] FOREIGN KEY ([RequestId]) REFERENCES [Requests]([Id]), 
    CONSTRAINT [FK_RequestXSoftware_Software] FOREIGN KEY ([SoftwareId]) REFERENCES [Software]([Id])
)
