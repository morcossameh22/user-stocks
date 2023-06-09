namespace Stocks.Core.Stock.ServiceContracts
{
    /* This is a interface definition for an external stock service.*/
    public interface IExternalStockService
    {
        Task<Dictionary<string, object>> GetStockPriceQuote(string? stockSymbol);
    }
}

