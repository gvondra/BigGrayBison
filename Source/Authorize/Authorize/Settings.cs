using System;

namespace Authorize
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public bool EnableDatabaseAccessToken { get; set; }
        public string LoginClientId { get; set; }
        public string LoginRedirectUrl { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
    }
}
