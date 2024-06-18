CREATE PROCEDURE [auth].[GetUserNameAvailable]
	@name NVARCHAR(512)
AS
	SELECT [auth].[IsUserNameAvailable](@name) [IsAvailable];
RETURN 0
