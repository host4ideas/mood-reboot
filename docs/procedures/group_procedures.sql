CREATE OR ALTER PROCEDURE SP_CREATE_GROUP
(@NAME NVARCHAR(45), @IMAGE NVARCHAR(MAX) = '')
AS
	DECLARE @GROUP_ID INT;
	SELECT @GROUP_ID = (MAX([GROUP].GROUP_ID) + 1) FROM [GROUP];
	INSERT INTO [GROUP] VALUES (@GROUP_ID, @IMAGE, @NAME);
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_GROUP
(@GROUP_ID INT, @NAME NVARCHAR(45), @IMAGE NVARCHAR(MAX) = '')
AS
	UPDATE [GROUP] SET NAME = @NAME, IMAGE = @IMAGE;
GO

CREATE OR ALTER PROCEDURE SP_DELETE_GROUP
(@GROUP_ID INT)
AS
	DELETE FROM [GROUP] WHERE GROUP_ID = @GROUP_ID;
GO
