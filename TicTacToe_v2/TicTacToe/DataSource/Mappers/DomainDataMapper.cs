namespace TicTacToe.DataSource.Mappers;

using TicTacToe.DataSource.Models;
using TicTacToe.Domain.Models;

public class DomainDataMapper
{
    public static GameBoardDTO GameBoardToDTO(GameBoard gameBoard)
    {
        return new GameBoardDTO { Field = gameBoard.Field };
    }
    public static GameBoard GameBoardToDomain(GameBoardDTO gameBoardEntity)
    {
        return new GameBoard(gameBoardEntity.Field);
    }
    public static CurrentGameDTO CurrentGameToDTO(CurrentGame currentGame, string playerX, string? playerO, int state)
    {
        return new CurrentGameDTO { Uuid = currentGame.Uuid, GameBoard = GameBoardToDTO(currentGame.gameBoard),
                                    PlayerX = playerX, PlayerO = playerO, State = state };
    }
    public static CurrentGame CurrentGameToDomain(CurrentGameDTO currentGameEntity)
    {
        return new CurrentGame(currentGameEntity.GameBoard.Field, currentGameEntity.Uuid);
    }
}