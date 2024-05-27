CREATE PROCEDURE [auth].[GetSigningKey_All]
AS
SELECT [SigningKeyId], [KeyVaultKey], [IsActive], [CreateTimestamp], [UpdateTimestamp]
FROM [auth].[SigningKey]
ORDER BY [UpdateTimestamp]
;