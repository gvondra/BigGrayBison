CREATE PROCEDURE [auth].[GetUserByName]
	@name NVARCHAR(512)
AS
SELECT [UserId], [Name], [EmailAddressId], [IsActive], [CreateTimestamp], [UpdateTimestamp]
FROM [auth].[User]
WHERE [Name] = @name;