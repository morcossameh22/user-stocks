using Stocks.Core.Entities;

namespace Stocks.Core.Stock.Domain.RepositoryContracts
{
    /* This is an interface declaration for a repository that deals with stocks. */
    public interface IStocksRepository
    {
        Task RemoveStock(StockEntity stockEntity);
    }
}

