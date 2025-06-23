using Microsoft.EntityFrameworkCore;
using TicTacToe.DataSource.Enum;
using TicTacToe.DataSource.Mappers;
using TicTacToe.DataSource.Models;
using TicTacToe.Domain.Services;

namespace TicTacToe.DataSource.Services;

public class GameDbService
(AppDbContext dbContext, IGameService gameService) : IGameDbService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IGameService _gameService = gameService;
    public async Task<CurrentGameDTO?> NewGame(string userUuid, bool isSolo)
    {
        var gameDTO =
            new CurrentGameDTO { Uuid = Guid.NewGuid().ToString(), PlayerX = userUuid, PlayerO = isSolo ? "" : null };

        if (isSolo)
            gameDTO.State = (int)GameStates.PLAYERX_TURN;
        _dbContext.Games.Add(gameDTO);
        try
        {
            await _dbContext.SaveChangesAsync();
            return gameDTO;
        }
        catch
        {
            return null;
        }
    }
    public async Task<CurrentGameDTO?> GetGame(string uuid)
    {
        return await _dbContext.Games.FindAsync(uuid);
    }
    public async Task<int> UpdateGame(string uuid, string userUuid, string? action)
    {
        var gameDTO = await GetGame(uuid);

        if (gameDTO == null)
            return (int)ErrorCodes.NOT_FOUND;
        else if (action == null || !CheckState(gameDTO, userUuid))
            return 0;

        _dbContext.Entry(gameDTO).State = EntityState.Detached;
        _ = int.TryParse(action, out int actionToInt);
        int j = actionToInt % 100, i = actionToInt / 100;

        if (gameDTO.PlayerO == "")
            gameDTO = UpdateSoloGame(gameDTO, i, j);
        else
            gameDTO = UpdateMultiGame(gameDTO, i, j, userUuid);

        if (gameDTO == null)
            return (int)ErrorCodes.BAD_REQUEST;

        CheckForWinner(gameDTO);
        _dbContext.Entry(gameDTO).State = EntityState.Modified;

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
    public async Task<int> DeleteGame(string uuid)
    {
        var gameDTO = await GetGame(uuid);

        if (gameDTO == null)
            return (int)ErrorCodes.NOT_FOUND;

        _dbContext.Entry(gameDTO).State = EntityState.Detached;
        _dbContext.Games.Remove(gameDTO);

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

    public async Task<List<CurrentGameDTO>> GetAllGames()
    {
        return await _dbContext.Games.ToListAsync();
    }

    public async Task<int> AddPlayerO(string uuid, string userUuid)
    {
        var game = await GetGame(uuid);

        if (game == null || game.PlayerO != null)
            return (int)ErrorCodes.NOT_FOUND;
        else if (game.PlayerX == userUuid)
            return 0;

        _dbContext.Entry(game).State = EntityState.Detached;
        game.PlayerO = userUuid;
        game.State = (int)GameStates.PLAYERX_TURN;
        _dbContext.Entry(game).State = EntityState.Modified;

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

    private bool CheckState(CurrentGameDTO game, string userUuuid)
    {
        if ((userUuuid == game.PlayerO && game.State == (int)GameStates.PLAYERO_TURN) ||
            (userUuuid == game.PlayerX && game.State == (int)GameStates.PLAYERX_TURN))
            return true;

        return false;
    }

    private CurrentGameDTO ? UpdateSoloGame(CurrentGameDTO gameDTO, int row, int col)
    {
        var game = DomainDataMapper.CurrentGameToDomain(gameDTO);

        if (_gameService.IsValid(game, row, col))
            _gameService.MovePlayer(game, row, col, true);
        else
            return null;

        if (!_gameService.IsOver(game).status)
            _gameService.NextMove(game);

        return DomainDataMapper.CurrentGameToDTO(game, gameDTO.PlayerX, gameDTO.PlayerO, gameDTO.State);
    }

    private CurrentGameDTO ? UpdateMultiGame(CurrentGameDTO gameDTO, int row, int col, string userUuid)
    {
        var game = DomainDataMapper.CurrentGameToDomain(gameDTO);

        if (_gameService.IsValid(game, row, col))
        {
            if (userUuid == gameDTO.PlayerX)
            {
                _gameService.MovePlayer(game, row, col, true);
                if (_gameService.IsOver(game).status)
                    gameDTO.State = (int)GameStates.PLAYERX_TURN;
                else
                    gameDTO.State = (int)GameStates.PLAYERO_TURN;
            }
            else
            {
                _gameService.MovePlayer(game, row, col, false);
                if (_gameService.IsOver(game).status)
                    gameDTO.State = (int)GameStates.PLAYERO_TURN;
                else
                    gameDTO.State = (int)GameStates.PLAYERX_TURN;
            }
        }
        else
            return null;

        return DomainDataMapper.CurrentGameToDTO(game, gameDTO.PlayerX, gameDTO.PlayerO, gameDTO.State);
    }

    private void CheckForWinner(CurrentGameDTO gameDTO)
    {
        var game = DomainDataMapper.CurrentGameToDomain(gameDTO);
        var winResult = _gameService.IsOver(game);

        if (winResult.winner == GameService.playerX)
            gameDTO.State = (int)GameStates.PLAYERX_WIN;
        else if (winResult.winner == GameService.playerO)
            gameDTO.State = (int)GameStates.PLAYERO_WIN;
        else if (winResult.status)
            gameDTO.State = (int)GameStates.DRAW;
    }
}