using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using CustomCommandBot.Shared.Models.Discord;
using IdentityServer4.Extensions;
using System.Collections.Generic;
using CustomCommandBot.Server.Bot;

namespace CustomCommandBot.Server.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly string provider = "Discord";
        private readonly string returnUrl = "https://localhost:5001/auth/callback";

        private DiscordClient client;

        public AuthController(DiscordClient _client)
        {
            client = _client;
        }

        private readonly IReadOnlyList<string> ClaimsToSync =
            new List<string>()
            {
                "urn:discord:avatar:url",
                "urn:discord:user:discriminator",
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/guilds"
            };

        [Route("@me")]
        [HttpGet]
        [Authorize]
        public ActionResult<User> GetCurrentUser()
        {
            return new(HttpContext.User.GetDiscordUser(client.GetGuildIds()));
        }

        [Route("login")]
        [HttpGet]
        public IActionResult LogInGet()
        {
            var callbackUrl = Url.Action("ExternalLoginCallback");

            AuthenticationProperties authParams = new()
            {
                RedirectUri = callbackUrl,
                Items =
                {
                    { "scheme", provider },
                    { "returnUrl", returnUrl }
                }
            };

            return Challenge(authParams, provider);
        }

        [Route("externallogincallback")]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var externalUser = result.Principal;
            if (externalUser == null)
            {
                throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var claims = externalUser.Claims.ToList();

            // try to determine the unique id of the external user - the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("Unknown userid");
            }

            var externalUserId = userIdClaim.Value;
            var externalProvider = userIdClaim.Issuer;

            var user = Shared.Models.Discord.User.FromClaims(claims, client.GetGuildIds());

            List<Claim> additionalClaims = new();

            foreach (string claimName in ClaimsToSync)
            {
                var claim = claims.FirstOrDefault(e => e.Type == claimName);

                if (claim is not null)
                    additionalClaims.Add(claim);
            }

            var identity = new IdentityServerUser(user.Id)
            {
                DisplayName = user.Username,
                IdentityProvider = provider,
                AuthenticationTime = DateTime.Now,
                AdditionalClaims = additionalClaims
            };

            
            var principal = identity.CreatePrincipal();

            /*foreach (var addedClaim in ClaimsToSync)
            {
                var userClaim = claims
                    .FirstOrDefault(c => c.Type == addedClaim);

                if (principal.HasClaim(c => c.Type == addedClaim.Key))
                {
                    var externalClaim = principal.FindFirst(addedClaim);

                    if (userClaim == null)
                    {
                        await principal.AddClaimAsync(user,
                            new Claim(addedClaim.Key, externalClaim.Value));
                        refreshSignIn = true;
                    }
                    else if (userClaim.Value != externalClaim.Value)
                    {
                        await _userManager
                            .ReplaceClaimAsync(user, userClaim, externalClaim);
                        refreshSignIn = true;
                    }
                }
                else if (userClaim == null)
                {
                    // Fill with a default value
                    await _userManager.AddClaimAsync(user, new Claim(addedClaim.Key,
                        addedClaim.Value));
                    refreshSignIn = true;
                }
            }*/

            await HttpContext.SignInAsync(principal);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // validate return URL and redirect back to authorization endpoint or a local page
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }
    }
}
