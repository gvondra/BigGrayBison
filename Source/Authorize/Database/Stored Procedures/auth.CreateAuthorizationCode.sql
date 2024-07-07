CREATE PROCEDURE [auth].[CreateAuthorizationCode]
	@id UNIQUEIDENTIFIER OUT,
	@userId UNIQUEIDENTIFIER,
	@clientId UNIQUEIDENTIFIER,
	@keyId UNIQUEIDENTIFIER,
	@code VARBINARY(1024),
	@state NVARCHAR(64),
	@expiration DATETIME2(4),
	@redirectUrl NVARCHAR(2048),
	@codeChallenge VARBINARY(1024),
	@codeChallengeMethod SMALLINT,
	@isActive BIT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();

	INSERT INTO [auth].[AuthorizationCode] ([AuthorizationCodeId], [UserId], [ClientId], [KeyId], [Code], [State], [Expiration],
	[RedirectUrl], [CodeChallenge], [CodeChallengeMethod], [IsActive], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @userId, @clientId, @keyId, @code, @state, @expiration,
	@redirectUrl, @codeChallenge, @codeChallengeMethod, @isActive, @timestamp, @timestamp);
END