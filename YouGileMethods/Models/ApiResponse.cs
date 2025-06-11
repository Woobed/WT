using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YouGileMethods.Models
{
    // моделька для получения ответа от апи
    public class ApiResponse
    {
        public string id { get; set; }
        public string title { get; set; }
        public string fileId { get; set; } // Для прикрепленных файлов
        public string chatId { get; set; }  
    }
}
