using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using CustomCommandBot.Server.Bot;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace CustomCommandBot.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Make types able to be serialised in the database
            TypeSerialisationRegisterer.SerialiseDatabaseTypes();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
            services.AddRazorPages();

            var builder = services.AddIdentityServer();
            var key = new RsaSecurityKey(RSA.Create(2048));

            builder.AddSigningCredential(key, SecurityAlgorithms.RsaSha256);

            builder.AddInMemoryApiScopes(IdentityConfig.ApiScopes);
            builder.AddInMemoryClients(IdentityConfig.Clients);

            // accepts any access token issued by identity server
            /*services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,

                        ValidateIssuer = false,
                        ValidIssuer = "https://localhost:5001",

                        ValidateAudience = false,
                        ValidAudience = "ccb-audience",

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromMinutes(60)
                    };
                    options.Configuration = new()
                    {
                        Issuer = "https://localhost:5001"
                    };
                });*/

            services.AddAuthentication()
                .AddDiscord(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = "786365688482103326";
                    options.ClientSecret = "LipzSYw69odyFNvW_8nrhq27y1kdR2pn";

                    options.Scope.Add("guilds");

                    options.SaveTokens = true;

                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
                    options.ClaimActions.MapJsonKey("urn:discord:avatar:url", "avatar", ClaimValueTypes.String);
                    options.ClaimActions.MapJsonKey("urn:discord:user:discriminator", "discriminator", ClaimValueTypes.Integer);

                    options.Events.OnCreatingTicket = async ctx =>
                    {
                        List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                        ctx.Identity.AddClaim(await GetGuildClaims(ctx));

                        tokens.Add(new AuthenticationToken()
                        {
                            Name = "TicketCreated",
                            Value = DateTime.UtcNow.ToString()
                        });

                        ctx.Properties.StoreTokens(tokens);
                    };
                });

            // adds an authorization policy to make sure the token is for scope 'api1'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireClaim("scope", "api1");
                });
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
                    options.AllowInputFormatterExceptionMessages = false;
                });

            services.AddSingleton(sp => new DiscordClient());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Temporary for testing
            app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always });

            //app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            // app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                    //.RequireAuthorization("ApiScope");
                endpoints.MapFallbackToFile("index.html");
            });
        }

        // Thanks to jeremywhittington on GitHub:
        // https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/209#issuecomment-354569545
        private async Task<Claim> GetGuildClaims(OAuthCreatingTicketContext context)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/users/@me/guilds");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("failed to get guilds");
            }

            var payload = JArray.Parse(await response.Content.ReadAsStringAsync());
            Claim claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/guilds", payload.ToString(), ClaimValueTypes.String);
            return claim;
        }
    }
}
