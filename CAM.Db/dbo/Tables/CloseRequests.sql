CREATE TABLE [dbo].[CloseRequests]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LoginId] VARCHAR(50) NOT NULL, 
    [DateRequested] DATETIME NOT NULL DEFAULT getdate(), 
    [RequestedBy] VARCHAR(50) NOT NULL, 
    [IsPending] BIT NOT NULL DEFAULT 1
)
