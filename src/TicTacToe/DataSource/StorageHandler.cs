namespace TicTacToe.Data;

public class StorageHandler : IRepository {
  private GameStorage _storage;

  public StorageHandler(GameStorage storage) { _storage = storage; }
  public void SaveCurrentGame(CurrentGameEntity currentGameEntity) {
    _storage.games.AddOrUpdate(currentGameEntity.uuid, currentGameEntity,
                               (key, val) => currentGameEntity);
  }
  public CurrentGameEntity? GetCurrentGameEntity(string uuid) {
    if (_storage.games.TryGetValue(uuid, out CurrentGameEntity? val))
      return val;
    else
      return null;
  }
}