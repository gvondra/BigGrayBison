CREATE PROCEDURE [auth].[UpdateUser]
	@id UNIQUEIDENTIFIER,
	@name NVARCHAR(512),
	@emailAddressId UNIQUEIDENTIFIER,
	@isActive BIT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [auth].[User]
	SET [Name] = @name,
	[EmailAddressId] = @emailAddressId,
	[IsActive] = @isActive,
	[UpdateTimestamp] = @timestamp
	WHERE [UserId] = @id;
END