CREATE OR ALTER PROCEDURE SP_CREATE_GROUP_CONTENT
(@NAME NVARCHAR(50), @COURSE_ID INT, @IS_VISIBLE BIT = 0, @GROUPCONTENTID INT OUT)
AS
	DECLARE @NEW_ID INT;
	SELECT @NEW_ID = MAX(GROUP_CONTENT_ID) + 1 FROM CONTENT_COURSE;

	IF (@NEW_ID IS NULL) BEGIN
		SET @NEW_ID = 0;
	END

	INSERT INTO CONTENT_COURSE VALUES (@NEW_ID, @NAME, @IS_VISIBLE, @COURSE_ID);

	SET @GROUPCONTENTID = @NEW_ID;
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_GROUP_CONTENT
(@GROUP_CONTENT_ID INT, @NAME NVARCHAR(50), @IS_VISIBLE BIT)
AS
	UPDATE CONTENT_COURSE SET [NAME] = @NAME, IS_VISIBLE = @IS_VISIBLE WHERE GROUP_CONTENT_ID = @GROUP_CONTENT_ID;
GO

CREATE OR ALTER PROCEDURE SP_DELETE_GROUP_CONTENT
(@GROUP_CONTENT_ID INT)
AS
	DELETE FROM CONTENT_COURSE WHERE GROUP_CONTENT_ID = @GROUP_CONTENT_ID;
GO