using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicTacToe.DataSource.Models;

[Index(nameof(Login), IsUnique = true)]
public class UserDTO
{
    [Key]
    public string Uuid { get; set; } = "";
    [Required] public string Login { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}