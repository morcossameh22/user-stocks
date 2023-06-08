using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Stocks.Core.Identity;
using Stocks.Core.ServiceContracts;
using Stocks.Core.User.Domain.RepositoryContracts;
using Stocks.Infrastructure.Repositories;

namespace Stocks.Tests.Infrastructure.Repositories
{
	public class UsersRepositoryTests
	{
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly UsersRepository _userRepository;

        public UsersRepositoryTests()
		{
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );

            _userRepository = new UsersRepository(_userManagerMock.Object);
        }

        [Fact]
        public async Task CreateUser_ValidUserAndPassword_Success()
        {
            var user = new ApplicationUser {
                Id = Guid.NewGuid(),
                UserName = "john.doe",
                Email = "john.doe@example.com"
            };
            var password = "password";

            _userManagerMock.Setup(um => um.CreateAsync(user, password))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userRepository.CreateUser(user, password);

            Assert.Equal(IdentityResult.Success, result);
        }

        [Fact]
        public async Task FindByEmail_ExistingEmail_UserReturned()
        {
            var email = "john.doe@example.com";
            var user = new ApplicationUser {
                Id = Guid.NewGuid(),
                UserName = "john.doe",
                Email = email
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(email))
                .ReturnsAsync(user);

            var result = await _userRepository.FindByEmail(email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task FindByEmail_NonExistingEmail_ExceptionThrown()
        {
            var email = "nonexisting@example.com";

            _userManagerMock.Setup(um => um.FindByEmailAsync(email));

            await Assert.ThrowsAsync<Exception>(() => _userRepository.FindByEmail(email));
        }

        [Fact]
        public async Task FindByUserIdWithStoks_ExistingUserId_UserWithStocksReturned()
        {
            var userId = Guid.NewGuid();
            var users = new List<ApplicationUser>
            {
                new ApplicationUser {
                    Id = userId,
                    UserName = "john.doe",
                    Email = "john.doe@example.com"
                }
            };

            _userManagerMock.Setup(um => um.Users)
                    .Returns(users.AsQueryable().BuildMock());

            var result = await _userRepository.FindByUserIdWithStoks(userId.ToString());

            Assert.Equal(users[0], result);
        }

        [Fact]
        public async Task FindByUserIdWithStoks_NonExistingUserId_ExceptionThrown()
        {
            var userId = Guid.NewGuid();

            var users = new List<ApplicationUser>
            {
                new ApplicationUser {
                    Id = Guid.NewGuid(),
                    UserName = "john.doe",
                    Email = "john.doe@example.com"
                }
            };

            _userManagerMock.Setup(um => um.Users)
                    .Returns(users.AsQueryable().BuildMock());

            await Assert.ThrowsAsync<Exception>(() => _userRepository.FindByUserIdWithStoks(userId.ToString()));
        }

        [Fact]
        public async Task UpdateUser_ValidUser_Success()
        {
            var user = new ApplicationUser {
                Id = Guid.NewGuid(),
                UserName = "john.doe",
                Email = "john.doe@example.com"
            };

            _userManagerMock.Setup(um => um.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userRepository.UpdateUser(user);

            Assert.Equal(IdentityResult.Success, result);
        }
    }
}

