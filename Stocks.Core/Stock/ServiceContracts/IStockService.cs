using Stocks.Core.Stock.DTO;

namespace Stocks.Core.Stock.ServiceContracts
{
    public interface IStockService
    {
        Task<ICollection<StockResponse>> ListUserStocks(ListStocksRequest listStocksRequest);
        Task AddStockToUser(UserStockDTO userStockDTO);
        Task RemoveStockFromUser(UserStockDTO userStockDTO);
    }
}
