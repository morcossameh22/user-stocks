using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Stocks.Core.DTO;
using Stocks.Core.Identity;
using Stocks.Core.ServiceContracts;
using Stocks.Core.User.Domain.RepositoryContracts;
using Stocks.Core.User.ServiceContracts;

namespace Stocks.Core.User.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IUsersRepository _userRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthenticationService(IUsersRepository usersRepository, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
    {
      _userRepository = usersRepository;
      _signInManager = signInManager;
      _jwtService = jwtService;
    }

    public async Task<AuthenticationResponse> Register(RegisterDTO registerDTO)
    {
      ApplicationUser user = new ApplicationUser()
      {
        Email = registerDTO.Email,
        PhoneNumber = registerDTO.PhoneNumber,
        UserName = registerDTO.Email,
        PersonName = registerDTO.PersonName
      };

      IdentityResult result = await _userRepository.CreateUser(user, registerDTO.Password);

      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, isPersistent: false);
        return await GenerateToken(user);
      }
      else
      {
        string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
        throw new Exception(errorMessage);
      }
    }


    public async Task<AuthenticationResponse> Login(LoginDTO loginDTO)
    {
      var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

      if (result.Succeeded)
      {
        ApplicationUser? user = await _userRepository.FindByEmail(loginDTO.Email);

        if (user == null)
        {
          throw new Exception("User not found");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return await GenerateToken(user);
      }

      else
      {
        throw new Exception("Invalid email or password");
      }
    }

    public async Task Logout()
    {
      await _signInManager.SignOutAsync();
    }

    public async Task<AuthenticationResponse> GenerateNewAccessToken(TokenModel tokenModel)
    {
      if (tokenModel == null)
      {
        throw new Exception("Invalid client request");
      }

      ClaimsPrincipal? principal = _jwtService.GetPrincipalFromJwtToken(tokenModel.Token);
      if (principal == null)
      {
        throw new Exception("Invalid jwt access token");
      }

      string? email = principal.FindFirstValue(ClaimTypes.Email);

      ApplicationUser? user = await _userRepository.FindByEmail(email);

      if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.Now)
      {
        throw new Exception("Invalid refresh token");
      }

      return await GenerateToken(user);
    }

    private async Task<AuthenticationResponse> GenerateToken(ApplicationUser user)
    {
      var authenticationResponse = _jwtService.CreateJwtToken(user);
      user.RefreshToken = authenticationResponse.RefreshToken;
      user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
      await _userRepository.UpdateUser(user);

      return authenticationResponse;
    }
  }
}

