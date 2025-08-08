CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Forename] nvarchar(255) NOT NULL,
    [Surname] nvarchar(255) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [DateofBirth] DATE NOT NULL,
    [IsActive] bit NOT NULL
)
