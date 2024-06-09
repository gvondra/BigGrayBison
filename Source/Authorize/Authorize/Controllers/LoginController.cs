using Authorize.Models;
using BigGrayBison.Authorize.Framework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Authorize.Controllers
{
    // Notice, trying to implement per the OAuth Spec; RFC 6749 (https://oauth.net/2/)
    // Thus, some names do not follow our convensions
    public class LoginController : Controller
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IClientFactory _clientFactory;

        public LoginController(ISettingsFactory settingsFactory, IClientFactory clientFactory)
        {
            _settingsFactory = settingsFactory;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] string response_type,
            [FromQuery] string client_id,
            [FromQuery] string redirect_uri,
            [FromQuery] string state)
        {
            IClient client = null;
            if (!string.IsNullOrEmpty(client_id) && Guid.TryParse(client_id, out Guid clientId))
                client = await _clientFactory.Get(_settingsFactory.CreateCore(), clientId);
            IActionResult result = ValidateRequestParameters(response_type, client, redirect_uri, state);
            if (result == null)
            {
                result = View(new LoginVM
                {
                    ResponseType = response_type,
                    ClientId = client_id,
                    RedirectUrl = redirect_uri,
                    State = state
                });
            }
            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            IClient client = null;
            IActionResult result = null;
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(loginVM.ClientId) && Guid.TryParse(loginVM.ClientId, out Guid clientId))
                    client = await _clientFactory.Get(_settingsFactory.CreateCore(), clientId);
                result = ValidateRequestParameters(loginVM.ResponseType, client, loginVM.RedirectUrl, loginVM.State);
            }
            if (ModelState.IsValid && result == null && client != null)
            {
            }
            return result ?? View(loginVM);
        }

        // much of this is defined in section 4.1.2.1 (https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1)
        [NonAction]
        private IActionResult ValidateRequestParameters(
            string responseType,
            IClient client,
            string redirectUrl,
            string state)
        {
            string errorCode = null;
            IActionResult result = null;
            if (string.IsNullOrEmpty(redirectUrl))
                ModelState.AddModelError(string.Empty, "Missing redirect uri parameter value");
            else if (client == null)
                ModelState.AddModelError(string.Empty, "Missing or invalid client id parameter value");
            else if (!client.IsValidRedirectUrl(redirectUrl))
                ModelState.AddModelError(string.Empty, "Invalid redirect uri parameter value");
            if (ModelState.IsValid)
            {
                if (!string.Equals(responseType ?? string.Empty, "code", System.StringComparison.OrdinalIgnoreCase))
                    errorCode = "unsupported_response_type";
                if (!string.IsNullOrEmpty(errorCode))
                {
                    UriBuilder builder = new UriBuilder(redirectUrl);
                    if (!string.IsNullOrEmpty(builder.Query) && builder.Query.Length > 0)
                        builder.Query += "&";
                    builder.Query += "error=" + HttpUtility.UrlEncode(errorCode);
                    if (!string.IsNullOrEmpty(state))
                        builder.Query += "&" + HttpUtility.UrlEncode(state);
                    result = Redirect(builder.Uri.ToString());
                }
            }
            return result;
        }
    }
}
