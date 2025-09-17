namespace TicTacToe.DataSource;

public interface IRepository {
  public Task SaveCurrentGame(CurrentGameEntity currentGameEntity);
  public Task<CurrentGameEntity?> GetCurrentGameEntity(string uuid);
}