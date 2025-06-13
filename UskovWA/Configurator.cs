using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YouGileMethods.Models;

namespace UskovWA
{
    // конфигуратор который тянет объект ApiSettings из appsettings
    public static class Configurator
    {
        public static ApiSettings GetApiData() {
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            return Configuration?.GetSection("ApiSettings")?.Get<ApiSettings>();
        }
    }
}
