CREATE PROCEDURE [auth].[GetUserCredentialByUserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT 
[UserCredentialId],
[UserId],
[MasterKey],
[SecretSalt],
[SecretKey],
[Secret],
[IsActive],
[Expiration],
[CreateTimestamp],
[UpdateTimestamp]
FROM [auth].[UserCredential]
WHERE [UserId] = @userId
ORDER BY [UpdateTimestamp] DESC
;