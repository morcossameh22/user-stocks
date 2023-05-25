using System;
using System.ComponentModel.DataAnnotations;
using Stocks.Core.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stocks.Core.Entities
{
	public class Stock
	{
        [Key]
        public Guid StockID { get; set; }

        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        public string? StockSymbol { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}

