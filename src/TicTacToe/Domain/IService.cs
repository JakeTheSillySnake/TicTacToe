namespace TicTacToe.Domain;

public interface IService {
    public GameBoard NextMove(string uuid);
    public bool IsValid(string uuid);
    public bool IsOver(string uuid);
}