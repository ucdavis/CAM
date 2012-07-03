CREATE TABLE [dbo].[IpAddresses]
(
	[Id] CHAR(12) NOT NULL PRIMARY KEY, 
    [Host] VARCHAR(100) NULL, 
    [RangeId] CHAR(9) NOT NULL, 
    [SortOrder] INT NOT NULL
)
