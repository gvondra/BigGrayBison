using System;

namespace Authorize
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public bool EnableDatabaseAccessToken { get; set; }
        public Guid? LoginClientId { get; set; }
        public string LoginRedirectUrl { get; set; }
        public Guid? AddressDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string EncryptionKeyVaultAddress { get; set; }
        public string SigningKeyVaultAddress { get; set; }
        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string BrassLoonAddressApiBaseAddress { get; set; }
        public string TokenIssuer { get; set; }
    }
}
