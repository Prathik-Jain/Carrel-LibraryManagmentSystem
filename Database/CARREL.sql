 --CREATING DATABASE CARREL
 CREATE DATABASE CARREL
 GO
-------------------------------------------------------------------------------
 --USE CARREL DATABASE
 USE CARREL
 GO

 -------------------------------------------------------------------------------
--CREATING SEQUENCES
-------------------------------------------------------------------------------

 --CREATING BOOK SEQUENCE
 CREATE SEQUENCE BookSequence
	AS INT
	START WITH 1
	INCREMENT BY 1
	MINVALUE 0
	MAXVALUE 9999999 --~1Cr. BOOKS
	CYCLE
	NO CACHE
-------------------------------------------------------------------------------

--CREATING ADMIN SEQUENCE
 CREATE SEQUENCE AdminSequence
	AS TINYINT
	START WITH 1
	INCREMENT BY 1
	MINVALUE 0
	NO MAXVALUE --255 Admins Max.
	NO CACHE
-------------------------------------------------------------------------------

--CREATING STUDENT SEQUENCE
 CREATE SEQUENCE MemberSequence
	AS SMALLINT
	START WITH 1
	INCREMENT BY 1
	MINVALUE 0
	MAXVALUE 9999 --~10K. Members
	CYCLE
	NO CACHE
GO

-------------------------------------------------------------------------------
--CREATING TABLES
-------------------------------------------------------------------------------


--Creating Book table
	CREATE TABLE [dbo].[Book](
	[ID]  [INT] NOT NULL,
	[UID] AS ('BOOK-'+right(CONVERT([VARCHAR](7),[ID]),(7))) PERSISTED UNIQUE NOT NULL, --BOOK-1 to BOOK-9999999
	[ISBN] [NCHAR](10) NOT NULL,
	[TITLE]  [NVARCHAR](MAX) NOT NULL,
	[AUTHOR] [NVARCHAR](MAX) NOT NULL, --Will contain JSON string of authors.
	[PUBLISHER] [NVARCHAR](MAX) NOT NULL,
	[EDITION] [NCHAR](3) NULL,
	[PRICE] [SMALLINT] NOT NULL,
	[AVAILABLE] [BIT] NOT NULL DEFAULT 1,
	[RACK] [NVARCHAR](2) NULL, 
	[QRPRINTED] [BIT] NOT NULL DEFAULT 0,
	[LASTBORROWED] [NVARCHAR](MAX) NULL, -- Will contain JSON string of last borrower with time.
	[ADDEDON] [DATE] NOT NULL DEFAULT CONVERT(DATE,GETDATE(),6)
	PRIMARY KEY CLUSTERED ([UID] ASC)
	);

-------------------------------------------------------------------------------
--CREATING Department Table
	CREATE TABLE [dbo].[Dept](
	[ID] INT NOT NULL IDENTITY,
 	[NAME] [NVARCHAR](MAX) NOT NULL
	);

-------------------------------------------------------------------------------
--Creating Admin Table
	CREATE TABLE [dbo].[Admin] (
    [ID]        [TINYINT]           NOT NULL,
    [UID]       AS            ('ADM-'+right('00'+CONVERT([varchar](4),[ID]),(3))) PERSISTED UNIQUE NOT NULL, --ADM-001 to ADM255
    [FNAME]     [NVARCHAR](MAX)  NOT NULL,
    [LNAME]     [NVARCHAR](MAX)  NULL,
	[PHONE]     [NCHAR] (10)    NOT NULL,
	[PIN]       [NCHAR] (4)      NOT NULL,
	[SECURITYQUESTION] [NVARCHAR](MAX) NOT NULL,
	[ANSWER]	[NVARCHAR](MAX)     NOT NULL,
    [IMAGE]     [IMAGE]         NULL,
    PRIMARY KEY CLUSTERED ([UID] ASC)
);

-------------------------------------------------------------------------------
--Creating Member Table
	CREATE TABLE [dbo].[Member](
	[ID]        [SMALLINT]     NOT NULL,
    [UID]       AS            ('MEM-'+right('000'+CONVERT([varchar](4),[ID]),(4))) PERSISTED UNIQUE NOT NULL, --MEM-0001 to MEM-9999
    [FNAME]     [NVARCHAR](MAX)  NOT NULL,
    [LNAME]     [NVARCHAR](MAX)  NULL,
	[PHONE]     [NCHAR](10)    NOT NULL,
	[DEPT]		[NCHAR](3) NOT NULL,
	[SEM]		[TINYINT] NULL,
	[MAXBOOKS]	[TINYINT] NOT NULL DEFAULT 2,
	[BORROWEDBOOKS] [NVARCHAR](MAX) NULL,
	[DUES]		[NVARCHAR](MAX) NULL,
	[IMAGE]		[IMAGE] NULL,
	PRIMARY KEY CLUSTERED ([UID] ASC)
	);	
