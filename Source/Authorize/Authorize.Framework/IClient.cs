using BigGrayBison.Authorize.Constants.Internal.Enumerations;
using System;

namespace BigGrayBison.Authorize.Framework
{
    public interface IClient
    {
        Guid ClientId { get; }
        ClientType Type { get; }
        string[] Roles { get; }
        string[] RedirectionUrls { get; }
        bool IsActive { get; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }

        bool IsValidRedirectUrl(string redirectUrl);
    }
}
