using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YouGileMethods.Abstractions;
using YouGileMethods.Models;

namespace UskovWA
{
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
