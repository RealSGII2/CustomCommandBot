using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace CustomCommandBot.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            HttpClientHandler handler = new()
            {
                AllowAutoRedirect = false
            };
            HttpClient client = new(handler);

            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest() { 
                Address = "https://localhost:5001"
            });
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
