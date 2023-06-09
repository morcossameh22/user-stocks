using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.Stock.DTO
{
    /* The UserStockDTO class represents a user's stock with a required stock symbol and optional user
    ID. */
    public class UserStockDTO
    {
        [Required(ErrorMessage = CoreConstants.BlankStockSymbol)]
        public string StockSymbol { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }
}
