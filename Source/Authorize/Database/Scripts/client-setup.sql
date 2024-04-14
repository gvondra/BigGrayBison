/*
Run this script as part of system setup/installation add an initial client
*/
DECLARE @clientId UNIQUEIDENTIFIER;
DECLARE @timestamp DATETIME2(4);
IF NOT EXISTS (SELECT TOP 1 1 FROM [auth].[Client])
BEGIN
	EXEC [auth].[CreateClient] @clientId OUT, 2, null, null, '', '["http://localhost:5000"]', 1, @timestamp OUT;
	PRINT 'Created Client with Id';
	PRINT @clientId
END
ELSE
BEGIN
	PRINT 'No client created'
END