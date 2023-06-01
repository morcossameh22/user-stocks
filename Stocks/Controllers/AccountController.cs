using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Stocks.Core.DTO;
using Stocks.Core.User.ServiceContracts;

namespace Stocks.WebAPI.Controllers
{
  [AllowAnonymous]
  public class AccountController : ControllerBase
  {
    private readonly IAuthenticationService _authenticationService;

    public AccountController(IAuthenticationService authenticationService)
    {
      _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> PostRegister(RegisterDTO registerDTO)
    {
      if (ModelState.IsValid == false)
      {
        string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return Problem(errorMessage);
      }

      AuthenticationResponse response = await _authenticationService.Register(registerDTO);

      return response;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> PostLogin(LoginDTO loginDTO)
    {
      if (ModelState.IsValid == false)
      {
        string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return Problem(errorMessage);
      }

      return await _authenticationService.Login(loginDTO);
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
      await _authenticationService.Logout();

      return NoContent();
    }

    [HttpPost("generate-new-jwt-token")]
    public async Task<ActionResult<AuthenticationResponse>> GenerateNewAccessToken(TokenModel tokenModel)
    {
      return await _authenticationService.GenerateNewAccessToken(tokenModel);
    }
  }
}

