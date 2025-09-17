using Microsoft.EntityFrameworkCore;
using TicTacToe.DataSource.Enum;
using TicTacToe.DataSource.Models;

namespace TicTacToe.DataSource.Services;

public class UserDbService
(AppDbContext dbContext) : IUserDbService
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<UserDTO?> NewUser(string login, string password)
    {
        var user = new UserDTO { Login = login, Password = password, Uuid = Guid.NewGuid().ToString() };

        _dbContext.Users.Add(user);
        try
        {
            await _dbContext.SaveChangesAsync();
            return user;
        }
        catch
        {
            return null;
        }
    }
    public async Task<UserDTO?> GetUserById(string uuid)
    {
        return await _dbContext.Users.FindAsync(uuid);
    }
    public UserDTO ? GetUserByLogin(string login)
    {
        return _dbContext.Users.Where(e => e.Login == login).FirstOrDefault();
    }
    public async Task<int> UpdateUser(string uuid, string newPassword)
    {
        var user = await GetUserById(uuid);

        if (user == null)
            return (int)ErrorCodes.NOT_FOUND;

        _dbContext.Entry(user).State = EntityState.Detached;
        user.Password = newPassword;
        _dbContext.Entry(user).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
            return 0;
        }
        catch
        {
            return (int)ErrorCodes.SERVER_ERROR;
        }
    }
    public async Task<int> DeleteUser(string uuid)
    {
        var user = await _dbContext.Users.FindAsync(uuid);

        if (user == null)
            return (int)ErrorCodes.NOT_FOUND;
        ;

        _dbContext.Entry(user).State = EntityState.Detached;
        _dbContext.Users.Remove(user);

        try
        {
            await _dbContext.SaveChangesAsync();
            return 0;
        }
        catch
        {
            return (int)ErrorCodes.SERVER_ERROR;
        }
    }
}