CREATE OR ALTER PROCEDURE SP_CREATE_COURSE
(@FIRSTEDITORID INT, @CENTER_ID INT, @NAME NVARCHAR(30), @DESCRIPTION NVARCHAR(350) = NULL, @IMAGE NVARCHAR(MAX) = NULL, @IS_VISIBLE BIT = 0, @PASSWORD NVARCHAR(12) = NULL, @COURSEID INT OUTPUT, @GROUPID INT OUTPUT)
AS
	-- Create a group for discussion
	DECLARE @GROUP_ID INT;
	SELECT @GROUP_ID = (MAX([GROUP].GROUP_ID) + 1) FROM [GROUP];

	IF (@GROUP_ID IS NULL) BEGIN
		SET @GROUP_ID = 0;
	END
	INSERT INTO [GROUP] VALUES (@GROUP_ID, @IMAGE, @NAME);

	--Insert the course
	DECLARE @COURSE_ID INT;
	SELECT @COURSE_ID = (MAX([COURSE].[COURSE_ID]) + 1) FROM [COURSE];

	IF (@COURSE_ID IS NULL) BEGIN
		SET @COURSE_ID = 0;
	END

	INSERT INTO COURSE VALUES
	(@COURSE_ID, GETDATE(), GETDATE(), @DESCRIPTION, @IMAGE, @NAME, @CENTER_ID, @IS_VISIBLE, @GROUP_ID, @PASSWORD);

	SET @COURSEID = @COURSE_ID;
	SET @GROUPID = @GROUP_ID;
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_COURSE
(@COURSE_ID INT, @NAME NVARCHAR(30), @DESCRIPTION NVARCHAR(350) = NULL, @IMAGE NVARCHAR(MAX) = NULL)
AS
	UPDATE [COURSE] SET [NAME] = @NAME, [IMAGE] = @IMAGE, [DESCRIPTION] = @DESCRIPTION
	WHERE [COURSE].COURSE_ID = @COURSE_ID;
GO

CREATE OR ALTER PROCEDURE SP_DELETE_COURSE
(@COURSE_ID INT)
AS
	--Delete the course
	DELETE FROM COURSE WHERE COURSE_ID = @COURSE_ID;
GO

CREATE OR ALTER PROCEDURE SP_USER_COURSES
(@USER_ID INT)
AS
	SELECT C.COURSE_ID, C.DATE_PUBLISHED, C.DATE_MODIFIED, C.[DESCRIPTION], C.[IMAGE], C.[NAME], CT.[NAME], UC.IS_EDITOR, U.USERNAME
	FROM COURSE C INNER JOIN USER_COURSE UC
	ON C.COURSE_ID = UC.COURSE_ID
	INNER JOIN [USER] U
	ON UC.USER_ID = U.USER_ID
	INNER JOIN CENTER CT
	ON CT.CENTER_ID = C.CENTER_ID
	WHERE UC.[USER_ID] = @USER_ID;
GO

CREATE OR ALTER PROCEDURE SP_CENTER_COURSES
(@USER_ID INT, @CENTER_ID INT)
AS
	SELECT C.COURSE_ID, C.DATE_PUBLISHED, C.DATE_MODIFIED, C.[DESCRIPTION], C.[IMAGE], C.[NAME], CT.[NAME], UC.IS_EDITOR
	FROM COURSE C INNER JOIN CENTER CT
	ON C.CENTER_ID = CT.CENTER_ID
	INNER JOIN USER_COURSE UC
	ON UC.COURSE_ID = C.COURSE_ID
	WHERE C.CENTER_ID = @CENTER_ID;
GO

CREATE OR ALTER PROCEDURE SP_DELETE_COURSE
(@COURSE_ID INT)
AS
	DECLARE @GROUP_ID INT;
	SELECT @GROUP_ID = GROUP_ID FROM COURSE WHERE COURSE_ID = @COURSE_ID;
	DELETE FROM [GROUP] WHERE GROUP_ID = @GROUP_ID;
	DELETE FROM COURSE WHERE COURSE_ID = @COURSE_ID;
GO