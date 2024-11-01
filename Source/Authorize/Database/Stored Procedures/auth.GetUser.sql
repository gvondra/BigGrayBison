﻿CREATE PROCEDURE [auth].[GetUser]
	@id UNIQUEIDENTIFIER
AS
SELECT [UserId], [Name], [EmailAddressId], [IsActive], [Roles], [CreateTimestamp], [UpdateTimestamp]
FROM [auth].[User]
WHERE [UserId] = @id;