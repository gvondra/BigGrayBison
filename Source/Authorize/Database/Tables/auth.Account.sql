CREATE TABLE [auth].[Account]
(
	[AccountId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(512) NOT NULL,
	[IsActive] BIT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Account_CreateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_Account_UpdateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([AccountId])
)

GO

CREATE UNIQUE INDEX [IX_Account_Name] ON [auth].[Account] ([Name])
