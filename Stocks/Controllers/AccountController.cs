using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Stocks.Core.DTO;
using Stocks.Core.User.ServiceContracts;

namespace Stocks.WebAPI.Controllers
{
    /* This class is used to handle HTTP requests related to user authentication and account
    management. It contains methods for user registration, login, logout, and generating new
    JWT tokens for authentication. */
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// This is a POST request to register a user and returns an authentication response.
        /// </summary>
        /// <param name="RegisterDTO">RegisterDTO is a data transfer object (DTO) that contains the
        /// necessary information for registering a new user. It typically includes properties such as
        /// username, email, password, and any other relevant user information.</param>
        /// <returns>
        /// The method is returning an `ActionResult` of type `AuthenticationResponse`.
        /// </returns>
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

        /// <summary>
        /// This is a POST request for user authentication and returns an authentication response.
        /// </summary>
        /// <param name="LoginDTO">LoginDTO is a data transfer object (DTO) that contains the user's
        /// login credentials, such as their email or username and password. It is used to pass this
        /// information from the client-side to the server-side for authentication purposes.</param>
        /// <returns>
        /// The method is returning an asynchronous task that will eventually return an ActionResult of
        /// type AuthenticationResponse.
        /// </returns>
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

        /// <summary>
        /// This function handles a HTTP DELETE request to log out a user and returns a NoContent
        /// response.
        /// </summary>
        /// <returns>
        /// The method is returning a `NoContent` result, which indicates that the server has
        /// successfully processed the request and is not returning any content in the response body.
        /// </returns>
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();

            return NoContent();
        }

        /// <summary>
        /// This is a HTTP POST endpoint that generates a new JWT token for authentication.
        /// </summary>
        /// <param name="TokenModel">TokenModel is a custom model class that contains the necessary
        /// information to generate a new JWT token. It includes the current token and the refresh 
        /// token. The controller action is using this model to generate a new access token for the
        /// user.</param>
        /// <returns>
        /// An `ActionResult` of type `AuthenticationResponse` is being returned.
        /// </returns>
        [HttpPost("generate-new-jwt-token")]
        public async Task<ActionResult<AuthenticationResponse>> GenerateNewAccessToken(TokenModel tokenModel)
        {
            return await _authenticationService.GenerateNewAccessToken(tokenModel);
        }
    }
}

