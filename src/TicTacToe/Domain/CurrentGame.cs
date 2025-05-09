namespace TicTacToe.Domain;

public class CurrentGame : IService {
  public GameBoard gameBoard = new();
  public string uuid = "";
  public static int player = 1, opponent = 2;
  public CurrentGame() { uuid = Guid.NewGuid().ToString(); }
  public CurrentGame(int[,] field, string uuid) {
    gameBoard = new(field);
    this.uuid = uuid;
  }

  public GameBoard NextMove() {
    int row = -1, col = -1, bestVal = 1000;
    for (int i = 0; i < 3; i++)
      for (int j = 0; j < 3; j++) {
        if (gameBoard.field[i, j] == 0) {
          // compute value for this move
          gameBoard.field[i, j] = opponent;
          int moveVal = MiniMax(gameBoard, 0, true, uuid);
          gameBoard.field[i, j] = 0;
          if (moveVal < bestVal) {
            bestVal = moveVal;
            row = i;
            col = j;
          }
        }
      }
    if (row > -1 && col > -1)
      gameBoard.field[row, col] = opponent;
    return gameBoard;
  }

  public int MiniMax(GameBoard board, int depth, bool isMax, string uuid) {
    var res = IsOver();
    if (res.status) {
      if (res.winner == player)
        return 10 - depth;
      else if (res.winner == opponent)
        return -10 + depth;
      else
        return 0;
    }
    if (isMax) {
      // compute maximiser's move
      int best = -10000;
      for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
          if (gameBoard.field[i, j] == 0) {
            gameBoard.field[i, j] = player;
            // choose max value
            best = Math.Max(best, MiniMax(board, depth + 1, !isMax, uuid));
            gameBoard.field[i, j] = 0;
          }
        }
      }
      return best;
    } else {
      // compute minimiser's move
      int best = 10000;
      for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
          if (gameBoard.field[i, j] == 0) {
            gameBoard.field[i, j] = opponent;
            // choose min value
            best = Math.Min(best, MiniMax(board, depth + 1, !isMax, uuid));
            gameBoard.field[i, j] = 0;
          }
        }
      }
      return best;
    }
  }

  public bool MovesLeft(GameBoard board) {
    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 3; j++) {
        if (board.field[i, j] == 0)
          return true;
      }
    }
    return false;
  }

  public bool IsValid(int row, int col) {
    var board = gameBoard;
    if (row >= 0 && row < 4 && col >= 0 && col < 4 &&
        gameBoard.field[row, col] == 0)
      return true;
    return false;
  }

  public GameOver IsOver() {
    var board = gameBoard;
    bool over = false;
    int winner = 0;
    // check rows
    for (int row = 0; row < 3 && !over; row++) {
      if (gameBoard.field[row, 0] == gameBoard.field[row, 1] &&
          gameBoard.field[row, 0] == gameBoard.field[row, 2] &&
          gameBoard.field[row, 0] != 0) {
        over = true;
        winner = gameBoard.field[row, 0];
      }
    }
    // check columns
    for (int col = 0; col < 3 && !over; col++) {
      if (gameBoard.field[0, col] == gameBoard.field[1, col] &&
          gameBoard.field[0, col] == gameBoard.field[2, col] &&
          gameBoard.field[0, col] != 0) {
        over = true;
        winner = gameBoard.field[0, col];
      }
    }
    // check diagonals
    if (gameBoard.field[0, 0] == gameBoard.field[1, 1] &&
        gameBoard.field[0, 0] == gameBoard.field[2, 2] &&
        gameBoard.field[0, 0] != 0) {
      over = true;
      winner = gameBoard.field[0, 0];
    }
    if (gameBoard.field[0, 2] == gameBoard.field[1, 1] &&
        gameBoard.field[0, 2] == gameBoard.field[2, 0] &&
        gameBoard.field[0, 2] != 0) {
      over = true;
      winner = gameBoard.field[0, 2];
    }
    if (!MovesLeft(board))
      over = true;
    return new GameOver(over, winner);
  }
}