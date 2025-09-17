namespace TicTacToe.Web.Mappers;

using TicTacToe.Domain.Models;
using TicTacToe.Web.Models;

public class DomainWebMapper
{
    public static GameBoardDAO GameBoardToDAO(GameBoard gameBoard)
    {
        return new GameBoardDAO(gameBoard.Field);
    }
    public static GameBoard GameBoardToDomain(GameBoardDAO gameBoardEntity)
    {
        return new GameBoard(gameBoardEntity.Field);
    }
    public static CurrentGameDAO CurrentGameToDAO(CurrentGame currentGame)
    {
        return new CurrentGameDAO(currentGame.gameBoard.Field, currentGame.Uuid);
    }
    public static CurrentGame CurrentGameToDomain(CurrentGameDAO currentGameEntity)
    {
        return new CurrentGame(currentGameEntity.GameBoard.Field, currentGameEntity.Uuid);
    }
}