CREATE PROCEDURE [auth].[UpdateAccount]
	@id UNIQUEIDENTIFIER,
	@name NVARCHAR(512),
	@isActive BIT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [auth].[Account]
	SET [Name] = @name,
	[IsActive] = @isActive,
	[UpdateTimestamp] = @timestamp
	WHERE [AccountId] = @id;
END