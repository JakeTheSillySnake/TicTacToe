using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Web.Enum;
using TicTacToe.Web.Models;
using TicTacToe.Web.Services;

namespace TicTacToe.Web.Controllers;

[Route("game")]
[ApiController]
public class AuthController
(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    [Route("register")]
    public IActionResult Register()
    {
        return View(new SignUpRequest());
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDAO>> NewUser([FromForm] SignUpRequest request)
    {
        var res = await _authService.RegisterUser(request);

        if (res == (int)UserSignUpCodes.OK)
        {
            return TryAuthRequest(new JwtRequest { Login = request.Login, Password = request.Password });
        }
        else if (res == (int)UserSignUpCodes.LOGIN_EXISTS)
        {
            return View("Register", new SignUpRequest { loginExists = true });
        }

        return RedirectToAction("Error", "Main", new { code = 500 });
    }

    [Route("login")]
    public IActionResult Login()
    {
        return View(new JwtRequest());
    }

    [HttpPost("login")]
    public IActionResult Login([FromForm] JwtRequest request)
    {
        return TryAuthRequest(request);
    }

    [Authorize]
    [Route("logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("access_token");
        HttpContext.Response.Cookies.Delete("refresh_token");

        return RedirectToAction("Login");
    }

    [Route("update-access")]
    public async Task<IActionResult> UpdateAccessTokenAsync()
    {
        Debug.Print("updating access token...");
        var token = HttpContext.Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(token))
        {
            HttpContext.Response.StatusCode = 401;
            return RedirectToAction("Login");
        }

        var response = await _authService.UpdateAccessTokenAsync(new RefreshJwtRequest { RefreshToken = token });

        if (response.Type == (int)AuthCodes.OK)
        {
            HttpContext.Response.Cookies.Append("access_token", response.AccessToken, new CookieOptions { HttpOnly = true });
            string? localReturnUrl = HttpContext.Request.Query["returnUrl"];

            Debug.Print("access token updated");
            
            return RedirectToAction("UpdateRefreshToken", new { returnUrl = localReturnUrl });
        }
        else
        {
            HttpContext.Response.StatusCode = 401;
            return RedirectToAction("Login");
        }
    }

    [Authorize]
    [Route("update-refresh")]
    public async Task<IActionResult> UpdateRefreshToken()
    {
        Debug.Print("updating refresh token...");
        var token = HttpContext.Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(token))
        {
            HttpContext.Response.StatusCode = 401;
            return RedirectToAction("Login");
        }

        var response = await _authService.UpdateRefreshTokenAsync(new RefreshJwtRequest { RefreshToken = token });

        if (response.Type == (int)AuthCodes.OK)
        {
            HttpContext.Response.Cookies.Append("refresh_token", response.RefreshToken, new CookieOptions { HttpOnly = true });
            string? localReturnUrl = HttpContext.Request.Query["returnUrl"];

            Debug.Print("refresh token updated");

            if (!string.IsNullOrEmpty(localReturnUrl))
                return Redirect(localReturnUrl);

            return RedirectToAction("Home", "Main");
        }
        else
        {
            HttpContext.Response.StatusCode = 401;
            return RedirectToAction("Login");
        }
    }

    private ActionResult TryAuthRequest(JwtRequest request)
    {
        var response = _authService.Authorize(request);

        if (response.Type == (int)AuthCodes.OK)
        {
            HttpContext.Response.Cookies.Append("access_token", response.AccessToken, new CookieOptions { HttpOnly = true });
            HttpContext.Response.Cookies.Append("refresh_token", response.RefreshToken, new CookieOptions { HttpOnly = true });

            return RedirectToAction("Home", "Main");
        }
        else if (response.Type == (int)AuthCodes.INCORRECT_DATA)
        {
            return View("Login", new JwtRequest { IncorrectData = true });
        }

        return RedirectToAction("Error", "Main", new { Code = 500 });
    }
}