using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicTacToe.Web.Services;

namespace TicTacToe.Web.Filters;

[AttributeUsage(AttributeTargets.All)]
public class AuthFilter
(IUserService userService) : Attribute, IAuthorizationFilter
{
    private readonly IUserService _userService = userService;
    private const string _authTypeName = "Basic ";

    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {

        var request = filterContext.HttpContext.Request;
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
                filterContext.HttpContext.User = principal;
            }
        }

        if (!authSuccessful)
        {
            filterContext.HttpContext.Response.StatusCode = 401;
            filterContext.Result = new RedirectToActionResult("Login", "Auth", new {});
        }
    }
}