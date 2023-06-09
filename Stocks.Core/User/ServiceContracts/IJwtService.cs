using Stocks.Core.DTO;
using Stocks.Core.Identity;

using System.Security.Claims;

namespace Stocks.Core.ServiceContracts
{
    /* This is an interface for a service that deals with JSON Web Tokens (JWTs) in an application.
    It has two methods: */
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}

