using Azure.Identity;

namespace BigGrayBison.Common.Core
{
    public static class AzureCredential
    {
        public static DefaultAzureCredential DefaultAzureCredential { get; } = CreateDefaultAzureCredential();

        private static DefaultAzureCredential CreateDefaultAzureCredential() => new DefaultAzureCredential(GetDefaultAzureCredentialOptions());

        private static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions()
        {
            return new DefaultAzureCredentialOptions()
            {
                ExcludeAzureCliCredential = false,
                ExcludeAzurePowerShellCredential = false,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeEnvironmentCredential = false,
                ExcludeManagedIdentityCredential = false,
                ExcludeVisualStudioCodeCredential = false,
                ExcludeVisualStudioCredential = false
            };
        }
    }
}
