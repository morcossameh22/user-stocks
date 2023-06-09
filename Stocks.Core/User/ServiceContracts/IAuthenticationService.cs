using Stocks.Core.DTO;

namespace Stocks.Core.User.ServiceContracts
{
    /* This is a C# interface called `IAuthenticationService` that defines a contract for an
    authentication service. It includes four methods: */
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Register(RegisterDTO registerDTO);
        Task<AuthenticationResponse> Login(LoginDTO loginDTO);
        Task Logout();
        Task<AuthenticationResponse> GenerateNewAccessToken(TokenModel tokenModel);
    }
}

