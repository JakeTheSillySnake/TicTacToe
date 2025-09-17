namespace TicTacToe.DataSource;

using TicTacToe.Domain;

public class DomainDataMapper {
  public static GameBoardEntity GameBoardToEntity(GameBoard gameBoard) {
    return new GameBoardEntity(gameBoard.field);
  }
  public static GameBoard GameBoardToDomain(GameBoardEntity gameBoardEntity) {
    return new GameBoard(gameBoardEntity.field);
  }
  public static CurrentGameEntity CurrentGameToEntity(CurrentGame currentGame) {
    return new CurrentGameEntity(currentGame.gameBoard.field, currentGame.uuid);
  }
  public static CurrentGame
  CurrentGameToDomain(CurrentGameEntity currentGameEntity) {
    return new CurrentGame(currentGameEntity.gameBoard.field,
                           currentGameEntity.uuid);
  }
}