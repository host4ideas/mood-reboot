CREATE OR ALTER PROCEDURE SP_CREATE_MESSAGE
(@GROUP_ID INT, @USER_ID INT , @CONTENT NVARCHAR(350) = NULL, @FILE_ID INT = NULL, @ERROR_MESSAGE NVARCHAR(150) OUTPUT)
AS
	DECLARE @USERNAME NVARCHAR(30);

	SELECT @USERNAME = USERNAME FROM [USER]
	WHERE USER_ID = @USER_ID;

	--If the message isn't empty
	IF(@CONTENT = NULL AND @FILE_ID = NULL) BEGIN
		SET @ERROR_MESSAGE = 'Empty message';
		RETURN @ERROR_MESSAGE;
	END
	--Get new message ID
	DECLARE @MESSAGE_ID INT;
	SELECT @MESSAGE_ID = (MAX([MESSAGE].MESSAGE_ID) + 1) FROM [MESSAGE];

	IF (@MESSAGE_ID IS NULL) BEGIN
		SET @MESSAGE_ID = 0;
	END

	--Insert the message
	INSERT INTO [MESSAGE] VALUES (@MESSAGE_ID, @GROUP_ID, @CONTENT, @USER_ID, GETDATE(), @FILE_ID, 0, @USERNAME);
GO