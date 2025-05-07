using System.Drawing;

namespace TicTacToe.Web;

public class CurrentGameWebEntity {
    public GameBoardWebEntity gameBoard = new();
    public string uuid = "";
    public static int size = 3, player = 1, opponent = 2;
    public CurrentGameWebEntity() {
        uuid = Guid.NewGuid().ToString();
    }
    public CurrentGameWebEntity(int[,] field, string uuid) {
        gameBoard = new(field);
        this.uuid = uuid;
    }
}