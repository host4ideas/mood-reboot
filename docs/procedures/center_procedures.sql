CREATE OR ALTER PROCEDURE SP_CREATE_CENTER
(@EMAIL NVARCHAR(255), @NAME NVARCHAR(50), @ADDRESS NVARCHAR(150), @TELEPHONE NVARCHAR(15), @IMAGE NVARCHAR(MAX) = NULL)
AS
	--Generate new ID
	DECLARE @NEWID INT;
	SELECT @NEWID = MAX(CENTER_ID) + 1 FROM CENTER;
	--Insert new center
	INSERT INTO CENTER VALUES(@NEWID, @EMAIL, @NAME, @ADDRESS, @IMAGE, @TELEPHONE);
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_CENTER
(@CENTER_ID INT, @EMAIL NVARCHAR(255), @NAME NVARCHAR(50), @ADDRESS NVARCHAR(150), @IMAGE NVARCHAR(MAX) = NULL, @TELEPHONE NVARCHAR(15))
AS
	UPDATE CENTER SET EMAIL = @EMAIL, NAME = @NAME, ADDRESS = @ADDRESS, IMAGE = @IMAGE, TELEPHONE = @TELEPHONE
	WHERE CENTER_ID = @CENTER_ID;
GO

CREATE OR ALTER PROCEDURE SP_DELETE_CENTER
(@CENTER_ID INT)
AS
	DELETE FROM CENTER WHERE CENTER_ID = @CENTER_ID;
GO

CREATE OR ALTER PROCEDURE SP_USER_CENTERS
(@USER_ID INT)
AS
	SELECT  C.ADDRESS, C.CENTER_ID, C.EMAIL, C.IMAGE, C.NAME, C.NAME, C.TELEPHONE, UC.IS_EDITOR
	FROM CENTER C INNER JOIN USER_CENTER UC
	ON C.CENTER_ID = UC.CENTER_ID
	WHERE UC.USER_ID = @USER_ID;
GO