CREATE TABLE [auth].[AccountUser]
(
	[AccountId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[IsActive] BIT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_AccountUser_CreateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_AccountUser_UpdateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_AccountUser] PRIMARY KEY NONCLUSTERED ([AccountId]), 
    CONSTRAINT [FK_AccountUser_To_Account] FOREIGN KEY ([AccountId]) REFERENCES [auth].[Account]([AccountId]), 
    CONSTRAINT [FK_AccountUser_To_User] FOREIGN KEY ([UserId]) REFERENCES [auth].[User]([UserId])
)

GO

CREATE NONCLUSTERED INDEX [IX_AccountUser_AccountId] ON [auth].[AccountUser] ([AccountId])

GO

CREATE NONCLUSTERED INDEX [IX_AccountUser_UserId] ON [auth].[AccountUser] ([UserId])
