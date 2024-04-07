/*
Run this script as part of system setup/installation add an initail user
*/

DECLARE @id UNIQUEIDENTIFIER;
DECLARE @name NVARCHAR(512) = 'setup-user'
DECLARE @emialAddressId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
DECLARE @isActive BIT = 1;
DECLARE @timestamp DATETIME2(4);
DECLARE @credentialExpiration DATETIME2(1) = DATEADD(day, 14, SYSUTCDATETIME());

IF [auth].[IsUserNameAvailable](@name) = 1
BEGIN
    EXEC [auth].[CreateUser] @id out, @name, @emialAddressId, @isActive, null, null, @credentialExpiration, @timestamp out;
END