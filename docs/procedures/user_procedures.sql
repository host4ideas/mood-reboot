CREATE OR ALTER PROCEDURE SP_CREATE_USER
@EMAIL NVARCHAR(255),
@PASSWORD NVARCHAR(12),
@USERNAME NVARCHAR(30),
@FIRST_NAME NVARCHAR(30),
@LAST_NAME NVARCHAR(30),
@IMAGE NVARCHAR(MAX) = NULL,
@ROLE NVARCHAR(10) = 'USER',
@USERID INT OUTPUT,
@RESPONSE_MESSAGE NVARCHAR(250) OUTPUT
AS
    SET NOCOUNT ON

	DECLARE @salt UNIQUEIDENTIFIER=NEWID()
	BEGIN TRY

		DECLARE @NEW_ID INT;
		SELECT @NEW_ID = (MAX([USER].[USER_ID]) + 1) FROM [USER];

		--In case is the first user
		IF(@NEW_ID IS NULL) BEGIN
			SET @NEW_ID = 0
		END

		SET @USERID = @NEW_ID;

		IF(@ROLE IS NULL) BEGIN
			SET @ROLE = 'USER';
		END

		DECLARE @EXISTING_USERS INT;
		SELECT @EXISTING_USERS = COUNT([USER].[USER_ID]) FROM [USER] WHERE [EMAIL] = @EMAIL AND [USERNAME] = @USERNAME;

		--Insert if the user doesn't exist
		IF(@EXISTING_USERS = 0) BEGIN
			DECLARE @HASHPASSWORD VARBINARY(MAX);

			SET @HASHPASSWORD = HASHBYTES('SHA2_512', @PASSWORD) + CAST(@salt AS VARBINARY(MAX));
			
			INSERT INTO [USER]
			VALUES(@NEW_ID, @EMAIL, @HASHPASSWORD, @USERNAME, @FIRST_NAME, @LAST_NAME, GETDATE(), @IMAGE, @ROLE, null, CAST(@salt AS NVARCHAR(MAX)), @PASSWORD);

			SET @RESPONSE_MESSAGE='Success'
		END

	END TRY
	BEGIN CATCH
		SET @RESPONSE_MESSAGE=ERROR_MESSAGE() 
	END CATCH
GO

CREATE OR ALTER PROCEDURE SP_USER_LOGIN
    @EMAIL NVARCHAR(254) = NULL,
    @USERNAME NVARCHAR(50) = NULL,
	@PASSWORD NVARCHAR(12),
    @RESPONSE_MESSAGE NVARCHAR(250) OUTPUT
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
