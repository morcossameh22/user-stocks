namespace Stocks.Core.Stock.DTO
{
    /* The StockResponse class contains properties for a stock's symbol, current price, lowest price,
    highest price, and open price. */
    public class StockResponse
    {
        public string? StockSymbol { get; set; }
        public double CurrentPrice { get; set; }
        public double LowestPrice { get; set; }
        public double HighestPrice { get; set; }
        public double OpenPrice { get; set; }
    }
}

