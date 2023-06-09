using System.Security.Claims;

using AutoMapper;

using Microsoft.AspNetCore.Identity;

using Stocks.Core.DTO;
using Stocks.Core.Identity;
using Stocks.Core.ServiceContracts;
using Stocks.Core.User.Domain.RepositoryContracts;
using Stocks.Core.User.ServiceContracts;

namespace Stocks.Core.User.Services
{
    /* The AuthenticationService class handles user authentication and token generation using a
    repository, sign-in manager, JWT service, and mapper. */
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthenticationService(IUsersRepository usersRepository, SignInManager<ApplicationUser> signInManager, IJwtService jwtService, IMapper mapper)
        {
            _userRepository = usersRepository;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        /// <summary>
        /// This function registers a new user and generates an authentication token if successful.
        /// </summary>
        /// <param name="RegisterDTO">RegisterDTO is a data transfer object (DTO) that contains the
        /// information needed to register a new user. It typically includes properties such as email,
        /// password, and any other relevant user information.</param>
        /// <returns>
        /// The method is returning a `Task` that will eventually resolve to an `AuthenticationResponse`
        /// object.
        /// </returns>
        public async Task<AuthenticationResponse> Register(RegisterDTO registerDTO)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(registerDTO);

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

        /// <summary>
        /// This function logs in a user with their email and password, generates a token, and returns
        /// an authentication response.
        /// </summary>
        /// <param name="LoginDTO">A data transfer object (DTO) that contains the user's email and
        /// password for authentication.</param>
        /// <returns>
        /// The method is returning an object of type `Task<AuthenticationResponse>`.
        /// </returns>
        public async Task<AuthenticationResponse> Login(LoginDTO loginDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(
              loginDTO.Email,
              loginDTO.Password,
              isPersistent: false,
              lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userRepository.FindByEmail(loginDTO.Email)
                      ?? throw new Exception(CoreConstants.UserNotFound);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return await GenerateToken(user);
            }

            else
            {
                throw new Exception(CoreConstants.InvalidEmailPass);
            }
        }

        /// <summary>
        /// The function logs out the current user by calling the SignOutAsync method of the
        /// SignInManager.
        /// </summary>
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// This function generates a new access token for a user based on their refresh token and
        /// verifies their identity.
        /// </summary>
        /// <param name="TokenModel">TokenModel is a class that contains the access token and refresh
        /// token sent by the client. It is used to authenticate the user and generate a new access
        /// token.</param>
        /// <returns>
        /// The method `GenerateNewAccessToken` returns a `Task` that resolves to an
        /// `AuthenticationResponse` object.
        /// </returns>
        public async Task<AuthenticationResponse> GenerateNewAccessToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                throw new Exception(CoreConstants.InvalidClientReq);
            }

            ClaimsPrincipal? principal = _jwtService.GetPrincipalFromJwtToken(tokenModel.Token)
                      ?? throw new Exception(CoreConstants.InvalidAccessToken);

            string? email = principal.FindFirstValue(ClaimTypes.Email)
                  ?? throw new Exception(CoreConstants.NotFoundMessage); ;

            ApplicationUser? user = await _userRepository.FindByEmail(email);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.Now)
            {
                throw new Exception(CoreConstants.InvalidRefreshToken);
            }

            return await GenerateToken(user);
        }

        /// <summary>
        /// This function generates a JWT token for a given user and updates their refresh token and
        /// expiration date in the user repository.
        /// </summary>
        /// <param name="ApplicationUser">ApplicationUser is a class that represents a user in the
        /// application. In this context, it is used to generate a JWT token for the user.</param>
        /// <returns>
        /// The method is returning an object of type `Task<AuthenticationResponse>`.
        /// </returns>
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

