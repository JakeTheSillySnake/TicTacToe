using Microsoft.AspNetCore.Mvc;
using TicTacToe.Web.Models;
using TicTacToe.Web.Filters;
using TicTacToe.DataSource.Services;
using TicTacToe.Web.Services;

namespace TicTacToe.Web.Controllers;

[Route("game")]
[ServiceFilter(typeof(AuthFilter))]
[ApiController]
public class GameController
(IGameDbService gameDb, IUserService userService) : Controller
{
    private readonly IGameDbService _gameDb = gameDb;
    private readonly IUserService _userService = userService;

    [Route("new-solo")]
    public async Task<IActionResult> NewGameSoloAsync()
    {
        return await NewGame(true);
    }

    [Route("new-multi")]
    public async Task<IActionResult> NewGameMultiAsync()
    {
        return await NewGame(false);
    }

    private async Task<IActionResult> NewGame(bool isSolo)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var (userUuid, _) = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);

        if (userUuid == null)
            return RedirectToAction("Error", "Main", new { Code = 404 });

        var game = await _gameDb.NewGame(userUuid, isSolo);

        if (game == null)
            return RedirectToAction("Error", "Main", new { Code = 500 });

        return RedirectToAction("GetGame", new { game.Uuid });
    }

    [HttpGet("{uuid}")]
    public async Task<ActionResult<CurrentGameDAO>> GetGame(string uuid)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var (userUuid, _) = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);
        var game = await _gameDb.GetGame(uuid);

        if (userUuid == null || game == null || (userUuid != game.PlayerO && userUuid != game.PlayerX))
            return RedirectToAction("Error", "Main", new { Code = 404 });

        return View(new CurrentGameDAO { Uuid = game.Uuid, GameBoard = new GameBoardDAO(game.GameBoard.Field),
                                         PlayerX = game.PlayerX, PlayerO = game.PlayerO, State = game.State,
                                         IsPlayerX = userUuid == game.PlayerX });
    }

    [HttpPost("{uuid}")]
    public async Task<IActionResult> UpdateGame(string uuid, [FromForm] CurrentGameDAO currentGameDAO)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var (userUuid, _) = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);
        var game = await _gameDb.GetGame(uuid);

        if (game == null || userUuid == null || (userUuid != game.PlayerO && userUuid != game.PlayerX))
        {
            return RedirectToAction("Error", "Main", new { code = 404 });
        }

        var error = await _gameDb.UpdateGame(uuid, userUuid, currentGameDAO.Action);

        if (error != 0)
            return RedirectToAction("Error", "Main", new { Code = error });

        return RedirectToAction("GetGame", new { uuid });
    }

    [Route("{uuid}/delete")]
    public async Task<IActionResult> DeleteGame(string uuid)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var (userUuid, _) = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);
        var game = await _gameDb.GetGame(uuid);

        if (game == null || userUuid == null || (userUuid != game.PlayerO && userUuid != game.PlayerX))
        {
            return RedirectToAction("Error", "Main", new { code = 404 });
        }

        int error = await _gameDb.DeleteGame(uuid);

        if (error == 0)
            return RedirectToAction("GetAvailableGames");

        return RedirectToAction("Error", "Main", new { code = error });
    }

    [Route("all")]
    public async Task<IActionResult> GetAvailableGames()
    {
        var gamesDTO = await _gameDb.GetAllGames();
        List<CurrentGameDAO> gamesDAO = [];
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var (userUuid, _) = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);

        if (userUuid == null)
            return RedirectToAction("Error", "Main", new { Code = 404 });

        foreach (var gameDTO in gamesDTO)
        {
            if (userUuid == gameDTO.PlayerO || userUuid == gameDTO.PlayerX || gameDTO.PlayerO == null)
                gamesDAO.Add(new CurrentGameDAO { Uuid = gameDTO.Uuid,
                                                  GameBoard = new GameBoardDAO(gameDTO.GameBoard.Field),
                                                  PlayerX = gameDTO.PlayerX, PlayerO = gameDTO.PlayerO });
        }

        return View(gamesDAO.OrderBy(obj => obj.PlayerO is "" ? 0 : 1));
    }

    [Route("{uuid}/join")]
    public async Task<IActionResult> JoinGame(string uuid)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var (userUuid, _) = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);

        if (userUuid == null)
            return RedirectToAction("Error", "Main", new { Code = 404 });

        int error = await _gameDb.AddPlayerO(uuid, userUuid);

        if (error != 0)
            return RedirectToAction("Error", "Main", new { Code = error });

        return RedirectToAction("GetGame", new { uuid });
    }
}
