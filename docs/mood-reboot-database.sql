USE [master]
GO
/****** Object:  Database [MOODREBOOT]    Script Date: 22/03/2023 22:27:09 ******/
CREATE DATABASE [MOODREBOOT]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MOODREBOOT', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.DESARROLLO\MSSQL\DATA\MOODREBOOT.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MOODREBOOT_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.DESARROLLO\MSSQL\DATA\MOODREBOOT_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MOODREBOOT] SET COMPATIBILITY_LEVEL = 160
GO
ALTER DATABASE [MOODREBOOT] COLLATE Latin1_General_100_BIN2_UTF8;
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MOODREBOOT].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MOODREBOOT] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MOODREBOOT] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MOODREBOOT] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MOODREBOOT] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MOODREBOOT] SET ARITHABORT OFF 
GO
ALTER DATABASE [MOODREBOOT] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MOODREBOOT] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MOODREBOOT] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MOODREBOOT] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MOODREBOOT] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MOODREBOOT] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MOODREBOOT] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MOODREBOOT] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MOODREBOOT] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MOODREBOOT] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MOODREBOOT] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MOODREBOOT] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MOODREBOOT] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MOODREBOOT] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MOODREBOOT] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MOODREBOOT] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MOODREBOOT] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MOODREBOOT] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MOODREBOOT] SET  MULTI_USER 
GO
ALTER DATABASE [MOODREBOOT] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MOODREBOOT] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MOODREBOOT] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MOODREBOOT] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MOODREBOOT] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MOODREBOOT] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MOODREBOOT] SET QUERY_STORE = ON
GO
ALTER DATABASE [MOODREBOOT] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MOODREBOOT]
GO
/****** Object:  Table [dbo].[CENTER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CENTER](
	[CENTER_ID] [int] NOT NULL,
	[EMAIL] [nvarchar](255) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[ADDRESS] [nvarchar](150) NOT NULL,
	[IMAGE] [nvarchar](max) NULL,
	[TELEPHONE] [nvarchar](15) NOT NULL,
	[DIRECTOR] [int] NULL,
	[APPROVED] [bit] NOT NULL,
 CONSTRAINT [PK_CENTER] PRIMARY KEY CLUSTERED 
(
	[CENTER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CONTENT](
	[CONTENT_ID] [int] NOT NULL,
	[TEXT] [nvarchar](max) NOT NULL,
	[GROUP_CONTENT_ID] [int] NOT NULL,
	[FILE_ID] [int] NULL,
 CONSTRAINT [PK_CONTENT] PRIMARY KEY CLUSTERED 
(
	[CONTENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CONTENT_COURSE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CONTENT_COURSE](
	[GROUP_CONTENT_ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[IS_VISIBLE] [bit] NOT NULL,
	[COURSE_ID] [int] NOT NULL,
 CONSTRAINT [PK_CONTENT_COURSE] PRIMARY KEY CLUSTERED 
(
	[GROUP_CONTENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COURSE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COURSE](
	[COURSE_ID] [int] NOT NULL,
	[DATE_PUBLISHED] [datetime] NOT NULL,
	[DATE_MODIFIED] [datetime] NULL,
	[DESCRIPTION] [nvarchar](350) NULL,
	[IMAGE] [nvarchar](max) NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[CENTER_ID] [int] NOT NULL,
	[IS_VISIBLE] [bit] NOT NULL,
	[GROUP_ID] [int] NULL,
	[PASSWORD] [nvarchar](12) NULL,
 CONSTRAINT [PK_COURSE] PRIMARY KEY CLUSTERED 
(
	[COURSE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FILE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FILE](
	[FILE_ID] [int] NOT NULL,
	[USER_ID] [int] NULL,
	[MIME_TYPE] [nvarchar](100) NOT NULL,
	[NAME] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_FILE] PRIMARY KEY CLUSTERED 
(
	[FILE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GROUP]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GROUP](
	[GROUP_ID] [int] NOT NULL,
	[IMAGE] [nvarchar](max) NULL,
	[NAME] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_GROUP] PRIMARY KEY CLUSTERED 
(
	[GROUP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MESSAGE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MESSAGE](
	[MESSAGE_ID] [int] NOT NULL,
	[GROUP_ID] [int] NOT NULL,
	[TEXT] [nvarchar](350) NULL,
	[USER_ID] [int] NOT NULL,
	[DATE_POSTED] [datetime] NOT NULL,
	[FILE_ID] [int] NULL,
	[USERNAME] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_MESSAGE] PRIMARY KEY CLUSTERED 
(
	[MESSAGE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER](
	[USER_ID] [int] NOT NULL,
	[EMAIL] [nvarchar](255) NOT NULL,
	[PASSWORD] [varbinary](max) NOT NULL,
	[USERNAME] [nvarchar](20) NOT NULL,
	[FIRST_NAME] [nvarchar](20) NOT NULL,
	[LAST_NAME] [nvarchar](30) NOT NULL,
	[SIGN_DATE] [datetime] NOT NULL,
	[IMAGE] [nvarchar](max) NULL,
	[ROLE] [nvarchar](10) NOT NULL,
	[LAST_SEEN] [datetime] NULL,
	[SALT] [nvarchar](max) NOT NULL,
	[PASS_TEST] [nvarchar](max) NULL,
	[APPROVED] [bit] NOT NULL,
 CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__USER__161CF72423A73214] UNIQUE NONCLUSTERED 
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__USER__161CF72482F72AEE] UNIQUE NONCLUSTERED 
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__USER__B15BE12E9D472A34] UNIQUE NONCLUSTERED 
(
	[USERNAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__USER__B15BE12ED1B98C1E] UNIQUE NONCLUSTERED 
(
	[USERNAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER_ACTION]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_ACTION](
	[ID] [int] NOT NULL,
	[USER_ID] [int] NOT NULL,
	[TOKEN] [nvarchar](max) NOT NULL,
	[REQUEST_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_USER_ACTION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER_CENTER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_CENTER](
	[USER_ID] [int] NOT NULL,
	[CENTER_ID] [int] NOT NULL,
	[IS_EDITOR] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER_COURSE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_COURSE](
	[ID] [int] NOT NULL,
	[USER_ID] [int] NOT NULL,
	[COURSE_ID] [int] NOT NULL,
	[IS_EDITOR] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER_GROUP]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_GROUP](
	[ID] [int] NOT NULL,
	[USER_ID] [int] NOT NULL,
	[GROUP_ID] [int] NULL,
	[JOIN_DATE] [datetime] NOT NULL,
	[LAST_SEEN] [datetime] NOT NULL,
	[IS_ADMIN] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CENTER]  WITH CHECK ADD  CONSTRAINT [FK_CENTER_USER] FOREIGN KEY([DIRECTOR])
REFERENCES [dbo].[USER] ([USER_ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[CENTER] CHECK CONSTRAINT [FK_CENTER_USER]
GO
ALTER TABLE [dbo].[CONTENT]  WITH CHECK ADD  CONSTRAINT [FK_CONTENT_CONTENT_COURSE] FOREIGN KEY([GROUP_CONTENT_ID])
REFERENCES [dbo].[CONTENT_COURSE] ([GROUP_CONTENT_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CONTENT] CHECK CONSTRAINT [FK_CONTENT_CONTENT_COURSE]
GO
ALTER TABLE [dbo].[CONTENT]  WITH CHECK ADD  CONSTRAINT [FK_CONTENT_FILE] FOREIGN KEY([FILE_ID])
REFERENCES [dbo].[FILE] ([FILE_ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[CONTENT] CHECK CONSTRAINT [FK_CONTENT_FILE]
GO
ALTER TABLE [dbo].[CONTENT_COURSE]  WITH CHECK ADD  CONSTRAINT [FK_CONTENT_COURSE_COURSE] FOREIGN KEY([COURSE_ID])
REFERENCES [dbo].[COURSE] ([COURSE_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CONTENT_COURSE] CHECK CONSTRAINT [FK_CONTENT_COURSE_COURSE]
GO
ALTER TABLE [dbo].[COURSE]  WITH CHECK ADD  CONSTRAINT [FK_COURSE_CENTER] FOREIGN KEY([CENTER_ID])
REFERENCES [dbo].[CENTER] ([CENTER_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[COURSE] CHECK CONSTRAINT [FK_COURSE_CENTER]
GO
ALTER TABLE [dbo].[COURSE]  WITH CHECK ADD  CONSTRAINT [FK_COURSE_GROUP] FOREIGN KEY([GROUP_ID])
REFERENCES [dbo].[GROUP] ([GROUP_ID])
GO
ALTER TABLE [dbo].[COURSE] CHECK CONSTRAINT [FK_COURSE_GROUP]
GO
ALTER TABLE [dbo].[FILE]  WITH CHECK ADD  CONSTRAINT [FK_FILE_USER] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USER] ([USER_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FILE] CHECK CONSTRAINT [FK_FILE_USER]
GO
ALTER TABLE [dbo].[MESSAGE]  WITH CHECK ADD  CONSTRAINT [FK_MESSAGE_FILE] FOREIGN KEY([FILE_ID])
REFERENCES [dbo].[FILE] ([FILE_ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[MESSAGE] CHECK CONSTRAINT [FK_MESSAGE_FILE]
GO
ALTER TABLE [dbo].[MESSAGE]  WITH CHECK ADD  CONSTRAINT [FK_MESSAGE_GROUP] FOREIGN KEY([GROUP_ID])
REFERENCES [dbo].[GROUP] ([GROUP_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MESSAGE] CHECK CONSTRAINT [FK_MESSAGE_GROUP]
GO
ALTER TABLE [dbo].[MESSAGE]  WITH CHECK ADD  CONSTRAINT [FK_MESSAGE_USER] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USER] ([USER_ID])
GO
ALTER TABLE [dbo].[MESSAGE] CHECK CONSTRAINT [FK_MESSAGE_USER]
GO
ALTER TABLE [dbo].[USER_ACTION]  WITH CHECK ADD  CONSTRAINT [FK_USER_ACTION_USER] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USER] ([USER_ID])
GO
ALTER TABLE [dbo].[USER_ACTION] CHECK CONSTRAINT [FK_USER_ACTION_USER]
GO
ALTER TABLE [dbo].[USER_CENTER]  WITH CHECK ADD  CONSTRAINT [FK_USER_CENTER_CENTER] FOREIGN KEY([CENTER_ID])
REFERENCES [dbo].[CENTER] ([CENTER_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[USER_CENTER] CHECK CONSTRAINT [FK_USER_CENTER_CENTER]
GO
ALTER TABLE [dbo].[USER_CENTER]  WITH CHECK ADD  CONSTRAINT [FK_USER_CENTER_USER] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USER] ([USER_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[USER_CENTER] CHECK CONSTRAINT [FK_USER_CENTER_USER]
GO
ALTER TABLE [dbo].[USER_COURSE]  WITH CHECK ADD  CONSTRAINT [FK_USER_COURSE_COURSE] FOREIGN KEY([COURSE_ID])
REFERENCES [dbo].[COURSE] ([COURSE_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[USER_COURSE] CHECK CONSTRAINT [FK_USER_COURSE_COURSE]
GO
ALTER TABLE [dbo].[USER_COURSE]  WITH CHECK ADD  CONSTRAINT [FK_USER_COURSE_USER] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USER] ([USER_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[USER_COURSE] CHECK CONSTRAINT [FK_USER_COURSE_USER]
GO
ALTER TABLE [dbo].[USER_GROUP]  WITH CHECK ADD  CONSTRAINT [FK_USER_GROUP_GROUP] FOREIGN KEY([GROUP_ID])
REFERENCES [dbo].[GROUP] ([GROUP_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[USER_GROUP] CHECK CONSTRAINT [FK_USER_GROUP_GROUP]
GO
ALTER TABLE [dbo].[USER_GROUP]  WITH CHECK ADD  CONSTRAINT [FK_USER_GROUP_USER] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USER] ([USER_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[USER_GROUP] CHECK CONSTRAINT [FK_USER_GROUP_USER]
GO
ALTER TABLE [dbo].[USER]  WITH CHECK ADD  CONSTRAINT [CK__USER__ROLE] CHECK  (([ROLE]='ADMIN' OR [ROLE]='USER'))
GO
ALTER TABLE [dbo].[USER] CHECK CONSTRAINT [CK__USER__ROLE]
GO
/****** Object:  StoredProcedure [dbo].[SP_CENTER_COURSES]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CENTER_COURSES]
(@USER_ID INT, @CENTER_ID INT)
AS
	SELECT C.COURSE_ID, C.DATE_PUBLISHED, C.DATE_MODIFIED, C.[DESCRIPTION], C.[IMAGE], C.[NAME], CT.[NAME], UC.IS_EDITOR
	FROM COURSE C INNER JOIN CENTER CT
	ON C.CENTER_ID = CT.CENTER_ID
	INNER JOIN USER_COURSE UC
	ON UC.COURSE_ID = C.COURSE_ID
	WHERE C.CENTER_ID = @CENTER_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_COURSE_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_COURSE_CONTENT]
(@COURSE_ID INT)
AS
	-- Course arquitecture: Course -> Contents Course (groups of content) -> Content
	SELECT CT.CONTENT_ID, CT.[TEXT], CT.[FILE_ID] 
	FROM CONTENT CT INNER JOIN CONTENT_COURSE CC
	ON CT.GROUP_CONTENT_ID = CC.GROUP_CONTENT_ID
	WHERE CC.COURSE_ID = @COURSE_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_CENTER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_CENTER]
(@EMAIL NVARCHAR(255), @NAME NVARCHAR(50), @ADDRESS NVARCHAR(150), @TELEPHONE NVARCHAR(15), @APPROVED BIT, @IMAGE NVARCHAR(MAX) = NULL, @DIRECTOR INT = NULL, @CENTERID INT OUTPUT)
AS
	--Generate new ID
	DECLARE @NEWID INT;
	SELECT @NEWID = MAX(CENTER_ID) + 1 FROM CENTER;

	IF (@NEWID IS NULL) BEGIN
		SET @NEWID = 0;
	END

	--Insert new center
	INSERT INTO CENTER VALUES(@NEWID, @EMAIL, @NAME, @ADDRESS, @IMAGE, @TELEPHONE, @DIRECTOR, @APPROVED);

	SET @CENTERID = @NEWID;
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_CONTENT]
(@TEXT NVARCHAR(MAX), @GROUPCONTENT INT, @FILEID INT = NULL)
AS
	--Generate new ID
	DECLARE @NEWID INT;
	SELECT @NEWID = MAX(CONTENT_ID) + 1 FROM CONTENT;

	IF (@NEWID IS NULL) BEGIN
		SET @NEWID = 0;
	END

	INSERT INTO CONTENT VALUES (@NEWID, @TEXT, @GROUPCONTENT, @FILEID);
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_COURSE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_COURSE]
(@CENTER_ID INT, @NAME NVARCHAR(30), @DESCRIPTION NVARCHAR(350) = NULL, @IMAGE NVARCHAR(MAX) = NULL, @IS_VISIBLE BIT = 0, @PASSWORD NVARCHAR(12) = NULL, @COURSEID INT OUTPUT, @GROUPID INT OUTPUT)
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
/****** Object:  StoredProcedure [dbo].[SP_CREATE_FILE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_FILE]
(@NAME NVARCHAR(MAX), @MIME_TYPE NVARCHAR(100), @USER_ID INT = NULL, @FILE_ID INT OUTPUT)
AS
	DECLARE @FILEID INT;
	SELECT @FILEID = MAX([FILE_ID]) + 1 FROM [FILE];

	IF (@FILEID IS NULL) BEGIN
		SET @FILEID = 0;
	END

	INSERT INTO [FILE] VALUES (@FILEID, @USER_ID, @MIME_TYPE, @NAME);
	
	SET @FILE_ID = @FILEID;
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_GROUP]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CREATE_GROUP]
(@NAME NVARCHAR(45), @IMAGE NVARCHAR(MAX) = NULL, @GROUPID INT OUTPUT)
AS
	DECLARE @GROUP_ID INT;
	SELECT @GROUP_ID = (MAX([GROUP].GROUP_ID) + 1) FROM [GROUP];

	IF (@GROUP_ID IS NULL) BEGIN
		SET @GROUP_ID = 0;
	END

	INSERT INTO [GROUP] VALUES (@GROUP_ID, @IMAGE, @NAME);

	SET @GROUPID = @GROUP_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_GROUP_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_GROUP_CONTENT]
(@NAME NVARCHAR(50), @COURSE_ID INT, @IS_VISIBLE BIT = 0, @GROUPCONTENTID INT OUTPUT)
AS
	DECLARE @NEW_ID INT;
	SELECT @NEW_ID = MAX(GROUP_CONTENT_ID) + 1 FROM CONTENT_COURSE;

	IF (@NEW_ID IS NULL) BEGIN
		SET @NEW_ID = 0;
	END

	INSERT INTO CONTENT_COURSE VALUES (@NEW_ID, @NAME, @IS_VISIBLE, @COURSE_ID);

	SET @GROUPCONTENTID = @NEW_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_MESSAGE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_MESSAGE]
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
	INSERT INTO [MESSAGE] VALUES (@MESSAGE_ID, @GROUP_ID, @CONTENT, @USER_ID, GETDATE(), @FILE_ID, @USERNAME);
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_USER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_USER]
@EMAIL NVARCHAR(255),
@PASSWORD NVARCHAR(12),
@USERNAME NVARCHAR(30),
@FIRST_NAME NVARCHAR(30),
@LAST_NAME NVARCHAR(30),
@APPROVED BIT,
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
			VALUES(@NEW_ID, @EMAIL, @HASHPASSWORD, @USERNAME, @FIRST_NAME, @LAST_NAME, GETDATE(), @IMAGE, @ROLE, null, CAST(@salt AS NVARCHAR(MAX)), @PASSWORD, @APPROVED);

			SET @RESPONSE_MESSAGE='Success'
		END

	END TRY
	BEGIN CATCH
		SET @RESPONSE_MESSAGE=ERROR_MESSAGE() 
	END CATCH
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_USER_COURSE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_CREATE_USER_COURSE]
(@USER_ID INT, @COURSE_ID INT, @IS_EDITOR BIT)
AS
	DECLARE @NEWID INT;
	SELECT @NEWID = MAX([ID]) + 1 FROM [USER_COURSE];

	IF (@NEWID IS NULL) BEGIN
		SET @NEWID = 0;
	END

	INSERT INTO [USER_COURSE] VALUES (@NEWID, @USER_ID, @COURSE_ID, @IS_EDITOR);
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_CENTER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_DELETE_CENTER]
(@CENTER_ID INT)
AS
	DELETE FROM CENTER WHERE CENTER_ID = @CENTER_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_DELETE_CONTENT]
(@CONTENTID INT)
AS
	DELETE FROM CONTENT WHERE CONTENT_ID = @CONTENTID;
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_COURSE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_DELETE_COURSE]
(@COURSE_ID INT)
AS
	DECLARE @GROUP_ID INT;
	SELECT @GROUP_ID = GROUP_ID FROM COURSE WHERE COURSE_ID = @COURSE_ID;
	DELETE FROM [GROUP] WHERE GROUP_ID = @GROUP_ID;
	DELETE FROM COURSE WHERE COURSE_ID = @COURSE_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_FILE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_DELETE_FILE]
(@FILEID INT)
AS
	DELETE FROM [FILE] WHERE [FILE_ID] = @FILEID;
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_GROUP]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_DELETE_GROUP]
(@GROUP_ID INT)
AS
	DELETE FROM [GROUP] WHERE GROUP_ID = @GROUP_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_GROUP_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_DELETE_GROUP_CONTENT]
(@GROUP_CONTENT_ID INT)
AS
	DELETE FROM CONTENT_COURSE WHERE GROUP_CONTENT_ID = @GROUP_CONTENT_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_USER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_DELETE_USER]
(@USER_ID INT)
AS
	DELETE FROM [USER] WHERE USER_ID = @USER_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_GROUP_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_GROUP_CONTENT]
(@GROUP_CONTENT_ID INT)
AS
	-- Course arquitecture: Course -> Contents Course (groups of content) -> Content
	SELECT C.CONTENT_ID, C.FILE_ID, C.TEXT
	FROM CONTENT C INNER JOIN CONTENT_COURSE GC
	ON C.GROUP_CONTENT_ID = GC.GROUP_CONTENT_ID
	WHERE GC.GROUP_CONTENT_ID = @GROUP_CONTENT_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_CENTER]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_UPDATE_CENTER]
(@CENTER_ID INT, @EMAIL NVARCHAR(255), @NAME NVARCHAR(50), @ADDRESS NVARCHAR(150), @IMAGE NVARCHAR(MAX) = NULL, @TELEPHONE NVARCHAR(15))
AS
	UPDATE CENTER SET EMAIL = @EMAIL, NAME = @NAME, ADDRESS = @ADDRESS, IMAGE = @IMAGE, TELEPHONE = @TELEPHONE
	WHERE CENTER_ID = @CENTER_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_UPDATE_CONTENT]
(@CONTENTID INT, @CONTENT NVARCHAR(MAX))
AS
	UPDATE CONTENT SET [TEXT] = @CONTENT
	WHERE CONTENT_ID = @CONTENTID;
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_COURSE]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_UPDATE_COURSE]
(@COURSE_ID INT, @NAME NVARCHAR(30), @DESCRIPTION NVARCHAR(350) = NULL, @IMAGE NVARCHAR(MAX) = NULL)
AS
	UPDATE [COURSE] SET [NAME] = @NAME, [IMAGE] = @IMAGE, [DESCRIPTION] = @DESCRIPTION
	WHERE [COURSE].COURSE_ID = @COURSE_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_GROUP]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_UPDATE_GROUP]
(@GROUP_ID INT, @NAME NVARCHAR(45), @IMAGE NVARCHAR(MAX) = NULL)
AS
	UPDATE [GROUP] SET NAME = @NAME, IMAGE = @IMAGE;
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_GROUP_CONTENT]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_UPDATE_GROUP_CONTENT]
(@GROUP_CONTENT_ID INT, @NAME NVARCHAR(50), @IS_VISIBLE BIT)
AS
	UPDATE CONTENT_COURSE SET [NAME] = @NAME, IS_VISIBLE = @IS_VISIBLE WHERE GROUP_CONTENT_ID = @GROUP_CONTENT_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_USER_CENTERS]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_USER_CENTERS]
(@USER_ID INT)
AS
	SELECT  C.ADDRESS, C.CENTER_ID, C.EMAIL, C.IMAGE, C.NAME, C.NAME, C.TELEPHONE, UC.IS_EDITOR
	FROM CENTER C INNER JOIN USER_CENTER UC
	ON C.CENTER_ID = UC.CENTER_ID
	WHERE UC.USER_ID = @USER_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_USER_COURSES]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_USER_COURSES]
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
/****** Object:  StoredProcedure [dbo].[SP_USER_GROUPS]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_USER_GROUPS]
(@USER_ID INT)
AS
	SELECT G.GROUP_ID, G.IMAGE, G.NAME 
	FROM [GROUP] G INNER JOIN USER_GROUP UG
	ON G.GROUP_ID = UG.GROUP_ID
	WHERE UG.[USER_ID] = @USER_ID;
GO
/****** Object:  StoredProcedure [dbo].[SP_USER_LOGIN]    Script Date: 22/03/2023 22:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_USER_LOGIN]
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
USE [master]
GO
ALTER DATABASE [MOODREBOOT] SET  READ_WRITE 
GO
