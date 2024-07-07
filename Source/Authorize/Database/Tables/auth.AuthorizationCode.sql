CREATE TABLE [auth].[AuthorizationCode]
(
	[AuthorizationCodeId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[ClientId] UNIQUEIDENTIFIER NOT NULL,
	[KeyId] UNIQUEIDENTIFIER NOT NULL,
	[Code] VARBINARY(1024) NOT NULL,
	[State] NVARCHAR(64) NOT NULL,
	[Expiration] DATETIME2(4) NOT NULL,
	[RedirectUrl] NVARCHAR(2048) NOT NULL,
	[CodeChallenge] VARBINARY(1024) NULL,
	[CodeChallengeMethod] SMALLINT NOT NULL,
	[IsActive] BIT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_AuthorizationCode_CreateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_AuthorizationCode_UpdateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_AuthorizationCode] PRIMARY KEY CLUSTERED ([AuthorizationCodeId]), 
    CONSTRAINT [FK_AuthorizationCode_To_User] FOREIGN KEY ([UserId]) REFERENCES [auth].[User]([UserId]), 
    CONSTRAINT [FK_AuthorizationCode_To_Client] FOREIGN KEY ([ClientId]) REFERENCES [auth].[Client]([ClientId])
)

GO

CREATE NONCLUSTERED INDEX [IX_AuthorizationCode_UserId] ON [auth].[AuthorizationCode] ([UserId])

GO

CREATE NONCLUSTERED INDEX [IX_AuthorizationCode_ClientId] ON [auth].[AuthorizationCode] ([ClientId]) INCLUDE ([Expiration], [IsActive])
