using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Tinder.AutoMatcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var tinderAuthToken = hostContext.Configuration.GetValue<string>("TinderClient:Token"); // X-Auth-Token
            var tinderLocation = JsonSerializer.Deserialize<Models.Geolocation>(hostContext.Configuration.GetValue<string>("TinderClient:Location"));
            
            if (string.IsNullOrEmpty(tinderAuthToken) || !Guid.TryParse(tinderAuthToken, out Guid tinderAuthTokenGuid))
            {
                throw new Exception("TinderClient:Token is missing in appsettings.json or the token is malformed");
            }
            var tinderClient = new TinderClient(tinderAuthTokenGuid, tinderLocation!);

            services.AddSingleton<ITinderClient>(tinderClient);
            services.AddHostedService<Worker>();
        }

        private static string GetAPIToken()
        {
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://tinder.com/");
            //var tokenResponse = driver.SendCommand("localStorage.getItem(\"TinderWeb/APIToken\""));
          
            return "";
        }
    }
}
