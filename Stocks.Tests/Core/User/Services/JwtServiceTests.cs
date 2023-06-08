using System;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Stocks.Core.Identity;
using Stocks.Core.Services;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Stocks.Tests.Core.User.Services
{
	public class JwtServiceTests
	{
        private readonly JwtService _jwtService;
        private readonly Mock<IConfiguration> _configurationMock;

        public JwtServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _jwtService = new JwtService(_configurationMock.Object);
        }

        [Fact]
        public void CreateJwtToken_ValidUser_ReturnsAuthenticationResponseWithToken()
        {
            var user = new ApplicationUser
            {
                Id = new("DDE4BA55-808E-479F-BE8B-72F69913442F"),
                Email = "test@example.com",
                PersonName = "John Doe"
            };

            _configurationMock.SetupGet(c => c["Jwt:EXPIRATION_MINUTES"]).Returns("60");
            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("your-secure-key-with-a-length-of-256-bits-or-more");
            _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("issuer");
            _configurationMock.SetupGet(c => c["Jwt:Audience"]).Returns("audience");

            var result = _jwtService.CreateJwtToken(user);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.PersonName, result.PersonName);
            Assert.Equal(DateTime.Now.AddMinutes(60).Minute, result.Expiration.Minute);
            Assert.NotEmpty(result.RefreshToken);
            Assert.Equal(DateTime.Now.AddMinutes(60).Minute, result.RefreshTokenExpirationDateTime.Minute);
        }

        [Fact]
        public void GetPrincipalFromJwtToken_ValidToken_ReturnsClaimsPrincipal()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI4MmZkNjJhZS03YmIzLTRhNTMtYzcxZi0wOGRiNWM3ZTVjNTQiLCJqdGkiOiJjYTUwYmVkYS1hNTk3LTQxZGItOWEwNi1hYWZjYzdkNzYwMzMiLCJpYXQiOiIyNi8wNS8yMDIzIDU6MzA6MjIgUE0iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Im1vcmNvc3NhbWVoKzJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Ik1vcmNvcyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1vcmNvc3NhbWVoKzJAZ21haWwuY29tIiwiZXhwIjoxNjg3NzUwMjIyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjcyMjEiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAifQ.ryLGI_LcvzNgp18rTByEjFJGHZo9dSHnyh1q3osUNpc";

            _configurationMock.SetupGet(c => c["Jwt:Audience"]).Returns("http://localhost:4200");
            _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("http://localhost:7221");
            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("this is secret key for jwt");

            var result = _jwtService.GetPrincipalFromJwtToken(token);

            Assert.NotNull(result);
            Assert.IsType<ClaimsPrincipal>(result);
        }

        //[Fact]
        //public void GetPrincipalFromJwtToken_InvalidToken_ThrowsSecurityTokenException()
        //{
        //    // Arrange
        //    var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI4MmZkNjJhZS03YmIzLTRhNTMtYzcxZi0wOGRiNWM3ZTVjNTQiLCJqdGkiOiJjYTUwYmVkYS1hNTk3LTQxZGItOWEwNi1hYWZjYzdkNzYwMzMiLCJpYXQiOiIyNi8wNS8yMDIzIDU6MzA6MjIgUE0iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Im1vcmNvc3NhbWVoKzJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Ik1vcmNvcyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1vcmNvc3NhbWVoKzJAZ21haWwuY29tIiwiZXhwIjoxNjg3NzUwMjIyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjcyMjEiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAifQ.ryLGI_LcvzNgp18rTByEjFJGHZo9dSHnyh1q3osUNpc";

        //    _configurationMock.SetupGet(c => c["Jwt:Audience"]).Returns("http://localhost:4200");
        //    _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("http://localhost:7221");
        //    _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("this is secretkey for jwt");

        //    // Act & Assert
        //    Assert.Throws<SecurityTokenException>(() => _jwtService.GetPrincipalFromJwtToken(token));
        //}
    }
}

