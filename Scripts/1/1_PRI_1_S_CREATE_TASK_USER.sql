-- FOLLOW THIS FORMAT WHEN CREATING A SCRIPT. THIS WAY WE DON'T DUPLICATE SCRIPT RUNS
DECLARE @TICKETID AS VARCHAR(10) = '1' -- Your task/ticket id
DECLARE @VERSION AS NUMERIC(3,0) = 4 -- The version of your script. If you need to update a script in the future make sure to increment this.
DECLARE @UPDATEID AS NUMERIC(3,0) -- Don't touch

SELECT @UPDATEID = COUNT(*) FROM TASK_PATCH WHERE PATCH_ID = @TICKETID AND VERSION = @VERSION -- We check if a record exists in the database for this patch with the version number. If it doesn't, execute the code
IF @UPDATEID = 0 -- checking if no record was found
BEGIN
	CREATE TABLE [TASK_USER](
		[USER_GU] [uniqueidentifier] NOT NULL PRIMARY KEY,
		[USERNAME] [nvarchar](20) NOT NULL,
		[PASSWORD] [nvarchar](50) NOT NULL,
		[EMAIL] [nvarchar](50) NOT NULL,
		[ADD_TIMESTAMP] [smalldatetime],
		[UPDATE_TIMESTAMP] [smalldatetime]
		CONSTRAINT UC_USERNAME UNIQUE (USERNAME)
	)

	INSERT INTO TASK_PATCH(PATCH_GU, PATCH_ID, VERSION, ADD_TIMESTAMP) VALUES (NEWID(), @TICKETID, @VERSION, GETDATE()) -- insert the patch denoting script has run
END