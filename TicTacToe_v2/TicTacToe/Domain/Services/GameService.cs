namespace TicTacToe.Domain.Services;

using TicTacToe.Domain.Models;

public class GameService : IGameService
{
    public const int size = 3, playerX = 1, playerO = 2;

    public GameBoard NextMove(CurrentGame game)
    {
        int row = -1, col = -1, bestVal = 1000;

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                if (game.gameBoard.Field[i, j] == 0)
                {
                    // compute value for this move
                    game.gameBoard.Field[i, j] = playerO;
                    int moveVal = MiniMax(game, game.gameBoard, 0, true, game.Uuid);
                    game.gameBoard.Field[i, j] = 0;
                    if (moveVal < bestVal)
                    {
                        bestVal = moveVal;
                        row = i;
                        col = j;
                    }
                }
            }
        if (row > -1 && col > -1)
            game.gameBoard.Field[row, col] = playerO;

        return game.gameBoard;
    }

    public int MiniMax(CurrentGame game, GameBoard board, int depth, bool isMax, string Uuid)
    {
        var res = IsOver(game);

        if (res.status)
        {
            if (res.winner == playerX)
                return 10 - depth;
            else if (res.winner == playerO)
                return -10 + depth;
            else
                return 0;
        }
        if (isMax)
        {
            // compute maximiser's move
            int best = -10000;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (game.gameBoard.Field[i, j] == 0)
                    {
                        game.gameBoard.Field[i, j] = playerX;
                        // choose max value
                        best = Math.Max(best, MiniMax(game, board, depth + 1, !isMax, Uuid));
                        game.gameBoard.Field[i, j] = 0;
                    }
                }
            }

            return best;
        }
        else
        {
            // compute minimiser's move
            int best = 10000;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (game.gameBoard.Field[i, j] == 0)
                    {
                        game.gameBoard.Field[i, j] = playerO;
                        // choose min value
                        best = Math.Min(best, MiniMax(game, board, depth + 1, !isMax, Uuid));
                        game.gameBoard.Field[i, j] = 0;
                    }
                }
            }

            return best;
        }
    }

    public bool MovesLeft(GameBoard board)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (board.Field[i, j] == 0)
                    return true;
            }
        }

        return false;
    }

    public bool IsValid(CurrentGame game, int row, int col)
    {
        if (row >= 0 && row < size + 1 && col >= 0 && col < size + 1 && game.gameBoard.Field[row, col] == 0)
            return true;

        return false;
    }

    public GameOver IsOver(CurrentGame game)
    {
        var board = game.gameBoard;
        bool over = false;
        int winner = 0;

        // check rows
        for (int row = 0; row < size && !over; row++)
        {
            if (game.gameBoard.Field[row, 0] == game.gameBoard.Field[row, 1] &&
                game.gameBoard.Field[row, 0] == game.gameBoard.Field[row, 2] && game.gameBoard.Field[row, 0] != 0)
            {
                over = true;
                winner = game.gameBoard.Field[row, 0];
            }
        }
        // check columns
        for (int col = 0; col < size && !over; col++)
        {
            if (game.gameBoard.Field[0, col] == game.gameBoard.Field[1, col] &&
                game.gameBoard.Field[0, col] == game.gameBoard.Field[2, col] && game.gameBoard.Field[0, col] != 0)
            {
                over = true;
                winner = game.gameBoard.Field[0, col];
            }
        }
        // check diagonals
        if (game.gameBoard.Field[0, 0] == game.gameBoard.Field[1, 1] &&
            game.gameBoard.Field[0, 0] == game.gameBoard.Field[2, 2] && game.gameBoard.Field[0, 0] != 0)
        {
            over = true;
            winner = game.gameBoard.Field[0, 0];
        }
        if (game.gameBoard.Field[0, 2] == game.gameBoard.Field[1, 1] &&
            game.gameBoard.Field[0, 2] == game.gameBoard.Field[2, 0] && game.gameBoard.Field[0, 2] != 0)
        {
            over = true;
            winner = game.gameBoard.Field[0, 2];
        }
        if (!MovesLeft(board))
            over = true;

        return new GameOver(over, winner);
    }

    public void MovePlayer(CurrentGame game, int row, int col, bool isPlayerX)
    {
        var token = isPlayerX ? playerX : playerO;

        game.gameBoard.Field[row, col] = token;
    }
}