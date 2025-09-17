namespace TicTacToe.Web.Services;

using TicTacToe.DataSource.Services;
using TicTacToe.Web.Enum;
using TicTacToe.Web.Models;

public class AuthService(IUserService userService, IJwtProvider jwtProvider, IUserDbService userDb) : IAuthService
{
    private readonly IUserService _userService = userService;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IUserDbService _userDb = userDb;
    public JwtResponse Authorize(JwtRequest request)
    {
        var user = _userService.AuthorizeUser(request.Login, request.Password);

        if (user == null)
            return new JwtResponse { Type = (int)AuthCodes.INCORRECT_DATA };

        var accessToken = _jwtProvider.GetNewAccessToken(user);
        var refreshToken = _jwtProvider.GetNewRefreshToken(user);

        if (accessToken == null || refreshToken == null)
            return new JwtResponse { Type = (int)AuthCodes.SERVER_ERROR };

        return new JwtResponse { Type = (int)AuthCodes.OK, AccessToken = accessToken, RefreshToken = refreshToken };
    }
    public async Task<JwtResponse> UpdateAccessTokenAsync(RefreshJwtRequest request)
    {
        var token = await GetUpdatedTokenAsync(request.RefreshToken, true);

        if (token == null)
            return new JwtResponse { Type = (int)AuthCodes.SERVER_ERROR };

        return new JwtResponse { Type = (int)AuthCodes.OK, AccessToken = token };
    }
    public async Task<JwtResponse> UpdateRefreshTokenAsync(RefreshJwtRequest request)
    {
        var token = await GetUpdatedTokenAsync(request.RefreshToken, false);

        if (token == null)
            return new JwtResponse { Type = (int)AuthCodes.SERVER_ERROR };

        return new JwtResponse { Type = (int)AuthCodes.OK, RefreshToken = token };
    }
    public async Task<int> RegisterUser(SignUpRequest request)
    {
        return await _userService.RegisterUser(request);
    }
    private async Task<string?> GetUpdatedTokenAsync(string refreshToken, bool isAccessToken)
    {
        var (name, uuid) = _jwtProvider.GetClaims(refreshToken, isAccessToken);
        var user = uuid != null ? await _userDb.GetUserById(uuid) : null;

        if (name == null || uuid == null || user == null || user.Login != name)
            return null;

        if (isAccessToken)
            return _jwtProvider.GetNewAccessToken(new UserDAO { Login = name, Uuid = uuid });
        
        return _jwtProvider.GetNewRefreshToken(new UserDAO { Login = name, Uuid = uuid });
    }
}