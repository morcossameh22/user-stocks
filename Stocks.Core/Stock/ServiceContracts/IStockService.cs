using Stocks.Core.Stock.DTO;

namespace Stocks.Core.Stock.ServiceContracts
{
  public interface IStockService
  {
    Task<ICollection<StockResponse>> listUserStocks(ListStocksRequest listStocksRequest);
    Task addStockToUser(UserStockDTO userStockDTO);
    Task removeStockFromUser(UserStockDTO userStockDTO);
  }
}
