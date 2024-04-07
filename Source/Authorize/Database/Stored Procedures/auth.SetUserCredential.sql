CREATE PROCEDURE [auth].[SetUserCredential]
	@id UNIQUEIDENTIFIER OUT,
	@userId UNIQUEIDENTIFIER,
	@secretKey UNIQUEIDENTIFIER,
	@secretSalt BINARY(16),
	@isActive BIT,
	@expiration DATETIME2(1),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();

	UPDATE [auth].[UserCredential]
	SET [IsActive] = 0,
	[UpdateTimestamp] = @timestamp
	WHERE [UserId] = @userId
	;

    INSERT INTO [auth].[UserCredential] ([UserCredentialId],[UserId],[SecretKey],[SecretSalt],[IsActive],[Expiration],[CreateTimestamp],[UpdateTimestamp])
	VALUES (@id, @userId, @secretKey, @secretSalt, @isActive, @expiration, @timestamp, @timestamp);
	;
END