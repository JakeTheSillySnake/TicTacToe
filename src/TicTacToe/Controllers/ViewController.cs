using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
// using TicTacToe.Domain;
using TicTacToe.Data;
using TicTacToe.DI;

namespace TicTacToe.Controllers;

[Route("tictactoe/[controller]")]
[ApiController]
public class GameController : Controller {
  private readonly ILogger<GameController> _logger;
  public Configuration config = new();
  public StorageHandler storageHandler;
  public string currId = "";

  public GameController(ILogger<GameController> logger) {
    _logger = logger;
    storageHandler = config.GetStorageHandler();
  }

  public IActionResult Index() { return View(); }

  [Route("new")]
  public IActionResult NewGame() {
    if (!ModelState.IsValid) {
      return BadRequest(ModelState);
    }
    CurrentGameEntity game = new();
    storageHandler.SaveCurrentGame(game);
    // redirect to url with resource
    return RedirectToAction("GetGame", new { game.uuid });
  }

  [HttpGet("{uuid}")]
  public ActionResult<CurrentGameEntity> GetGame(string uuid) {
    var game = storageHandler.GetCurrentGameEntity(uuid);
    if (game == null)
      return NotFound();
    return View(DomainDataMapper.CurrentGameToDomain(game));
  }

  [HttpPost("{uuid}")]
  public IActionResult UpdateGame(string uuid) {
    var gameEntity = storageHandler.GetCurrentGameEntity(uuid);
    if (gameEntity == null)
      return NotFound();
    var game = DomainDataMapper.CurrentGameToDomain(gameEntity);
    if (!game.IsOver().status) {
      game.NextMove();
    }
    return RedirectToAction("GetGame", new { game.uuid });
  }

  /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,
                 NoStore = true)]
  public IActionResult Error() {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ??
                                                 HttpContext.TraceIdentifier });
  }*/
}
