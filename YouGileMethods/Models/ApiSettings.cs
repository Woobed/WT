using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouGileMethods.Models
{
    // моделька для вытягивания данных из appsettings
    public class ApiSettings
    {
        public string apiKey { get; set; }
        public string apiUrl { get; set; }
        public string Column { get; set; }

    }
}
