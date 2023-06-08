using Microsoft.EntityFrameworkCore;
using Moq;
using Stocks.Core.Entities;
using Stocks.Core.Stock.Domain.RepositoryContracts;
using Stocks.Infrastructure.DbContext;
using Stocks.Infrastructure.ExternalServices;
using Stocks.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Infrastructure.Tests.Repositories
{
    public class StocksRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly Mock<DbSet<StockEntity>> _dbSetMock;

        public StocksRepositoryTests()
        {
            _dbContextMock = new Mock<ApplicationDbContext>();
            _dbSetMock = new Mock<DbSet<StockEntity>>();
            _dbContextMock.Setup(c => c.Stocks).Returns(_dbSetMock.Object);
        }

        [Fact]
        public async Task RemoveStock_ValidStockEntity_StockRemovedSuccessfully()
        {
            var stockEntity = new StockEntity {
                UserId = Guid.NewGuid(),
                StockSymbol = "AAPL"
            };

            _dbSetMock.Setup(d => d.Remove(stockEntity));
            _dbContextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var repository = new StocksRepository(_dbContextMock.Object);

            await repository.RemoveStock(stockEntity);

            _dbSetMock.Verify(d => d.Remove(stockEntity), Times.Once);
            _dbContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task RemoveStock_InvalidStockEntity_ExceptionThrown()
        {
            // Arrange
            var stockEntity = new StockEntity
            {
                UserId = Guid.NewGuid(),
                StockSymbol = "AAPL"
            };

            _dbSetMock.Setup(d => d.Remove(stockEntity));

            var repository = new StocksRepository(_dbContextMock.Object);

            //await repository.RemoveStock(stockEntity);
            await Assert.ThrowsAsync<Exception>(() => repository.RemoveStock(stockEntity));

            _dbSetMock.Verify(d => d.Remove(stockEntity), Times.Once);
            _dbContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}