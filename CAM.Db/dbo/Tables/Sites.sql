CREATE TABLE [dbo].[Sites] (
    [Id]   VARCHAR (5)  NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    [ActiveDirectoryServer] VARCHAR(100) NULL, 
    [SecurityGroupOu] VARCHAR(MAX) NULL, 
    [UserOu] VARCHAR(MAX) NULL, 
    [Username] VARCHAR(50) NULL, 
    [Password] VARCHAR(50) NULL, 
    [LyncUri] VARCHAR(MAX) NULL, 
    [ExchangeUri] VARCHAR(MAX) NULL, 
    [ExchangeDatabases] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED ([Id] ASC)
);

