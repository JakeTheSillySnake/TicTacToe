namespace TicTacToe.Data;

using TicTacToe.Domain;

public class StorageHandler(IRepository storage) : IService {
    public IRepository storage = storage;

    public GameBoard NextMove(string uuid) {
        CurrentGameEntity game = storage.GetCurrentGameEntity(uuid);
        //
        // minmax algo
        //
        return DomainDataMapper.GameBoardToDomain(game.gameBoard);
    }
    public bool IsValid(string uuid) {
        CurrentGameEntity game = storage.GetCurrentGameEntity(uuid);
        return true;
        // check prev moves + valid moves
    }
    public bool IsOver(string uuid) {
        CurrentGameEntity game = storage.GetCurrentGameEntity(uuid);
        /*for (int i = 0; i < CurrentGameEntity.size; i++) {
            for (int j = 0; j < CurrentGameEntity.size; j++) {

            }
        }*/
        return false;
        // check if over
    }
}