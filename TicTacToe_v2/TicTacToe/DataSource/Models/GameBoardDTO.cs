using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.DataSource.Models;

[ComplexType]
public class GameBoardDTO
{
    public int[,] Field { get; set; } = new int[3, 3];
}