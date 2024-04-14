CREATE PROCEDURE [auth].[CreateClient]
	@id UNIQUEIDENTIFIER OUT,
	@type SMALLINT,
	@secretKey UNIQUEIDENTIFIER,
	@secretSalt BINARY(16),
	@roles NVARCHAR(2048),
	@redirectionUrls NVARCHAR(3072),
	@isActive BIT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [auth].[Client] ([ClientId], [Type], [SecretKey], [SecretSalt], [Roles], [RedirectionUrls],
	[IsActive], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @type, @secretKey, @secretSalt, @roles, @redirectionUrls,
	@isActive, @timestamp, @timestamp);
END
