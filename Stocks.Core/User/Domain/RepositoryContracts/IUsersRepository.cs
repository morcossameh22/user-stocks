using Microsoft.AspNetCore.Identity;

using Stocks.Core.Identity;

namespace Stocks.Core.User.Domain.RepositoryContracts
{
    /* This is an interface for a repository that deals with user-related operations in a stock trading
    application. It defines four methods: */
    public interface IUsersRepository
    {
        Task<IdentityResult> CreateUser(ApplicationUser user, string password);
        Task<IdentityResult> UpdateUser(ApplicationUser user);
        Task<ApplicationUser> FindByEmail(string email);
        Task<ApplicationUser> FindByUserIdWithStoks(string userId);
    }
}

