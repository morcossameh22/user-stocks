namespace Stocks.Core.Stock.ServiceContracts
{
  public interface IExternalStockService
  {
    Task<Dictionary<string, object>> GetStockPriceQuote(string? stockSymbol);
  }
}

