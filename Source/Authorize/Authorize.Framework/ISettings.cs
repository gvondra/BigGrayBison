using System;

namespace BigGrayBison.Authorize.Framework
{
    public interface ISettings : BigGrayBison.Common.Core.ISettings,
        BrassLoon.Interface.Address.ISettings
    {
        Guid? AddressDomainId { get; }
        string EncryptionKeyVaultAddress { get; }
        string SigningKeyVaultAddress { get; }
    }
}
