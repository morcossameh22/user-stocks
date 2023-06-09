using Stocks.Core.Stock.DTO;

namespace Stocks.Core.Stock.ServiceContracts
{
    /* This is an interface called `IStockService` that defines three methods related to managing
    stocks for a user. */
    public interface IStockService
    {
        Task<ICollection<StockResponse>> ListUserStocks(ListStocksRequest listStocksRequest);
        Task AddStockToUser(UserStockDTO userStockDTO);
        Task RemoveStockFromUser(UserStockDTO userStockDTO);
    }
}
