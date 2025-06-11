
// данные для создания задания в апи
public class TaskData
{
    public string title { get; set; }
    public string description { get; set; }
    public string[] assignees { get; set; } // Массив ID исполнителей
    public Dictionary<string, object> customFields { get; set; } // Доп. поля
}