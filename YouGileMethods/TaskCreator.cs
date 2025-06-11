using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YouGileMethods.Models;
using static System.Net.Mime.MediaTypeNames;

namespace YouGileMethods
{
    // класс для работы с апи
    public class TaskCreator
    {
        // httpclient для работы с RESTApi
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _config;

        public TaskCreator(ApiSettings config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = new HttpClient();
            //авторизация в апи
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _config.apiKey);
            _httpClient.BaseAddress = new Uri(_config.apiUrl);
        }

        // метод проверки авторизации в апи
        public async Task<bool> CheckAuth()
        {
            try
            {
                var response = await _httpClient.GetAsync(String.Empty);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // метод создания таски в апи
        public async Task<ApiResponse> CreateTaskAsync(TaskData task)
        {
            if (string.IsNullOrWhiteSpace(_config.Column))
                throw new ArgumentException("Column ID is required");

            try
            {
                // объект который будет отправлен в апи
                var payload = new
                {
                    task.title,
                    task.description,
                    columnId = _config.Column
                };

                var response = await _httpClient.PostAsJsonAsync("tasks", payload);

                //2 строки для вывода ответа в консоль
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                // проверка статуса ответа
                response.EnsureSuccessStatusCode();
                
                // непосредственно объект ответа апи
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create task", ex);
            }
        }

        // метод для отправки файла (не используется)
        public async Task<FileUploadResponse> UploadFileAsync(byte[] fileContent, string fileName)
        {
            var content = new MultipartFormDataContent();
            var fileContentStream = new ByteArrayContent(fileContent);
            fileContentStream.Headers.ContentType = MediaTypeHeaderValue.Parse(/*"multipart/form-data"*/"application/pdf");
            content.Add(fileContentStream, "file", fileName);
            var response = await _httpClient.PostAsync("upload-file", content);

            Console.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<FileUploadResponse>();

            return result;
        }

        // метод для написания в чат (не используется)
        public async Task SendMessageToChatAsync(string chatId, string fileUrl)
        {
            var payload = new
            {
                text = $"Вот файл: {fileUrl}",
                textHtml = $"<p>Вот файл: <a href=\"{fileUrl}\">{Path.GetFileName(fileUrl)}</a></p>",
                label = "Файл"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"api-v2/chats/{chatId}/messages")
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.apiKey);

            var response = await _httpClient.SendAsync(request);

            Console.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
        }


    }
}
