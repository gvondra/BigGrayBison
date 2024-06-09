CREATE PROCEDURE [auth].[GetUserCredentialByUserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT 
[UserCredentialId],
[UserId],
[SecretKey],
[SecretSalt],
[IsActive],
[Expiration],
[CreateTimestamp],
[UpdateTimestamp]
FROM [auth].[UserCredential]
WHERE [UserId] = @userId
ORDER BY [UpdateTimestamp] DESC
;