namespace TicTacToe.Web.Services;

using TicTacToe.Web.Models;

public interface IUserService
{
    public Task<int> RegisterUser(SignUpRequest request);
    public UserDAO? AuthorizeUser(string login, string passwd);
}