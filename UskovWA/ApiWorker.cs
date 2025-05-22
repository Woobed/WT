using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YouGileMethods.Abstractions;
using YouGileMethods.Models;

namespace UskovWA
{
    public class ApiWorker
    {
        IConfigurationRoot Configuration { get; set; }
        public ApiWorker() { 
            Initialize();
        }

        void Initialize()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public ApiSettings GetApiData(ApiSettings config) {
            return Configuration?.GetSection("ApiSettings")?.Get<ApiSettings>();
        }
    }
}
