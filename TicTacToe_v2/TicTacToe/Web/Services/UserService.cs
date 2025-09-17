namespace TicTacToe.Web.Services;

using System.Net.Http.Headers;
using System.Text;
using TicTacToe.DataSource.Services;
using TicTacToe.Web.Enum;
using TicTacToe.Web.Models;

public class UserService
(IUserDbService userDb) : IUserService
{
    private readonly IUserDbService _userDb = userDb;
    public async Task<int> RegisterUser(SignUpRequest request)
    {
        if (_userDb.GetUserByLogin(request.Login) != null)
            return (int)UserSignUpCodes.LOGIN_EXISTS;

        var user = await _userDb.NewUser(request.Login, request.Password);

        if (user == null)
            return (int)UserSignUpCodes.SERVER_ERROR;

        return (int)UserSignUpCodes.OK;
    }
    public (string? uuid, string? login) AuthorizeUser(string header)
    {
        var encodedStr = AuthenticationHeaderValue.Parse(header).Parameter;
        string? credStr = (encodedStr != null) ? Encoding.UTF8.GetString(Convert.FromBase64String(encodedStr)) : null;

        if (string.IsNullOrEmpty(credStr))
            return (null, null);

        var credentials = credStr.Split([':'], 2);
        var user = _userDb.GetUserByLogin(credentials[0]);

        if (user != null && credentials[1] == user.Password)
            return (user.Uuid, user.Login);

        return (null, null);
    }
}