using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Data;
using TicTacToe.DI;
using TicTacToe.Web;

namespace TicTacToe.Controllers;

[Route("tictactoe/[controller]")]
[ApiController]
public class GameController : Controller {
  private readonly ILogger<GameController> _logger;
  public Configuration config = new();
  public StorageHandler storageHandler;

  public GameController(ILogger<GameController> logger) {
    _logger = logger;
    storageHandler = config.GetStorageHandler();
  }

  public IActionResult Index() { return View(); }

  [Route("new")]
  public IActionResult NewGame() {
    // make async
    if (!ModelState.IsValid) {
      return RedirectToAction("Error", new { code = 400 });
    }
    CurrentGameEntity game = new();
    storageHandler.SaveCurrentGame(game);
    return RedirectToAction("GetGame", new { game.uuid });
  }

  [HttpGet("{uuid}")]
  public ActionResult<CurrentGameEntity> GetGame(string uuid) {
    // pass web entity as param
    // make async
    var game = storageHandler.GetCurrentGameEntity(uuid);
    if (game == null)
      return RedirectToAction("Error", new { code = 404 });
    return View(DomainDataMapper.CurrentGameToDomain(game));
  }

  [HttpPost("{uuid}")]
  public IActionResult UpdateGame(string uuid) {
    // pass web entity, make async
    var gameEntity = storageHandler.GetCurrentGameEntity(uuid);
    if (gameEntity == null)
      return RedirectToAction("Error", new { code = 404 });
    var game = DomainDataMapper.CurrentGameToDomain(gameEntity);

    string? action = Request.Form["action"];
    if (string.IsNullOrEmpty(action))
      return RedirectToAction("GetGame", new { game.uuid });
    _ = int.TryParse(action, out int act);
    int j = act % 100, i = act / 100;

    if (game.IsValid(i, j))
      game.gameBoard.field[i, j] = CurrentGameEntity.player;
    if (!game.IsOver().status)
      gameEntity.gameBoard =
          DomainDataMapper.GameBoardToEntity(game.NextMove());
    return RedirectToAction("GetGame", new { game.uuid });
  }

  [Route("error")]
  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,
                 NoStore = true)]
  public IActionResult Error(int code) {
    string desc;
    if (code == 404)
      desc = "Not found: the page you're looking for doesn't exist.";
    else if (code == 400)
      desc = "Bad request.";
    else
      desc = "Something went wrong.";
    return View(new ErrorViewModel { Code = code, Description = desc });
  }
}
