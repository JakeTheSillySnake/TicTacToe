using System.ComponentModel.DataAnnotations;
using TicTacToe.DataSource.Enum;

namespace TicTacToe.DataSource.Models;

public class CurrentGameDTO
{
    [Key]
    public string Uuid { get; set; } = "";
    public GameBoardDTO GameBoard { get; set; } = new();
    public string PlayerX { get; set; } = "";
    public string? PlayerO { get; set; }
    public int State { get; set; } = (int)GameStates.WAIT;
}