CREATE FUNCTION [auth].[IsUserNameAvailable]
(
	@name NVARCHAR(512)
)
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT = 1;
	IF EXISTS (SELECT TOP 1 1 FROM [auth].[User] WHERE [Name] = @name)
	OR EXISTS (SELECT TOP 1 1 FROM [auth].[Account] WHERE [Name] = @name)
	BEGIN
		SET @result = 0;
	END
	RETURN @result;
END
