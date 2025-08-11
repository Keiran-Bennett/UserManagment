CREATE TABLE [dbo].[Logs]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT NOT NULL,
    [DateofAction] DATETIME NOT NULL,
    [Details] NVARCHAR(MAX) NOT NULL, 
    [Type] INT NOT NULL, 
    [Snapshot] NVARCHAR(MAX) NOT NULL 
)
