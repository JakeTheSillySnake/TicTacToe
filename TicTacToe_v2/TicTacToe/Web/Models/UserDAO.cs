using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Web.Models;

[Index(nameof(Login), IsUnique = true)]
public class UserDAO
{
    [Key]
    public string Uuid { get; set; } = "";
    [Required] public string Login { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}