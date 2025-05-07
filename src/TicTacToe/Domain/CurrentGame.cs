namespace TicTacToe.Domain;

public class CurrentGame {
    public GameBoard gameBoard = new();
    public string uuid = "";
    public static int size = 3, player = 1, opponent = 2;
    public CurrentGame() {
        uuid = Guid.NewGuid().ToString();
    }
    public CurrentGame(int[,] field, string uuid) {
        gameBoard = new(field);
        this.uuid = uuid;
    }
}