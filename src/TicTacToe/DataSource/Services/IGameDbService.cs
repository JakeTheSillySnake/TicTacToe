using TicTacToe.DataSource.Models;

namespace TicTacToe.DataSource.Services;

public interface IGameDbService
{
    Task<CurrentGameDTO?> NewGame(string userUuid, bool isSolo);
    Task<CurrentGameDTO?> GetGame(string uuid);
    Task<int> UpdateGame(string uuid, string userUuid, string? action);
    Task<int> DeleteGame(string uuid);
    Task<List<CurrentGameDTO>> GetAllGames();
    Task<int> AddPlayerO(string uuid, string userUuid);
}