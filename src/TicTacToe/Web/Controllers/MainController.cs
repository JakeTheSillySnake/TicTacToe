using Microsoft.AspNetCore.Mvc;
using TicTacToe.DataSource.Services;
using TicTacToe.Web.Filters;
using TicTacToe.Web.Mappers;
using TicTacToe.Web.Models;
using TicTacToe.Web.Services;

namespace TicTacToe.Web.Controllers;

[Route("game")]
[ApiController]
public class MainController
(IUserService userService, IUserDbService userDb) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly IUserDbService _userDb = userDb;

    [Route("home")]
    [ServiceFilter(typeof(AuthFilter))]
    public async Task<IActionResult> Home()
    {
        string? authHeader = HttpContext.Request.Headers.Authorization;
        var (uuid, _) = (authHeader != null) ? _userService.AuthorizeUser(authHeader) : (null, null);
        var user = uuid != null ? await _userDb.GetUserById(uuid) : null;

        if (user != null)
            return View(DataWebMapper.UserToDAO(user));
        else
            return RedirectToAction("Error", new { code = 404 });
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int code)
    {
        string desc;
        if (code == 404)
            desc = "Not found: the page you're looking for doesn't exist.";
        else if (code == 400)
            desc = "Bad request: you're trying to perform an illegal action.";
        else if (code == 500)
            desc = "Request couldn't be fulfilled due to internal server error.";
        else
            desc = "Something went wrong.";
        return View(new ErrorViewModel { Code = code, Description = desc });
    }
}