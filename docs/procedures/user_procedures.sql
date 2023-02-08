CREATE OR ALTER PROCEDURE SP_CREATE_USER
@EMAIL NVARCHAR(255),
@PASSWORD NVARCHAR(12),
@USERNAME NVARCHAR(30),
@FIRST_NAME NVARCHAR(30),
@LAST_NAME NVARCHAR(30),
@IMAGE NVARCHAR(MAX) = '',
@ROLE NVARCHAR(10) = 'USER',
@RESPONSE_MESSAGE NVARCHAR(250) OUTPUT
AS
    SET NOCOUNT ON

	DECLARE @salt UNIQUEIDENTIFIER=NEWID()
	BEGIN TRY

		DECLARE @NEW_ID INT;
		SELECT @NEW_ID = (MAX([USER].[USER_ID]) + 1) FROM [USER];

		DECLARE @EXISTING_USERS INT;
		SELECT @EXISTING_USERS = COUNT([USER].[USER_ID]) FROM [USER] WHERE [EMAIL] = @EMAIL AND [USERNAME] = @USERNAME;

		IF(@EXISTING_USERS = 0) BEGIN
			INSERT INTO [USER]
			VALUES(@NEW_ID, @EMAIL, HASHBYTES('SHA2_512', @PASSWORD+CAST(@salt AS NVARCHAR(36))), @salt, @USERNAME, @FIRST_NAME, @LAST_NAME, GETDATE(), @IMAGE, @ROLE)

			SET @RESPONSE_MESSAGE='Success'
		END

	END TRY
	BEGIN CATCH
		SET @RESPONSE_MESSAGE=ERROR_MESSAGE() 
	END CATCH
GO

CREATE OR ALTER PROCEDURE SP_USER_LOGIN
    @EMAIL NVARCHAR(254),
    @USERNAME NVARCHAR(50),
	@PASSWORD NVARCHAR(12),
    @RESPONSE_MESSAGE NVARCHAR(250)='' OUTPUT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @userID INT

    IF EXISTS (SELECT TOP 1 [USER_ID] FROM [USER] WHERE USERNAME=@USERNAME OR EMAIL=@EMAIL)
    BEGIN
        SET @userID=(SELECT [USER_ID] FROM [USER] WHERE (USERNAME=@USERNAME OR EMAIL=@EMAIL) AND [PASSWORD]=HASHBYTES('SHA2_512', @PASSWORD+CAST([SALT] AS NVARCHAR(36))))

       IF(@userID IS NULL)
           SET @RESPONSE_MESSAGE='Incorrect password'
       ELSE 
           SET @RESPONSE_MESSAGE='User successfully logged in'
    END
    ELSE
       SET @RESPONSE_MESSAGE='Invalid login'
	END
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_USER_USERNAME
(@EMAIL NVARCHAR(255), @USERNAME NVARCHAR(30), @NEW_USERNAME NVARCHAR(30))
AS
	UPDATE [USER] SET [USERNAME] = @NEW_USERNAME
	WHERE [USERNAME] = @USERNAME AND [EMAIL] = @EMAIL;
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_USER_PASSWORD
(@EMAIL NVARCHAR(255), @USERNAME NVARCHAR(30), @PASSWORD NVARCHAR(12))
AS
	UPDATE [USER] SET [PASSWORD] = @PASSWORD
	WHERE [USERNAME] = @USERNAME AND [EMAIL] = @EMAIL;
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_USER_LASTSEEN
(@EMAIL NVARCHAR(255) = '', @USERNAME NVARCHAR(30) = '', @LAST_SEEN DATETIME)
AS
	UPDATE [USER] SET [LAST_SEEN] = @LAST_SEEN
	WHERE [USERNAME] = @USERNAME OR [EMAIL] = @EMAIL;
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_USER_EMAIL
(@EMAIL NVARCHAR(255), @USERNAME NVARCHAR(30), @NEW_EMAIL NVARCHAR(255))
AS
	UPDATE [USER] SET [EMAIL] = @NEW_EMAIL
	WHERE [USERNAME] = @USERNAME AND [EMAIL] = @EMAIL;
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_USER_IMAGE
(@EMAIL NVARCHAR(255), @IMAGE NVARCHAR(MAX))
AS
	UPDATE [USER] SET [IMAGE] = @IMAGE
	WHERE [EMAIL] = @EMAIL;
GO

CREATE OR ALTER PROCEDURE SP_DELETE_USER
(@EMAIL NVARCHAR(255), @USERNAME NVARCHAR(30))
AS
	DELETE FROM [USER] WHERE EMAIL = @EMAIL AND USERNAME = @USERNAME;
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_USER_ROLE
(@EMAIL NVARCHAR(255), @USERNAME NVARCHAR(30), @ROLE NVARCHAR(10))
AS
	IF(@ROLE = 'ADMIN' OR @ROLE = 'USER') BEGIN
		UPDATE [USER] SET [ROLE] = @ROLE
		WHERE [USERNAME] = @USERNAME AND [EMAIL] = @EMAIL;
		
		RETURN 1;
	END
	ELSE BEGIN
		RETURN 0;
	END
GO