namespace TicTacToe.Data;

public interface IRepository {
    public void SaveCurrentGame(CurrentGameEntity currentGameEntity);
    public CurrentGameEntity GetCurrentGameEntity(string uuid);
}