CREATE PROCEDURE [auth].[ActivateAccoutUser]
	@accountId UNIQUEIDENTIFIER,
	@userId UNIQUEIDENTIFIER,
	@isActive BIT = 1,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [auth].[AccountUser]
	SET [IsActive] = @isActive,
	[UpdateTimestamp] = @timestamp
	WHERE [AccountId] = @accountId
	AND [UserId] = @userId;

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO [auth].[AccountUser] ([AccountId], [UserId], [IsActive], [CreateTimestamp], [UpdateTimestamp])
		VALUES (@accountId, @userId, @isActive, @timestamp, @timestamp);
	END
END