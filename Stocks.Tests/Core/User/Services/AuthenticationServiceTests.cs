using System;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Stocks.Core.DTO;
using Stocks.Core.Identity;
using Stocks.Core.ServiceContracts;
using Stocks.Core.Stock.Domain.RepositoryContracts;
using Stocks.Core.Stock.ServiceContracts;
using Stocks.Core.Stock.Services;
using Stocks.Core.User.Domain.RepositoryContracts;
using Stocks.Core.User.ServiceContracts;
using Stocks.Core.User.Services;
using AuthenticationService = Stocks.Core.User.Services.AuthenticationService;
using IAuthenticationService = Stocks.Core.User.ServiceContracts.IAuthenticationService;

namespace Stocks.Tests.Core.User.Services
{
	public class AuthenticationServiceTests
	{
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IAuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            _usersRepositoryMock = new Mock<IUsersRepository>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var optionsAccessorMock = new Mock<IOptions<IdentityOptions>>();
            var loggerMock = new Mock<ILogger<SignInManager<ApplicationUser>>>();
            var authSchemeProviderMock = new Mock<IAuthenticationSchemeProvider>();
            var userConfirmationMock = new Mock<IUserConfirmation<ApplicationUser>>();

            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                optionsAccessorMock.Object,
                loggerMock.Object,
                authSchemeProviderMock.Object,
                userConfirmationMock.Object
            );

            _jwtServiceMock = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();

            _authService = new AuthenticationService(
                _usersRepositoryMock.Object,
                _signInManagerMock.Object,
                _jwtServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Register_ValidRegisterDTO_Success()
        {
            var registerDTO = new RegisterDTO();
            var user = new ApplicationUser();
            var identityResult = IdentityResult.Success;

            _mapperMock.Setup(m => m.Map<ApplicationUser>(registerDTO)).Returns(user);
            _usersRepositoryMock.Setup(r => r.CreateUser(user, registerDTO.Password)).ReturnsAsync(identityResult);
            _signInManagerMock.Setup(s => s.SignInAsync(user, false, null)).Returns(Task.CompletedTask);
            _jwtServiceMock.Setup(j => j.CreateJwtToken(user)).Returns(new AuthenticationResponse());
            _usersRepositoryMock.Setup(r => r.UpdateUser(user));

            var result = await _authService.Register(registerDTO);

            Assert.NotNull(result);
            Assert.IsType<AuthenticationResponse>(result);
        }

        [Fact]
        public async Task Login_ValidLoginDTO_Success()
        {
            var loginDTO = new LoginDTO();
            var user = new ApplicationUser();
            var signInResult = SignInResult.Success;

            _signInManagerMock.Setup(s => s.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false))
                .ReturnsAsync(signInResult);
            _usersRepositoryMock.Setup(r => r.FindByEmail(loginDTO.Email)).ReturnsAsync(user);
            _signInManagerMock.Setup(s => s.SignInAsync(user, false, null)).Returns(Task.CompletedTask);
            _jwtServiceMock.Setup(j => j.CreateJwtToken(user)).Returns(new AuthenticationResponse());
            _usersRepositoryMock.Setup(r => r.UpdateUser(user));

            var result = await _authService.Login(loginDTO);

            Assert.NotNull(result);
            Assert.IsType<AuthenticationResponse>(result);
        }

        [Fact]
        public async Task Logout_Success()
        {
            await _authService.Logout();

            _signInManagerMock.Verify(s => s.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async Task GenerateNewAccessToken_ValidTokenModel_Success()
        {
            var tokenModel = new TokenModel();
            tokenModel.RefreshToken = "refreshtoken";
            var principal = new ClaimsPrincipal();

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, "test@example.com")
            }));

            var emailClaim = new Claim(ClaimTypes.Email, "test@example.com");
            var user = new ApplicationUser();
            user.RefreshToken = "refreshtoken";
            user.RefreshTokenExpirationDateTime = DateTime.Now.AddHours(1);

            _jwtServiceMock.Setup(j => j.GetPrincipalFromJwtToken(tokenModel.Token)).Returns(principal);
            _usersRepositoryMock.Setup(r => r.FindByEmail(emailClaim.Value)).ReturnsAsync(user);
            _usersRepositoryMock.Setup(r => r.UpdateUser(user)).ReturnsAsync(IdentityResult.Success);
            _jwtServiceMock.Setup(j => j.CreateJwtToken(user)).Returns(new AuthenticationResponse());

            var result = await _authService.GenerateNewAccessToken(tokenModel);

            Assert.NotNull(result);
            Assert.IsType<AuthenticationResponse>(result);
        }
    }
}

