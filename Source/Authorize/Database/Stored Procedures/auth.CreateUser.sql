﻿CREATE PROCEDURE [auth].[CreateUser]
	@id UNIQUEIDENTIFIER OUT,
	@name NVARCHAR(512),
	@emailAddressId UNIQUEIDENTIFIER,
	@isActive BIT,
	@roles INT,
	@masterKey UNIQUEIDENTIFIER,
	@secretSalt BINARY(16),
	@secretKey VARBINARY(256),
	@secret VARBINARY(1024),
	@credentialExpiration DATETIME2(1),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	DECLARE @count INT;
	DECLARE @message NVARCHAR(512);
	DECLARE @accountId UNIQUEIDENTIFIER;
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE; 

	BEGIN TRY
	BEGIN TRANSACTION;

	IF [auth].[IsUserNameAvailable](@name) = 0
	BEGIN
		SET @message = N'Name is alreadry in use: ' + @name;
		RAISERROR (@message, 16, 1);
	END

	INSERT INTO [auth].[User] ([UserId], [Name], [EmailAddressId], [IsActive], [Roles], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @name, @emailAddressId, @isActive, @roles, @timestamp, @timestamp);

	DECLARE @accountTimestamp DATETIME2(4);
	EXEC [auth].[CreateAccount] @accountId out, @name, @isActive, @accountTimestamp out;

	DECLARE @accountUserTimestamp DATETIME2(4);
	EXEC [auth].[ActivateAccoutUser] @accountId, @id, @isActive, @accountUserTimestamp out;

	DECLARE @credentialId UNIQUEIDENTIFIER;
	DECLARE @credentialTimestamp DATETIME2(4);
	EXEC [auth].[SetUserCredential] @credentialId OUT, @id, @masterKey, @secretSalt, @secretKey, @secret, 1, @credentialExpiration, @credentialTimestamp OUT;

	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
	END CATCH
END