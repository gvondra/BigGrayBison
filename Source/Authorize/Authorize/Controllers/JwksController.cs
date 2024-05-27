using BigGrayBison.Authorize.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwksController : ControllerBase
    {
        ILogger<JwksController> _logger;
        ISettingsFactory _settingsFactory;
        ISigningKeyFactory _signingKeyFactory;
        ISigningKeySaver _signingKeySaver;

        public JwksController(
            ILogger<JwksController> logger,
            ISettingsFactory settingsFactory,
            ISigningKeyFactory signingKeyFactory,
            ISigningKeySaver signingKeySaver)
        {
            _logger = logger;
            _settingsFactory = settingsFactory;
            _signingKeyFactory = signingKeyFactory;
            _signingKeySaver = signingKeySaver;
        }

        [HttpGet]
        [ResponseCache(Duration = 150, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Get()
        {
            IActionResult result = null;
            try
            {
                CoreSettings settings = _settingsFactory.CreateCore();
                IEnumerable<ISigningKey> signingKeys = await _signingKeyFactory.GetAll(settings);
                var jsonWebKeySet = new { Keys = new List<object>() };
                foreach (ISigningKey signingKey in signingKeys.Where(sk => sk.IsActive))
                {
                    jsonWebKeySet.Keys.Add(
                        await CreateJsonWebKey(settings, signingKey));
                }
                if (jsonWebKeySet.Keys.Count == 0)
                {
                    ISigningKey signingKey = _signingKeyFactory.Create();
                    await _signingKeySaver.Create(settings, signingKey);
                    jsonWebKeySet.Keys.Add(
                        await CreateJsonWebKey(settings, signingKey));
                }
                result = Content(
                    JsonConvert.SerializeObject(jsonWebKeySet, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore }),
                    "appliation/json");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return result;
        }

        [NonAction]
        private static async Task<JsonWebKey> CreateJsonWebKey(CoreSettings settings, ISigningKey signingKey)
        {
            RsaSecurityKey rsaSecurityKey = await signingKey.GetKey(settings, false);
            JsonWebKey jsonWebKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);
            jsonWebKey.KeyId = signingKey.SigningKeyId.ToString("N");
            jsonWebKey.Alg = "RS512";
            jsonWebKey.Use = "sig";
            return jsonWebKey;
        }
    }
}
