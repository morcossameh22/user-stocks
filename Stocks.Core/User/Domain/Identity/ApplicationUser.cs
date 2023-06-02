using Microsoft.AspNetCore.Identity;

using Stocks.Core.Entities;

namespace Stocks.Core.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDateTime { get; set; }
        public ICollection<StockEntity> Stocks { get; set; } = new List<StockEntity>();
    }
}
