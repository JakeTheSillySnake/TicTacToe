using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Domain;
using TicTacToe.Data;

namespace TicTacToe.Controllers;

public class HomeController : Controller {
  private readonly ILogger<HomeController> _logger;
  public StorageHandler storageHandler = new();
  public string currId = "";

  public HomeController(ILogger<HomeController> logger) {
    _logger = logger;
    CurrentGameEntity game = new();
    game.gameBoard.field[0, 0] = CurrentGameEntity.player;
    game.gameBoard.field[0, 1] = CurrentGameEntity.player;
    game.gameBoard.field[0, 2] = CurrentGameEntity.player;

    game.gameBoard.field[1, 1] = CurrentGameEntity.opponent;
    game.gameBoard.field[2, 1] = CurrentGameEntity.opponent;
    game.gameBoard.field[1, 2] = CurrentGameEntity.opponent;
    currId = game.uuid;
    storageHandler.SaveCurrentGame(game);
  }

  public IActionResult Game() {
    return View(storageHandler.GetCurrentGameEntity(currId).gameBoard);
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,
                 NoStore = true)]
  public IActionResult Error() {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ??
                                                 HttpContext.TraceIdentifier });
  }
}
