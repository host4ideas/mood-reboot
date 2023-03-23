DECLARE @GROUPCONTENTID INT;
DECLARE @CENTERID INT;
DECLARE @COURSEID INT;
DECLARE @USERID INT;
DECLARE @GROUPID INT;
DECLARE @RESPONSE_MESSAGE NVARCHAR(250)

DECLARE @EMAIL NVARCHAR(254);
DECLARE @USERNAME NVARCHAR(30);

SET @EMAIL = 'example1@example.com';
SET @USERNAME = 'admin1';

EXEC SP_CREATE_USER 
@EMAIL = @EMAIL, 
@PASSWORD = 'admin123', 
@USERNAME = @USERNAME, 
@FIRST_NAME = 'felix', 
@LAST_NAME = 'martinez',
@APPROVED = 1,
@USERID = @USERID OUT,
@IMAGE = 'default_user_logo.svg',
@ROLE = 'ADMIN',
@RESPONSE_MESSAGE = @RESPONSE_MESSAGE OUT;

PRINT @RESPONSE_MESSAGE;

EXEC SP_CREATE_CENTER
@EMAIL = 'example@example.com',
@NAME = 'IES Clara del Rey',
@ADDRESS = 'Calle del Padre Claret, 8',
@TELEPHONE = '999999999',
@APPROVED = 1,
@DIRECTOR = @USERID,
@CENTERID = @CENTERID OUT;

EXEC SP_CREATE_COURSE
@CENTER_ID = @CENTERID,
@NAME = 'Mi primer curso',
@DESCRIPTION = '#Normas Este es el primer curso del centro IES Clara del Rey. El curso está enfocado a informática para los alumnos de FP.',
@IS_VISIBLE = 1,
@COURSEID = @COURSEID OUT,
@GROUPID = @GROUPID OUT;

EXEC SP_CREATE_GROUP_CONTENT 'Unidad 1', 0, @COURSEID, @GROUPCONTENTID = @GROUPCONTENTID OUT;

EXEC SP_CREATE_CONTENT @TEXT = 'test', @GROUPCONTENT = @GROUPCONTENTID;

DECLARE @UC_ID INT;
SET @UC_ID = @USERID;
INSERT INTO USER_CENTER VALUES(@UC_ID, @USERID, @CENTERID, 1);

EXEC SP_CREATE_USER_COURSE @USERID, @COURSEID, 1;

DECLARE @UG_ID INT;
SET @UG_ID = @USERID;
INSERT INTO USER_GROUP VALUES(@UG_ID, @USERID, @GROUPID, GETDATE(), GETDATE(), 1);

PRINT 'RESULTS';
PRINT @USERID;
PRINT @CENTERID;
PRINT @COURSEID;
PRINT @GROUPID;
PRINT @GROUPCONTENTID;

-- This should delete everything
 --EXEC SP_DELETE_USER @USERID;
 --EXEC SP_DELETE_CENTER @CENTERID;
