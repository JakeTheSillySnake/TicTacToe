using TicTacToe.DataSource.Models;

namespace TicTacToe.DataSource.Services;

public interface IUserDbService
{
    public Task<UserDTO?> NewUser(string login, string password);
    public Task<UserDTO?> GetUserById(string uuid);
    public UserDTO ? GetUserByLogin(string login);
    public Task<int> UpdateUser(string uuid, string newPassword);
    public Task<int> DeleteUser(string uuid);
}