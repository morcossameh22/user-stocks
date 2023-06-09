using Stocks.Core.Entities;
using Stocks.Core.Stock.Domain.RepositoryContracts;
using Stocks.Infrastructure.DbContext;

namespace Stocks.Infrastructure.Repositories
{
    /* The StocksRepository class implements the IStocksRepository interface. */
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _context;

        public StocksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This function removes a stock entity from the context and throws an exception if the removal
        /// fails.
        /// </summary>
        /// <param name="StockEntity">StockEntity is an entity class representing a stock in a database.
        /// It contains properties that map to columns in the database table for stocks.</param>
        public async Task RemoveStock(StockEntity stockEntity)
        {
            _context.Stocks.Remove(stockEntity);

            int result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new Exception(InfrastructureConstants.RemoveStockFailed);
            }
        }
    }
}

