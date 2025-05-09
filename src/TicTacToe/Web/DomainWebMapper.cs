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
    return new CurrentGameWebEntity(currentGame.gameBoard.field,
                                    currentGame.uuid);
  }
  public static CurrentGame
  CurrentGameToDomain(CurrentGameWebEntity currentGameEntity) {
    return new CurrentGame(currentGameEntity.gameBoard.field,
                           currentGameEntity.uuid);
  }
}