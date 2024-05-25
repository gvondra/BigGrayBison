CREATE PROCEDURE [auth].[GetClient]
	@id UNIQUEIDENTIFIER
AS
SELECT [ClientId],[Type],[SecretKey],[SecretSalt],[Roles],[RedirectionUrls],
[IsActive],[CreateTimestamp],[UpdateTimestamp]
FROM [auth].[Client]
WHERE [ClientId] = @id
;