namespace TicTacToe.Web.Services;

using TicTacToe.DataSource.Services;
using TicTacToe.Web.Enum;
using TicTacToe.Web.Mappers;
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
    public UserDAO? AuthorizeUser(string login, string passwd)
    {
        var user = _userDb.GetUserByLogin(login);

        if (user != null && passwd == user.Password)
            return DataWebMapper.UserToDAO(user);

        return null;
    }
}