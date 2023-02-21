CREATE OR ALTER PROCEDURE SP_CREATE_MESSAGE
(@GROUP_ID INT, @USER_ID INT , @CONTENT NVARCHAR(350) = '', @FILE_ID INT = '', @ERROR_MESSAGE NVARCHAR(150) OUTPUT)
AS
	--If the message isn't empty
	IF(@CONTENT = '' AND @FILE_ID = '') BEGIN
		SET @ERROR_MESSAGE = 'Empty message';
		RETURN @ERROR_MESSAGE;
	END
	--Get new message ID
	DECLARE @MESSAGE_ID INT;
	SELECT @MESSAGE_ID = (MAX([MESSAGE].MESSAGE_ID) + 1) FROM [MESSAGE];
	--Insert the message
	INSERT INTO [MESSAGE] VALUES (@MESSAGE_ID, @GROUP_ID, @CONTENT, @USER_ID, GETDATE(), @FILE_ID);
GO