using System;
using Stocks.Core.DTO;
using Stocks.Core.Identity;
using System.Security.Claims;

namespace Stocks.Core.ServiceContracts
{
	public interface IJwtService
	{
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}

