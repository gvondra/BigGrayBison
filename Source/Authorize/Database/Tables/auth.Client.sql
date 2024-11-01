﻿CREATE TABLE [auth].[Client]
(
	[ClientId] UNIQUEIDENTIFIER NOT NULL,
	[Type] SMALLINT NOT NULL,
	[SecretKey] UNIQUEIDENTIFIER NULL,
	[SecretSalt] BINARY(16) NULL,
	[Roles] NVARCHAR(2048) NOT NULL,
	[RedirectionUrls] NVARCHAR(3072) NOT NULL,
	[IsActive] BIT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Client_CreateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_Client_UpdateTimestamp] DEFAULT(SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([ClientId])
)
