using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouGileMethods.Abstractions;

namespace YouGileMethods.Models
{
    public class ApiSettings : ISettings
    {
        public string apiKey { get; set; }
        public string apiUrl { get; set; }
    }
}
