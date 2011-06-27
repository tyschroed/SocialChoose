IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'User'))
BEGIN
    drop table [User]
END

create table [User] (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CookieId varchar(50) NULL,
	UserName varchar(50) NULL,
	Name varchar(200) NULL,
	FacebookToken varchar(500) NULL,
	FacebookExpiresDateTime datetime NULL,
	FacebookId decimal(10,0) NULL,
	PasswordHash varchar(50) NULL,
	PasswordSalt varchar(50) NULL,
	ModifiedDate datetime2(7) NOT NULL default (getdate())
)