using BigGrayBison.Authorize.Constants.Internal.Enumerations;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BigGrayBison.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigGrayBison.Authorize.Core
{
    public class Client : IClient
    {
        private readonly ClientData _data;

        public Client(ClientData data)
        {
            _data = data;
        }

        public Guid ClientId => _data.ClientId;

        public ClientType Type => (ClientType)_data.Type;

        public string[] Roles
        {
            get
            {
                DataSerializer serializer = new DataSerializer();
                return serializer.Deserialize<string[]>(_data.Roles);
            }
        }

        public string[] RedirectionUrls
        {
            get
            {
                DataSerializer serializer = new DataSerializer();
                return serializer.Deserialize<string[]>(_data.RedirectionUrls);
            }
        }

        public bool IsActive => _data.IsActive;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public bool IsValidRedirectUrl(string redirectUrl)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(redirectUrl);
            Uri givenUrl = new Uri(redirectUrl);
            Uri matched = RedirectionUrls?
                .Where(u => !string.IsNullOrEmpty(u))
                .Select(u => new Uri(u))
                .FirstOrDefault(u => IsValid(givenUrl, u));
            
            return matched != null;
        }

        private static bool IsValid(Uri givenUrl, Uri validUrl)
        {
            return string.IsNullOrEmpty(givenUrl.Fragment)
                && givenUrl.IsAbsoluteUri
                && string.Equals(givenUrl.Scheme, validUrl.Scheme, StringComparison.OrdinalIgnoreCase)
                && givenUrl.Port == validUrl.Port
                && string.Equals(givenUrl.Host, validUrl.Host, StringComparison.OrdinalIgnoreCase)
                && string.Equals(givenUrl.AbsolutePath, validUrl.AbsolutePath, StringComparison.OrdinalIgnoreCase);
        }
    }
}
