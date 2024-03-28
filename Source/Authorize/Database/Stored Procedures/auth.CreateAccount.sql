CREATE PROCEDURE [auth].[CreateAccount]
	@id UNIQUEIDENTIFIER OUT,
	@name NVARCHAR(512),
	@isActive BIT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [auth].[Account] ([AccountId], [Name], [IsActive], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @name, @isActive, @timestamp, @timestamp);
END