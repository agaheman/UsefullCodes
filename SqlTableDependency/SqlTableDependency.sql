﻿CREATE TABLE Persons
(
    Id int  IDENTITY(1,1) NOT NULL PRIMARY KEY ,
    FirstName nvarchar(27) NOT NULL,
    LastName nvarchar(27) NOT NULL
);

ALTER DATABASE [Agah.TestDB] SET ENABLE_BROKER