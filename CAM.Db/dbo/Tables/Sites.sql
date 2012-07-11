﻿CREATE TABLE [dbo].[Sites] (
    [Id]   VARCHAR (5)  NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    [ActiveDirectoryServer] VARCHAR(100) NULL, 
    [SecurityGroupOu] VARCHAR(MAX) NULL, 
    [UserOu] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED ([Id] ASC)
);

