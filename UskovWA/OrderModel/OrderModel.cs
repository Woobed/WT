using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

// моделька для формы на вьюхе
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


    [StringLength(1000, ErrorMessage = "Максимум 1000 символов")]
    public string Description { get; set; }

    public IBrowserFile SelectedFile { get; set; }
}