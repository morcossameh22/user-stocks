using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.Stock.DTO
{
    public class UserStockDTO
    {
        [Required(ErrorMessage = CoreConstants.BlankStockSymbol)]
        public string StockSymbol { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }
}
