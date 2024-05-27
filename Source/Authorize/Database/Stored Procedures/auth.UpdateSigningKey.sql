CREATE PROCEDURE [auth].[UpdateSigningKey]
	@id UNIQUEIDENTIFIER,
	@isActive BIT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [auth].[SigningKey]
	SET [IsActive] = @isActive,
	[UpdateTimestamp] = @timestamp
	WHERE [SigningKeyId] = @id;
END