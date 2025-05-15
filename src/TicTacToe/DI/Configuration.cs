namespace TicTacToe.DI;

using TicTacToe.DataSource;

public class Configuration {
  public GameStorage storage;
  public StorageHandler storageHandler;
  public Configuration() {
    storage = GameStorage.GetInstance();
    storageHandler = new(storage);
  }
  public StorageHandler GetStorageHandler() { return storageHandler; }
}