using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stocks.Core.DTO;
using Stocks.Core.User.ServiceContracts;
using Stocks.WebAPI.Controllers;

namespace Stocks.ControllerTests;

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

        accountController.ModelState.AddModelError("Email", "The Email field is required.");

        var result = await accountController.PostRegister(registerDTO);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        var errorMessage = problemDetails.Detail;
        Assert.NotNull(objectResult);
        Assert.Equal("The Email field is required.", errorMessage);
    }
}
