CREATE PROCEDURE [auth].[SetUserCredential]
	@id UNIQUEIDENTIFIER OUT,
	@userId UNIQUEIDENTIFIER,
	@masterKey UNIQUEIDENTIFIER,
	@secretSalt BINARY(16),
	@secretKey VARBINARY(256),
	@secret VARBINARY(1024),
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

    INSERT INTO [auth].[UserCredential] ([UserCredentialId],[UserId],[MasterKey],[SecretSalt],[SecretKey],[Secret],[IsActive],[Expiration],[CreateTimestamp],[UpdateTimestamp])
	VALUES (@id, @userId, @masterKey, @secretSalt, @secretKey, @secret, @isActive, @expiration, @timestamp, @timestamp);
	;
END