using Stocks.Core.DTO;

namespace Stocks.Core.User.ServiceContracts
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Register(RegisterDTO registerDTO);
        Task<AuthenticationResponse> Login(LoginDTO loginDTO);
        Task Logout();
        Task<AuthenticationResponse> GenerateNewAccessToken(TokenModel tokenModel);
    }
}

