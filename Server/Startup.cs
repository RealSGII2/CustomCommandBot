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

namespace CustomCommandBot.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Make types able to be serialised in the database
            TypeSerialisationRegisterer.SerialiseDatabaseTypes();

            // Start the bot
            DiscordClient.Start();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            var builder = services.AddIdentityServer();
            var key = new RsaSecurityKey(RSA.Create(2048));

            builder.AddSigningCredential(key, SecurityAlgorithms.RsaSha256);

            builder.AddInMemoryApiScopes(IdentityConfig.ApiScopes);
            builder.AddInMemoryClients(IdentityConfig.Clients);

            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,

                        ValidateIssuer = false,
                        ValidIssuer = "vast-issuer",

                        ValidateAudience = false,
                        ValidAudience = "vast-audience",

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromMinutes(60)
                    };
                    options.Configuration = new()
                    {
                        Issuer = "https://localhost:5001"
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

            services.AddControllers();
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
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
