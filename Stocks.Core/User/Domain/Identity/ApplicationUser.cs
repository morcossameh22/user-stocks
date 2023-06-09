using Microsoft.AspNetCore.Identity;

using Stocks.Core.Entities;

namespace Stocks.Core.Identity
{
    /* The class ApplicationUser extends IdentityUser and includes properties for person name, refresh
    token, refresh token expiration date time, and a collection of stock entities. */
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDateTime { get; set; }
        public ICollection<StockEntity> Stocks { get; set; } = new List<StockEntity>();
    }
}
