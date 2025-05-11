namespace TicTacToe.DI;

using TicTacToe.Data;

public class Configuration {
  public GameStorage storage;
  public StorageHandler storageHandler;
  public Configuration() {
    storage = GameStorage.GetInstance();
    storageHandler = new(storage);
  }
  public StorageHandler GetStorageHandler() { return storageHandler; }
}