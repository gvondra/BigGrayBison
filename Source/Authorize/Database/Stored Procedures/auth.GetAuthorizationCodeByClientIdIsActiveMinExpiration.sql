CREATE PROCEDURE [auth].[GetAuthorizationCodeByClientIdIsActiveMinExpiration]
	@clientId UNIQUEIDENTIFIER,
	@isActive BIT,
	@minExpiration DATETIME2(4)
AS
SELECT [AuthorizationCodeId], [UserId], [ClientId], [KeyId], [Code], [State], [Expiration],
	[RedirectUrl], [CodeChallenge], [CodeChallengeMethod], [IsActive], [CreateTimestamp], [UpdateTimestamp]
FROM [auth].[AuthorizationCode]
WHERE [ClientId] = @clientId
AND [IsActive] = @isActive
AND [Expiration] >= @minExpiration
;