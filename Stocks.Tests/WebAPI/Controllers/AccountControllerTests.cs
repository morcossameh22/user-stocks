using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stocks.Core.DTO;
using Stocks.Core.User.ServiceContracts;
using Stocks.WebAPI.Controllers;

namespace Stocks.Tests.WebAPI.ControllerTests;

public class AccountControllerTests
{
    private readonly IAuthenticationService _authenticationService;

    private readonly Mock<IAuthenticationService> _authenticationServiceMock;

    private readonly Fixture _fixture;


    public AccountControllerTests()
    {
        _fixture = new Fixture();

        _authenticationServiceMock = new Mock<IAuthenticationService>();

        _authenticationService = _authenticationServiceMock.Object;
    }

    [Fact]
    public async Task Register_ShouldReturnAuthenticationResponse()
    {
        RegisterDTO registerDTO = _fixture.Create<RegisterDTO>();

        AuthenticationResponse authenticationResponse = _fixture.Create<AuthenticationResponse>();

        AccountController accountController = new AccountController(_authenticationService);

        _authenticationServiceMock
         .Setup(temp => temp.Register(It.IsAny<RegisterDTO>()))
         .ReturnsAsync(authenticationResponse);

        ActionResult<AuthenticationResponse> result = await accountController.PostRegister(registerDTO);

        Assert.IsType<ActionResult<AuthenticationResponse>>(result);
    }

    [Fact]
    public async Task Register_ShouldReturnError()
    {
        RegisterDTO registerDTO = _fixture.Create<RegisterDTO>();

        AccountController accountController = new AccountController(_authenticationService);

        string ExpectedErrorMessage = "The Email field is required.";

        accountController.ModelState.AddModelError("Email", ExpectedErrorMessage);

        var result = await accountController.PostRegister(registerDTO);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        var errorMessage = problemDetails.Detail;
        Assert.NotNull(objectResult);
        Assert.Equal(ExpectedErrorMessage, errorMessage);
    }

    [Fact]
    public async Task Login_ShouldReturnAuthenticationResponse()
    {
        LoginDTO loginDTO = _fixture.Create<LoginDTO>();

        AuthenticationResponse authenticationResponse = _fixture.Create<AuthenticationResponse>();

        AccountController accountController = new AccountController(_authenticationService);

        _authenticationServiceMock
         .Setup(temp => temp.Login(It.IsAny<LoginDTO>()))
         .ReturnsAsync(authenticationResponse);

        ActionResult<AuthenticationResponse> result = await accountController.PostLogin(loginDTO);

        Assert.IsType<ActionResult<AuthenticationResponse>>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnError()
    {
        LoginDTO loginDTO = _fixture.Create<LoginDTO>();

        AccountController accountController = new AccountController(_authenticationService);

        string ExpectedErrorMessage = "The Email field is required.";

        accountController.ModelState.AddModelError("Email", ExpectedErrorMessage);

        var result = await accountController.PostLogin(loginDTO);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        var errorMessage = problemDetails.Detail;
        Assert.NotNull(objectResult);
        Assert.Equal(ExpectedErrorMessage, errorMessage);
    }

    [Fact]
    public async Task Logout_ShouldReturnNoContentResult()
    {

        AccountController accountController = new AccountController(_authenticationService);

        _authenticationServiceMock
         .Setup(temp => temp.Logout());

        IActionResult result = await accountController.Logout();

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GenerateNewAccessToken_ShouldReturnAuthenticationResponse()
    {
        TokenModel tokenModel = _fixture.Create<TokenModel>();

        AuthenticationResponse authenticationResponse = _fixture.Create<AuthenticationResponse>();

        AccountController accountController = new AccountController(_authenticationService);

        _authenticationServiceMock
         .Setup(temp => temp.GenerateNewAccessToken(It.IsAny<TokenModel>()))
         .ReturnsAsync(authenticationResponse);

        ActionResult<AuthenticationResponse> result =
                await accountController.GenerateNewAccessToken(tokenModel);

        Assert.IsType<ActionResult<AuthenticationResponse>>(result);
    }
}
