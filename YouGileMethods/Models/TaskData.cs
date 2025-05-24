public class TaskData
{
    public string title { get; set; }
    public string description { get; set; }
    public string columnId { get; set; } // Теперь вместо location используем columnId
    public string[] assignees { get; set; } // Массив ID исполнителей
    public Dictionary<string, object> customFields { get; set; } // Доп. поля
}