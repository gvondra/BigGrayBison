using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IAccessTokenGenerator
    {
        Task<JwtSecurityToken> GenerateForAuthorizationCode(ISettings settings, Guid clientId, string code);
    }
}
