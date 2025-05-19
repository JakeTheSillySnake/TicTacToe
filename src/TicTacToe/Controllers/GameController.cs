using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.DataSource;
using TicTacToe.DI;
using TicTacToe.Web;

namespace TicTacToe.Controllers;

[Route("game")]
[ApiController]
public class GameController : Controller {
  private readonly ILogger<GameController> _logger;
  public Configuration config = new();
  public IRepository storageHandler;

  public GameController(ILogger<GameController> logger) {
    _logger = logger;
    storageHandler = config.GetStorageHandler();
  }

  public IActionResult Index() { return View(); }

  [Route("new")]
  public async Task<IActionResult> NewGame() {
    if (!ModelState.IsValid) {
      return RedirectToAction("Error", new { code = 400 });
    }
    CurrentGameEntity game = new();
    await storageHandler.SaveCurrentGame(game);
    return RedirectToAction("GetGame", new { game.uuid });
  }

  [HttpGet("{uuid}")]
  public async Task<ActionResult<CurrentGameEntity>> GetGame(string uuid) {
    var gameEntity = await storageHandler.GetCurrentGameEntity(uuid);
    if (gameEntity == null)
      return RedirectToAction("Error", new { code = 404 });
    var game = DomainDataMapper.CurrentGameToDomain(gameEntity);
    return View(DomainWebMapper.CurrentGameToEntity(game));
  }

  [HttpPost("{uuid}")]
  public async Task<IActionResult>
  UpdateGame([FromForm] CurrentGameWebEntity currentGameWeb) {
    var gameEntity =
        await storageHandler.GetCurrentGameEntity(currentGameWeb.uuid);
    if (gameEntity == null)
      return RedirectToAction("Error", new { code = 404 });

    var game = DomainDataMapper.CurrentGameToDomain(gameEntity);
    string? action = currentGameWeb.action;
    if (string.IsNullOrEmpty(action))
      return RedirectToAction("GetGame", new { game.uuid });

    _ = int.TryParse(action, out int act);
    int j = act % 100, i = act / 100;
    if (game.IsValid(i, j))
      game.gameBoard.field[i, j] = CurrentGameEntity.player;
    else
      return RedirectToAction("Error", new { code = 400 });

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
      desc = "Bad request: you're trying to perform an illegal action.";
    else
      desc = "Something went wrong.";
    return View(new ErrorViewModel { Code = code, Description = desc });
  }
}
