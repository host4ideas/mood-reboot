DECLARE @GROUPCONTENTID INT;
DECLARE @CENTERID INT;
DECLARE @COURSEID INT;
DECLARE @USERID INT;
DECLARE @GROUPID INT;
DECLARE @RESPONSE_MESSAGE NVARCHAR(250)

DECLARE @EMAIL NVARCHAR(254);
DECLARE @USERNAME NVARCHAR(30);

SET @EMAIL = 'example3@example.com';
SET @USERNAME = 'admin3';

EXEC SP_CREATE_USER 
@EMAIL = @EMAIL, 
@PASSWORD = 'admin123', 
@USERNAME = @USERNAME, 
@FIRST_NAME = 'felix', 
@LAST_NAME = 'martinez',
@USERID = @USERID OUT,
@RESPONSE_MESSAGE = @RESPONSE_MESSAGE OUT;

PRINT @RESPONSE_MESSAGE;

EXEC SP_USER_LOGIN 
@EMAIL = @EMAIL, 
@PASSWORD = 'admin123', 
@USERNAME = @USERNAME, 
@RESPONSE_MESSAGE = @RESPONSE_MESSAGE OUT;
PRINT @RESPONSE_MESSAGE;

EXEC SP_CREATE_CENTER
@EMAIL = 'example@example.com',
@NAME = 'IES Clara del Rey',
@ADDRESS = 'Calle del Padre Claret, 8',
@TELEPHONE = '999999999',
@CENTERID = @CENTERID OUT;

EXEC SP_CREATE_COURSE
@CENTER_ID = @CENTERID,
@NAME = 'Mi primer curso',
@DESCRIPTION = '#Normas Este es el primer curso del centro IES Clara del Rey. El curso est� enfocado a inform�tica para los alumnos de FP.',
@IS_VISIBLE = 1,
@COURSEID = @COURSEID OUT,
@GROUPID = @GROUPID OUT;

EXEC SP_CREATE_GROUP_CONTENT 'Unidad 1', 0, @COURSEID, @GROUPCONTENTID = @GROUPCONTENTID OUT;

EXEC SP_CREATE_CONTENT @TEXT = 'test', @GROUPCONTENT = @GROUPCONTENTID;

INSERT INTO USER_CENTER VALUES(@USERID, @CENTERID, 1);

INSERT INTO USER_COURSE VALUES (@USERID, @COURSEID, 1);

INSERT INTO USER_GROUP VALUES(@USERID, @GROUPID, GETDATE());

PRINT 'RESULTS';
PRINT @USERID;
PRINT @CENTERID;
PRINT @COURSEID;
PRINT @GROUPID;
PRINT @GROUPCONTENTID;

-- This should delete everything
 --EXEC SP_DELETE_USER @USERID;
 --EXEC SP_DELETE_CENTER @CENTERID;