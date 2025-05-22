
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

public class OrderModel
{
    [Required(ErrorMessage = "ФИО обязательно")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Телефон обязателен")]
    [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Некорректный формат телефона")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; }

    public IBrowserFile SelectedFile { get; set; }
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}