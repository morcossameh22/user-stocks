using System;
using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.Stock.DTO
{
	public class UserStockDTO
	{
        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        public string StockSymbol { get; set; } = string.Empty;
    }
}
