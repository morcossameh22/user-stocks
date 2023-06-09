using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Stocks.Core.Identity;
using Stocks.Core.User.Domain.RepositoryContracts;

namespace Stocks.Infrastructure.Repositories
{
    /* The UsersRepository class implements methods for creating, finding, and updating users using a
    UserManager. */
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// This function creates a new user with a given password using the ASP.NET Identity framework.
        /// </summary>
        /// <param name="ApplicationUser">ApplicationUser is a class that represents a user in an
        /// ASP.NET Core application. It typically contains properties such as Id, UserName, Email, and
        /// PasswordHash. This class is used by the ASP.NET Core Identity system to manage user
        /// authentication and authorization.</param>
        /// <param name="password">The password parameter is a string that represents the password for
        /// the user being created. It will be used to create a hashed and encrypted version of the
        /// password that will be stored in the database for security purposes.</param>
        /// <returns>
        /// The method `CreateUser` returns a `Task` object that represents the asynchronous operation
        /// of creating a new user with the specified `ApplicationUser` object and password. The
        /// `IdentityResult` object is the result of the operation, which indicates whether the user was
        /// successfully created or not.
        /// </returns>
        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// This function finds a user by their email address and returns their application information.
        /// </summary>
        /// <param name="email">The email parameter is a string that represents the email address of the
        /// user being searched for.</param>
        /// <returns>
        /// The method is returning a `Task` that will eventually resolve to an instance of
        /// `ApplicationUser`.
        /// </returns>
        public async Task<ApplicationUser> FindByEmail(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email)
                ?? throw new Exception(InfrastructureConstants.UserNotFound);
            return user;
        }

        /// <summary>
        /// This function finds a user by their ID and includes their associated stocks.
        /// </summary>
        /// <param name="userId">The userId parameter is a string that represents the unique identifier
        /// of a user in the application. It is used to search for a specific user in the database and
        /// retrieve their information along with their associated stocks.</param>
        /// <returns>
        /// The method is returning a `Task` that will eventually resolve to an `ApplicationUser` object
        /// that has been retrieved from the database along with its associated `Stocks`. If the user
        /// with the specified `userId` is not found, an exception with the message "User not found"
        /// will be thrown.
        /// </returns>
        public async Task<ApplicationUser> FindByUserIdWithStoks(string userId)
        {

            ApplicationUser? user = await _userManager.Users
                .Include(u => u.Stocks)
                .FirstOrDefaultAsync(u => u.Id == new Guid(userId))
              ?? throw new Exception(InfrastructureConstants.UserNotFound);

            return user;
        }

        /// <summary>
        /// This function updates a user in the application using the UserManager's UpdateAsync method.
        /// </summary>
        /// <param name="ApplicationUser">ApplicationUser is a class that represents a user in an
        /// ASP.NET Core application. It typically contains properties such as Id, UserName, Email, and
        /// PasswordHash, among others. This class is used by the ASP.NET Core Identity system to manage
        /// user authentication and authorization.</param>
        /// <returns>
        /// The method is returning a `Task` object that represents the asynchronous operation of
        /// updating the `ApplicationUser` object passed as a parameter. The `IdentityResult` object is
        /// the result of the update operation, which indicates whether the update was successful or
        /// not.
        /// </returns>
        public async Task<IdentityResult> UpdateUser(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}

