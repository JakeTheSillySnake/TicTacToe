using System.Collections.Concurrent;

namespace TicTacToe.Data;

public class GameStorage : IRepository {
    public ConcurrentDictionary<string, CurrentGameEntity> storage = [];
    
    public void SaveCurrentGame(CurrentGameEntity currentGameEntity) {
        storage.AddOrUpdate(currentGameEntity.uuid, currentGameEntity, (key, val) => currentGameEntity);
    }
    public CurrentGameEntity GetCurrentGameEntity(string uuid) {
        if (storage.TryGetValue(uuid, out CurrentGameEntity? val)) {
            return val;
        }
        else throw new KeyNotFoundException("Error: game not found");
    }
}