-------------------------------------------------------------------------------
--CREATING Author Table
	CREATE TABLE [dbo].[Author](
	[ID] INT NOT NULL IDENTITY,
 	[NAME] [NVARCHAR](MAX) NOT NULL
	);

-------------------------------------------------------------------------------

--CREATING JSON TABLE TO HOLD ALL JSON STRINGS
CREATE TABLE [dbo].[JSON](
	[CATEGORY]	[NVARCHAR](4) NOT NULL,
	[DATA]		[NVARCHAR](MAX) NOT NULL,
	[NUMBER] [SMALLINT] NULL DEFAULT 1
 );
 GO

-------------------------------------------------------------------------
--CREATING INDEXES
-------------------------------------------------------------------------

-- Creating non-clustured index on book.isbn
CREATE NONCLUSTERED INDEX BOOK_ISBN   
    ON Book (ISBN);   
GO 

-------------------------------------------------------------------------
--CREATING A TRIGGER TO FIRE AS SOON AS DATA IS ENTERED INTO JSON TABLE
-------------------------------------------------------------------------
 CREATE TRIGGER AddData ON JSON
	 FOR INSERT
	 AS
	 DECLARE @CATEGORY [NVARCHAR](4)
	 SELECT @CATEGORY = I.CATEGORY FROM INSERTED I  
	 DECLARE @DATA [NVARCHAR](MAX)
	 SELECT @DATA = I.DATA FROM INSERTED I
	 DECLARE @NUMBER [SMALLINT]
	 SELECT @NUMBER = I.NUMBER FROM INSERTED I
	 DECLARE @COUNT [SMALLINT] = @NUMBER 
	 IF CHARINDEX('BOOK',@CATEGORY) > 0
	 BEGIN
		WHILE @COUNT > 0
		BEGIN
			DECLARE @BOOKID SMALLINT = NEXT VALUE FOR BookSequence
			SET @DATA = JSON_MODIFY(@DATA,'$.ID',@BOOKID)
			INSERT INTO Book(ID,ISBN,TITLE,AUTHOR,PUBLISHER,EDITION,PRICE,RACK)
			SELECT * FROM OPENJSON(@DATA)
			WITH (
				ID INT '$.ID',
				ISBN NCHAR(10) '$.ISBN',
				TITLE NVARCHAR(MAX) '$.Title',
				AUTHOR NVARCHAR(MAX) '$.Author',
				PUBLSIHER NVARCHAR(MAX) '$.Publisher',
				EDITION TINYINT '$.Edition',
				PRICE SMALLINT '$.Price',
				RACK NVARCHAR(2) '$.Rack'
			)
			SET @COUNT -= 1 --Decreases the count by one.
		END
	END
	IF CHARINDEX('MEM',@CATEGORY) >0
	BEGIN
		DECLARE @MEMID SMALLINT = NEXT VALUE FOR MemberSequence
		SET @DATA = JSON_MODIFY(@DATA,'$.ID',@MEMID)
		INSERT INTO Member (ID,FNAME,LNAME,PHONE,DEPT,SEM)
		SELECT * FROM OPENJSON(@DATA)
		WITH (
			ID SMALLINT '$.ID',
			FNAME VARCHAR(MAX) '$.FName',
			LNAME VARCHAR(MAX) '$.LName',
			PHONE NCHAR(10) '$.Phone',
			DEPT NCHAR(3) '$.Dept',
			SEM TINYINT '$.Sem'
		)
	END
	 IF CHARINDEX('ADM',@CATEGORY) > 0 
	 BEGIN 
		DECLARE @ADMID SMALLINT = NEXT VALUE FOR AdminSequence
		SET @DATA = JSON_MODIFY(@DATA,'$.ID',@ADMID)
		INSERT INTO Admin (ID,FNAME,LNAME,PHONE,PIN,SECURITYQUESTION,ANSWER)
		SELECT * FROM OPENJSON(@DATA)
		WITH (
			ID SMALLINT '$.ID',
			FNAME NVARCHAR(MAX) '$.FName',
			LNAME NVARCHAR(MAX) '$.LName',
			PHONE NCHAR(10) '$.Phone',
			PIN NCHAR(4) '$.Pin',
			SECQURITYQUESTION NVARCHAR(MAX) '$.SecurityQuestion',
			ANSWER NVARCHAR(MAX) '$.Answer'
		)
	 END
GO
-------------------------------------------------------------------------------