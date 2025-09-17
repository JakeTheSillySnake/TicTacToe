using Microsoft.AspNetCore.Mvc;
using TicTacToe.Web.Models;
using TicTacToe.Web.Services;
using TicTacToe.DataSource.Services;
using Microsoft.AspNetCore.Authorization;

namespace TicTacToe.Web.Controllers;

[Route("game/user")]
[ApiController]
[Authorize]
public class UserController
(IUserDbService userDb, IJwtProvider jwtProvider) : Controller
{
    private readonly IUserDbService _userDb = userDb;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    [HttpGet]
    public IActionResult GetUser()
    {
        var (name, uuid) = _jwtProvider.GetClaims(HttpContext);

        if (name == null || uuid == null)
            return RedirectToAction("Error", "Main", new { code = 403 });

        return View(new UserDAO { Login = name, Uuid = uuid });
    }

    [HttpPost]
    public async Task<IActionResult> EditUser([FromForm] ChangePasswdRequest passwdRequest)
    {
        var (name, uuid ) = _jwtProvider.GetClaims(HttpContext);
        var user = uuid != null ? await _userDb.GetUserById(uuid) : null;

        if (uuid == null || name == null || user == null)
            return RedirectToAction("Error", "Main", new { code = 403 });

        if (user.Password != passwdRequest.CurrentPassword)
            return View("GetUser", new UserDAO { Login = name, Uuid = uuid, PasswordCorrect = false });

        int error = await _userDb.UpdateUser(uuid, passwdRequest.NewPassword);

        if (error != 0)
            return RedirectToAction("Error", "Main", new { code = error });

        return View("GetUser", new UserDAO { Login = name, Uuid = uuid });
    }

    [Route("delete")]
    public async Task<IActionResult> DeleteUser()
    {
        var (name, uuid) = _jwtProvider.GetClaims(HttpContext);

        if (name == null || uuid == null)
            return RedirectToAction("Error", "Main", new { code = 403 });

        int error = await _userDb.DeleteUser(uuid);

        if (error == 0)
            return RedirectToAction("Logout", "Auth");

        return RedirectToAction("Error", "Main", new { code = error });
    }
}
