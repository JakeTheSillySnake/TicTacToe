using TicTacToe.DataSource.Models;

namespace TicTacToe.DataSource.Services;

public interface IGameDbService
{
    Task<CurrentGameDTO?> NewGame(string userUuid, bool isSolo);
    Task<CurrentGameDTO?> GetGame(string uuid);
    Task<int> UpdateGame(string uuid, string userUuid, string? action);
    Task<int> DeleteGame(string uuid);
    Task<int> ClearGame(string uuid);
    Task<List<CurrentGameDTO>> GetAllGames();
    Task<List<CurrentGameDTO>> GetFinishedGames(string userUuid);
    Task<int> AddPlayerO(string uuid, string userUuid);
    Task<List<UserWinRatioDTO>> GetUserWinRatios(int n);
}