using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stocks.Core.Identity;
using Stocks.Core.User.Domain.RepositoryContracts;

namespace Stocks.Infrastructure.Repositories
{
  public class UsersRepository : IUsersRepository
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersRepository(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
    {
      return await _userManager.CreateAsync(user, password);
    }

    public async Task<ApplicationUser> FindByEmail(string email)
    {
      ApplicationUser? user = await _userManager.FindByEmailAsync(email);

      if (user == null)
      {
        throw new Exception("User not found");
      }

      return user;
    }

    public async Task<ApplicationUser> FindByUserIdWithStoks(string userId)
    {

      ApplicationUser? user = await _userManager.Users
          .Include(u => u.Stocks)
          .FirstOrDefaultAsync(u => u.Id == new Guid(userId));

      if (user == null)
      {
        throw new Exception("User not found");
      }

      return user;
    }

    public async Task<IdentityResult> UpdateUser(ApplicationUser user)
    {
      return await _userManager.UpdateAsync(user);
    }
  }
}

