namespace TicTacToe.Web.Services;

using TicTacToe.Web.Models;

public interface IUserService
{
    public Task<int> RegisterUser(SignUpRequest request);
    public (string? uuid, string? login) AuthorizeUser(string header);
}