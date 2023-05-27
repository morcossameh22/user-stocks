using Stocks.Core.Entities;

namespace Stocks.Core.Stock.Domain.RepositoryContracts
{
  public interface IStocksRepository
  {
    Task removeStock(StockEntity stockEntity);
  }
}

