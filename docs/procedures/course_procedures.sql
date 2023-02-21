CREATE OR ALTER PROCEDURE SP_CREATE_COURSE
(@NAME NVARCHAR(30), @DESCRIPTION NVARCHAR(350) = '', @IMAGE NVARCHAR(MAX) = '')
AS
	DECLARE @GROUP_ID INT;
	DECLARE @COURSE_ID INT;

	--Create a group for discussion
	SELECT @GROUP_ID = (MAX([GROUP].GROUP_ID) + 1) FROM [GROUP];

	INSERT INTO [GROUP] VALUES (@GROUP_ID, @IMAGE, @NAME);

	--Insert the course
	SELECT @COURSE_ID = (MAX([USER].[USER_ID]) + 1) FROM [USER];

	INSERT INTO COURSE VALUES
	(@COURSE_ID, GETDATE(), GETDATE(), @GROUP_ID, @DESCRIPTION, @IMAGE, @NAME);
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_COURSE
(@COURSE_ID INT, @NAME NVARCHAR(30), @DESCRIPTION NVARCHAR(350) = '', @IMAGE NVARCHAR(MAX) = '')
AS
	UPDATE [COURSE] SET [NAME] = @NAME, [IMAGE] = @IMAGE, [DESCRIPTION] = @DESCRIPTION
	WHERE [COURSE].COURSE_ID = @COURSE_ID;
GO

CREATE OR ALTER PROCEDURE SP_DELETE_COURSE
(@COURSE_ID INT)
AS
	--Get the associated discussion group id
	DECLARE @GROUP_ID INT;
	SELECT @GROUP_ID = [COURSE].GROUP_ID FROM COURSE WHERE COURSE_ID = @COURSE_ID;
	--Delete the group
	DELETE FROM [GROUP] WHERE GROUP_ID = @GROUP_ID;
	--Delete the course
	DELETE FROM COURSE WHERE COURSE_ID = @COURSE_ID;
GO