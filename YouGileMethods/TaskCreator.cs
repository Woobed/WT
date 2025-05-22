using System.Text;
using System.Text.Json;
using YouGileMethods.Models;

namespace YouGileMethods
{
    public class TaskCreator
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _config;

        public TaskCreator(ApiSettings config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse> CreateTaskAsync(TaskData task)
        {
            // Валидация входных параметров
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (string.IsNullOrWhiteSpace(task.title))
            {
                throw new ArgumentException("Title is required", nameof(task.title));
            }

            if (string.IsNullOrWhiteSpace(task.location))
            {
                throw new ArgumentException("Location is required", nameof(task.location));
            }

            try
            {
                // Настройка запроса
                var request = new HttpRequestMessage(HttpMethod.Post, "https://yougile.com/data/api-v1/tasks");
                request.Headers.Add("Authorization", $"YOUGILE-KEY {_config.apiKey}");
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Сериализация данных
                var jsonContent = JsonSerializer.Serialize(task);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Отправка запроса
                using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"API request failed: {response.StatusCode}. {responseBody}");
                    }

                    return JsonSerializer.Deserialize<ApiResponse>(responseBody)
                        ?? throw new InvalidOperationException("Failed to deserialize API response");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create task in YouGile", ex);
            }
        }
    }
}