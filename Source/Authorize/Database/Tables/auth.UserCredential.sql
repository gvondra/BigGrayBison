CREATE TABLE [auth].[UserCredential]
(
	[UserCredentialId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[MasterKey] UNIQUEIDENTIFIER NULL,
	[SecretSalt] BINARY(16) NULL,
	[SecretKey] VARBINARY(256) NULL,
	[Secret] VARBINARY(2024) NULL,
	[IsActive] BIT NOT NULL,
	[Expiration] DATETIME2(1) NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_UserCredential_CreateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_UserCredential_UpdateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_UserCredential] PRIMARY KEY NONCLUSTERED ([UserCredentialId]), 
    CONSTRAINT [FK_UserCredential_To_User] FOREIGN KEY ([UserId]) REFERENCES [auth].[User]([UserId])
)

GO

CREATE CLUSTERED INDEX [IX_UserCredential_UserId] ON [auth].[UserCredential] ([UserId])
