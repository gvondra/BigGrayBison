using BigGrayBison.Authorize.Framework;
using BrassLoon.JwtUtility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Core
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly IAuthorizationCodeFactory _authorizationCodeFactory;
        private readonly ISigningKeyFactory _signingKeyFactory;

        public AccessTokenGenerator(
            IAuthorizationCodeFactory authorizationCodeFactory,
            ISigningKeyFactory signingKeyFactory)
        {
            _authorizationCodeFactory = authorizationCodeFactory;
            _signingKeyFactory = signingKeyFactory;
        }

        public async Task<JwtSecurityToken> GenerateForAuthorizationCode(ISettings settings, Guid clientId, string code)
        {
            JwtSecurityToken result = null;
            IAuthorizationCode authorizationCode = await _authorizationCodeFactory.GetActiveByCode(settings, clientId, code);
            if (authorizationCode != null)
            {
                IUser user = await authorizationCode.GetUser(settings);
                List<Claim> claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, (await user.GetEmailAddress(settings))),
                    new Claim(JwtRegisteredClaimNames.Name, user.Name)
                };
                ISigningKey signingKey = await _signingKeyFactory.GetActive(settings);
                RsaSecurityKey rsaSecurityKey = await signingKey.GetKey(settings, true);
                result = JwtSecurityTokenUtility.Create(rsaSecurityKey, settings.TokenIssuer, settings.TokenIssuer, claims, GetExpiration);
            }
            return result;
        }

        private static DateTime GetExpiration() => DateTime.UtcNow.AddHours(6);
    }
}
