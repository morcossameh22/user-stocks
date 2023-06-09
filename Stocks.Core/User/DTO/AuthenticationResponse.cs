namespace Stocks.Core.DTO
{
    /* The class "AuthenticationResponse" contains properties for person name, email, token, expiration
    date, refresh token, and refresh token expiration date. */
    public class AuthenticationResponse
    {
        public string? PersonName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpirationDateTime { get; set; }
    }
}
