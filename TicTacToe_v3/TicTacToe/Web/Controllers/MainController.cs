using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Web.Models;
using TicTacToe.Web.Services;

namespace TicTacToe.Web.Controllers;

[Route("game")]
[ApiController]
public class MainController(IJwtProvider jwtProvider) : Controller
{
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    [Route("home")]
    [Authorize]
    public IActionResult Home()
    {
        var (name, uuid) = _jwtProvider.GetClaims(HttpContext);
        
        if (name != null && uuid != null)
            return View(new UserDAO { Login = name, Uuid = uuid });

        return RedirectToAction("Error", new { code = 403 });
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int code)
    {
        string desc;
        if (code == 400)
            desc = "Bad request: you're trying to perform an illegal action.";
        else if (code == 401)
            desc = "Unauthorized access: we couldn't confirm your identity.";
        else if (code == 403)
            desc = "Forbidden request: you don't have the rights to access this page.";
        else if (code == 404)
            desc = "Not found: the page you're looking for doesn't exist.";
        else if (code == 500)
            desc = "Request couldn't be fulfilled due to internal server error.";
        else
            desc = "Something went wrong.";
        return View(new ErrorViewModel { Code = code, Description = desc });
    }
}