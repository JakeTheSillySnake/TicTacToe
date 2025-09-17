using Microsoft.AspNetCore.Mvc;
using TicTacToe.Web.Models;
using TicTacToe.Web.Mappers;
using TicTacToe.Web.Filters;
using TicTacToe.Web.Services;
using TicTacToe.DataSource.Services;

namespace TicTacToe.Web.Controllers;

[Route("game/user")]
[ServiceFilter(typeof(AuthFilter))]
[ApiController]
public class UserController
(IUserDbService userDb, IUserService userService) : Controller
{
    private readonly IUserDbService _userDb = userDb;
    private readonly IUserService _userService = userService;

    [HttpGet("{uuid}")]
    public async Task<ActionResult<UserDAO>> GetUser(string uuid)
    {
        var user = await _userDb.GetUserById(uuid);
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var registeredUserData = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);

        if (user == null || registeredUserData.uuid != uuid)
            return RedirectToAction("Error", "Main", new { code = 404 });

        return View(DataWebMapper.UserToDAO(user));
    }

    [HttpPost("{uuid}")]
    public async Task<IActionResult> EditUser(string uuid, [FromForm] UserDAO user)
    {
        if (uuid != user.Uuid)
            return RedirectToAction("Error", "Main", new { code = 404 });

        int error = await _userDb.UpdateUser(uuid, user.Password);

        if (error != 0)
            return RedirectToAction("Error", "Main", new { code = error });

        return RedirectToAction("Login", "Auth");
    }

    [Route("{uuid}/delete")]
    public async Task<IActionResult> DeleteUser(string uuid)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var registeredUserData = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);

        if (registeredUserData.uuid != uuid)
        {
            return RedirectToAction("Error", "Main", new { code = 404 });
        }

        int error = await _userDb.DeleteUser(uuid);

        if (error == 0)
            return RedirectToAction("Register", "Auth");

        return RedirectToAction("Error", "Main", new { code = error });
    }
}
