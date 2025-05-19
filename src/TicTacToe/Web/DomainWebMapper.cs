namespace TicTacToe.Web;

using TicTacToe.Domain;

public class DomainWebMapper {
  public static GameBoardWebEntity GameBoardToEntity(GameBoard gameBoard) {
    return new GameBoardWebEntity(gameBoard.field);
  }
  public static GameBoard
  GameBoardToDomain(GameBoardWebEntity gameBoardEntity) {
    return new GameBoard(gameBoardEntity.field);
  }
  public static CurrentGameWebEntity
  CurrentGameToEntity(CurrentGame currentGame) {
    var res = currentGame.IsOver();
    return new CurrentGameWebEntity(currentGame.gameBoard.field,
                                    currentGame.uuid, res.status, res.winner);
  }
  public static CurrentGame
  CurrentGameToDomain(CurrentGameWebEntity currentGameEntity) {
    return new CurrentGame(currentGameEntity.gameBoard.field,
                           currentGameEntity.uuid);
  }
}