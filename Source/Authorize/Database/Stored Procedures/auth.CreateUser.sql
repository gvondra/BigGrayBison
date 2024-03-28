﻿CREATE PROCEDURE [auth].[CreateUser]
	@id UNIQUEIDENTIFIER OUT,
	@name NVARCHAR(512),
	@emailAddressId UNIQUEIDENTIFIER,
	@isActive BIT,
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

	INSERT INTO [auth].[User] ([UserId], [Name], [EmailAddressId], [IsActive], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @name, @emailAddressId, @isActive, @timestamp, @timestamp);

	DECLARE @accountTimestamp DATETIME2(4);
	EXEC [auth].[CreateAccount] @accountId out, @name, @isActive, @accountTimestamp out;

	DECLARE @accountUserTimestamp DATETIME2(4);
	EXEC [auth].[ActivateAccoutUser] @accountId, @id, @isActive, @accountUserTimestamp out;
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
	END CATCH
END