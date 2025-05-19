namespace TicTacToe.DataSource;

public class StorageHandler(GameStorage storage) : IRepository {
  private GameStorage _storage = storage;

  public async Task SaveCurrentGame(CurrentGameEntity currentGameEntity) {
    await Task.Run(() => {
      _storage.games.AddOrUpdate(currentGameEntity.uuid, currentGameEntity,
                                 (key, val) => currentGameEntity);
    });
  }
  public async Task<CurrentGameEntity?> GetCurrentGameEntity(string uuid) {
    CurrentGameEntity? val = null;
    await Task.Run(() => {
     if (_storage.games.TryGetValue(uuid, out CurrentGameEntity? game))
       val = game;
    });
    return val;
  }
}