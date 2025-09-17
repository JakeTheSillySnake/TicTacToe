using Microsoft.AspNetCore.Mvc;
using TicTacToe.Web.Models;
using TicTacToe.DataSource.Services;
using TicTacToe.Web.Services;
using Microsoft.AspNetCore.Authorization;
using TicTacToe.DataSource.Enum;

namespace TicTacToe.Web.Controllers;

[Route("game")]
[ApiController]
[Authorize]
public class GameController
(IGameDbService gameDb, IUserDbService userDb, IJwtProvider jwtProvider) : Controller
{
    private readonly IGameDbService _gameDb = gameDb;
    private readonly IUserDbService _userDb = userDb;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

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
        var (name, uuid) = _jwtProvider.GetClaims(HttpContext);

        if (uuid == null || name == null)
            return RedirectToAction("Error", "Main", new { Code = 403 });

        var game = await _gameDb.NewGame(uuid, isSolo);

        if (game == null)
            return RedirectToAction("Error", "Main", new { Code = 500 });

        return RedirectToAction("GetGame", new { game.Uuid });
    }

    [HttpGet("{uuid}")]
    public async Task<ActionResult<CurrentGameDAO>> GetGame(string uuid)
    {
        var (name, userUuid) = _jwtProvider.GetClaims(HttpContext);
        var game = await _gameDb.GetGame(uuid);

        if (name == null || userUuid == null || game == null || (userUuid != game.PlayerO && userUuid != game.PlayerX))
            return RedirectToAction("Error", "Main", new { Code = 403 });

        return View(new CurrentGameDAO { Uuid = game.Uuid, GameBoard = new GameBoardDAO(game.GameBoard.Field),
                                         PlayerX = game.PlayerX, PlayerO = game.PlayerO, State = game.State,
                                         IsPlayerX = userUuid == game.PlayerX });
    }

    [HttpPost("{uuid}")]
    public async Task<IActionResult> UpdateGame(string uuid, [FromForm] CurrentGameDAO currentGameDAO)
    {
        var (name, userUuid) = _jwtProvider.GetClaims(HttpContext);
        var game = await _gameDb.GetGame(uuid);

        if (name == null || game == null || userUuid == null || (userUuid != game.PlayerO && userUuid != game.PlayerX))
        {
            return RedirectToAction("Error", "Main", new { code = 403 });
        }

        var error = await _gameDb.UpdateGame(uuid, userUuid, currentGameDAO.Action);

        if (error != 0)
            return RedirectToAction("Error", "Main", new { Code = error });

        return RedirectToAction("GetGame", new { uuid });
    }

    [Route("{uuid}/delete")]
    public async Task<IActionResult> DeleteGame(string uuid)
    {
        var (name, userUuid) = _jwtProvider.GetClaims(HttpContext);
        var game = await _gameDb.GetGame(uuid);

        if (name == null || game == null || userUuid == null || (userUuid != game.PlayerO && userUuid != game.PlayerX))
        {
            return RedirectToAction("Error", "Main", new { code = 403 });
        }

        int error;

        if (game.State >= (int)GameStates.PLAYERX_WIN)
            error = await _gameDb.ClearGame(uuid);
        else
            error = await _gameDb.DeleteGame(uuid);

        if (error == 0)
            return RedirectToAction("GetAvailableGames");

        return RedirectToAction("Error", "Main", new { code = error });
    }

    [Route("all")]
    public async Task<IActionResult> GetAvailableGames()
    {
        var gamesDTO = await _gameDb.GetAllGames();
        List<CurrentGameDAO> gamesDAO = [];
        var (name, userUuid) = _jwtProvider.GetClaims(HttpContext);

        if (name == null || userUuid == null)
            return RedirectToAction("Error", "Main", new { Code = 403 });

        foreach (var gameDTO in gamesDTO)
        {
            if (!gameDTO.Cleared && (userUuid == gameDTO.PlayerO || userUuid == gameDTO.PlayerX || gameDTO.PlayerO == null))
                gamesDAO.Add(new CurrentGameDAO
                {
                    Uuid = gameDTO.Uuid,
                    GameBoard = new GameBoardDAO(gameDTO.GameBoard.Field),
                    PlayerX = gameDTO.PlayerX,
                    PlayerO = gameDTO.PlayerO,
                    CreationDate = gameDTO.CreationDate,
                    State = gameDTO.State
                });
        }

        return View(gamesDAO.OrderBy(obj => obj.PlayerO is "" ? 0 : 1).ThenByDescending(obj => obj.CreationDate));
    }

    [Route("history")]
    public async Task<IActionResult> GetFinishedGames()
    {
        var (name, userUuid) = _jwtProvider.GetClaims(HttpContext);

        if (name == null || userUuid == null)
            return RedirectToAction("Error", "Main", new { Code = 403 });

        var gamesUnordered = await _gameDb.GetFinishedGames(userUuid);
        var gamesDTO = gamesUnordered.OrderByDescending(game => game.CreationDate);
        List<CurrentGameDAO> gamesDAO = [];

        foreach (var gameDTO in gamesDTO)
        {
            gamesDAO.Add(new CurrentGameDAO
            {
                Uuid = gameDTO.Uuid,
                GameBoard = new GameBoardDAO(gameDTO.GameBoard.Field),
                PlayerX = gameDTO.PlayerX,
                PlayerO = gameDTO.PlayerO,
                IsPlayerX = userUuid == gameDTO.PlayerX,
                CreationDate = gameDTO.CreationDate,
                State = gameDTO.State
            });
        }

        return View(gamesDAO);
    }

    [Route("leaders/{n}")]
    public async Task<IActionResult> GetLeaders(int n)
    {
        var leadersDTO = await _gameDb.GetUserWinRatios(n);
        List<UserWinRatioDAO> leadersDAO = [];

        foreach (var leaderDTO in leadersDTO)
        {
            var user = await _userDb.GetUserById(leaderDTO.Uuid);
            
            leadersDAO.Add(new UserWinRatioDAO
            {
                Uuid = leaderDTO.Uuid,
                Wins = leaderDTO.Wins,
                Name = (user == null) ? "Name not found" : user.Login,
                WinRatio = leaderDTO.WinRatio * 100,
                Total = leaderDTO.Total
            });
        }

        return View(leadersDAO);
    }

    [Route("{uuid}/join")]
    public async Task<IActionResult> JoinGame(string uuid)
    {
        var (name, userUuid) = _jwtProvider.GetClaims(HttpContext);

        if (name == null || userUuid == null)
            return RedirectToAction("Error", "Main", new { Code = 403 });

        int error = await _gameDb.AddPlayerO(uuid, userUuid);

        if (error != 0)
            return RedirectToAction("Error", "Main", new { Code = error });

        return RedirectToAction("GetGame", new { uuid });
    }
}
