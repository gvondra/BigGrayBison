CREATE PROCEDURE [auth].[CreateSigningKey]
	@id UNIQUEIDENTIFIER OUT,
	@keyVaultKey UNIQUEIDENTIFIER,
	@isActive BIT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [auth].[SigningKey] ([SigningKeyId], [KeyVaultKey], [IsActive], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @keyVaultKey, @isActive, @timestamp, @timestamp);
END