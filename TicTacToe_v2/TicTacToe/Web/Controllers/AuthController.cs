using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Web.Enum;
using TicTacToe.Web.Models;
using TicTacToe.Web.Services;

namespace TicTacToe.Web.Controllers;

[Route("game")]
[ApiController]
public class AuthController
(IUserService userService) : Controller
{
    private readonly IUserService _userService = userService;
    private const string _authTypeName = "Basic ";
    private const string _realm = "\\tictactoe\\";

    [Route("register")]
    public IActionResult Register()
    {
        return View(new SignUpRequest());
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDAO>> NewUser([FromForm] SignUpRequest request)
    {
        var res = await _userService.RegisterUser(request);

        if (res == (int)UserSignUpCodes.OK)
            return RedirectToAction("Home", "Main");
        else if (res == (int)UserSignUpCodes.LOGIN_EXISTS)
            return View("Register", new SignUpRequest { loginExists = true });
        else
            return RedirectToAction("Error", "Main", new { code = 500 });
    }

    [Route("login")]
    public void Login()
    {
        var request = HttpContext.Request;
        var response = HttpContext.Response;
        string? authHeader = request.Headers.Authorization;
        bool authSuccessful = false;

        if (authHeader != null && authHeader.StartsWith(_authTypeName, StringComparison.OrdinalIgnoreCase))
        {
            var (uuid, login) = _userService.AuthorizeUser(authHeader);
            if (uuid != null && login != null)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, login, ClaimValueTypes.String, _authTypeName) };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, _authTypeName));

                authSuccessful = true;
                HttpContext.User = principal;
            }
        }

        if (!authSuccessful)
        {
            response.StatusCode = 401;
            response.Headers.Append("WWW-Authenticate", $"Basic realm={_realm}");
        }
        else
            response.Redirect("/game/home");
    }
}