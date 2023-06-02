using Stocks.Core.Entities;
using Stocks.Core.Stock.Domain.RepositoryContracts;
using Stocks.Infrastructure.DbContext;

namespace Stocks.Infrastructure.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _context;

        public StocksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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

