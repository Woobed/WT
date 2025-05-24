using System.Net.Http.Headers;
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

        public async Task<ApiResponse> CreateTaskWithAttachmentAsync(TaskData task, byte[] fileContent = null, string fileName = null)
        {
            // Создаем задачу
            var taskResponse = await CreateTaskAsync(task);

            // Если есть файл - прикрепляем его
            if (fileContent != null && fileContent.Length > 0 && !string.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    // Загружаем файл
                    var fileUploadResponse = await UploadFileAsync(fileContent, fileName);

                    // Прикрепляем файл к задаче
                    await AttachFileToTaskAsync(taskResponse.id, fileUploadResponse.id);

                    // Можно добавить информацию о файле в ответ
                    taskResponse.fileId = fileUploadResponse.id;
                }
                catch (Exception ex)
                {
                    // Логируем ошибку, но не прерываем выполнение
                    Console.WriteLine($"Warning: Failed to attach file to task. {ex.Message}");
                }
            }

            return taskResponse;
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


        

        public async Task<FileUploadResponse> UploadFileAsync(byte[] fileContent, string fileName)
        {
            if (fileContent == null || fileContent.Length == 0)
                throw new ArgumentException("File content is empty", nameof(fileContent));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name is required", nameof(fileName));

            try
            {
                using var content = new MultipartFormDataContent();
                var fileContentStream = new ByteArrayContent(fileContent);
                fileContentStream.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                content.Add(fileContentStream, "file", fileName);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://yougile.com/data/api-v1/files/upload")
                {
                    Content = content
                };
                request.Headers.Add("Authorization", $"YOUGILE-KEY {_config.apiKey}");

                using var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"File upload failed: {response.StatusCode}. {responseBody}");

                return JsonSerializer.Deserialize<FileUploadResponse>(responseBody)
                    ?? throw new InvalidOperationException("Failed to deserialize file upload response");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to upload file to YouGile", ex);
            }
        }
        public async Task AttachFileToTaskAsync(string taskId, string fileId)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentException("Task ID is required", nameof(taskId));

            if (string.IsNullOrWhiteSpace(fileId))
                throw new ArgumentException("File ID is required", nameof(fileId));

            try
            {
                var payload = new { taskId, fileId };
                var jsonContent = JsonSerializer.Serialize(payload);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://yougile.com/data/api-v1/tasks/attach")
                {
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"YOUGILE-KEY {_config.apiKey}");

                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to attach file: {response.StatusCode}. {errorBody}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to attach file to task", ex);
            }
        }
    }
}