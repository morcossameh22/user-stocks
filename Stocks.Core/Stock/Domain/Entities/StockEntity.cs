﻿using System.ComponentModel.DataAnnotations;
using Stocks.Core.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stocks.Core.Entities
{
    /* This is a class representing a stock entity with properties such as StockID, StockSymbol,
    UserId, and User. */
    public class StockEntity
    {
        [Key]
        public Guid StockID { get; set; } = new Guid();

        [Required(ErrorMessage = CoreConstants.BlankStockSymbol)]
        public string? StockSymbol { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}

