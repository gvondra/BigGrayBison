CREATE PROCEDURE [auth].[UpdateUser]
	@id UNIQUEIDENTIFIER,
	@name NVARCHAR(512),
	@emailAddressId UNIQUEIDENTIFIER,
	@isActive BIT,
	@roles INT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [auth].[User]
	SET [Name] = @name,
	[EmailAddressId] = @emailAddressId,
	[IsActive] = @isActive,
	[Roles] = @roles,
	[UpdateTimestamp] = @timestamp
	WHERE [UserId] = @id;
END