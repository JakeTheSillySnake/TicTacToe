namespace TicTacToe.Web.Services;

using TicTacToe.Web.Models;

public interface IAuthService
{
    public JwtResponse Authorize(JwtRequest request);
    public Task<JwtResponse> UpdateAccessTokenAsync(RefreshJwtRequest request);
    public Task<JwtResponse> UpdateRefreshTokenAsync(RefreshJwtRequest request);
    public Task<int> RegisterUser(SignUpRequest request);
}