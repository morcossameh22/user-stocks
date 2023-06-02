using Stocks.Core.Entities;

namespace Stocks.Core.Stock.Domain.RepositoryContracts
{
    public interface IStocksRepository
    {
        Task RemoveStock(StockEntity stockEntity);
    }
}

