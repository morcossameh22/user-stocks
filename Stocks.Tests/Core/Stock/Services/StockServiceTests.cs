using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Stocks.Core.Entities;
using Stocks.Core.Identity;
using Stocks.Core.Stock.Domain.RepositoryContracts;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.ServiceContracts;
using Stocks.Core.Stock.Services;
using Stocks.Core.User.Domain.RepositoryContracts;

namespace Stocks.Tests.Core.Stock.Services
{
	public class StockServiceTests
	{
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly Mock<IExternalStockService> _externalStockServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IStockService _stockService;

        public StockServiceTests()
        {
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _externalStockServiceMock = new Mock<IExternalStockService>();
            _mapperMock = new Mock<IMapper>();

            _stockService = new StockService(
                _usersRepositoryMock.Object,
                _stocksRepositoryMock.Object,
                _externalStockServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task ListUserStocks_ShouldReturnStockResponses_WhenUserExists()
        {
            var listStocksRequest = new ListStocksRequest { UserId = "123" };

            var applicationUser = new ApplicationUser();
            applicationUser.Stocks.Add(new StockEntity { StockSymbol = "AAPL" });

            var stockEntity = new StockEntity();
            var responseDictionary = new Dictionary<string, object>();

            _usersRepositoryMock.Setup(repo => repo.FindByUserIdWithStoks(listStocksRequest.UserId))
                .ReturnsAsync(applicationUser);

            _externalStockServiceMock.Setup(service => service.GetStockPriceQuote(stockEntity.StockSymbol))
                .ReturnsAsync(responseDictionary);

            _mapperMock.Setup(mapper => mapper.Map<StockResponse>(stockEntity))
                .Returns(new StockResponse());

            var result = await _stockService.ListUserStocks(listStocksRequest);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<StockResponse>>(result);
        }

        [Fact]
        public async Task AddStockToUser_ShouldAddStockToUserAndReturnSuccessfulResult()
        {
            var userStockDto = new UserStockDTO { UserId = "123", StockSymbol = "AAPL" };
            var applicationUser = new ApplicationUser();
            var identityResult = IdentityResult.Success;

            _usersRepositoryMock.Setup(repo => repo.FindByUserIdWithStoks(userStockDto.UserId))
                .ReturnsAsync(applicationUser);

            _usersRepositoryMock.Setup(repo => repo.UpdateUser(applicationUser))
                .ReturnsAsync(identityResult);

            await _stockService.AddStockToUser(userStockDto);

            _usersRepositoryMock.Verify(repo => repo.FindByUserIdWithStoks(It.IsAny<string>()), Times.Once);
            _usersRepositoryMock.Verify(repo => repo.UpdateUser(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task RemoveStockFromUser_ShouldRemoveStockFromUser()
        {
            var userStockDto = new UserStockDTO { UserId = "123", StockSymbol = "AAPL" };
            var applicationUser = new ApplicationUser();
            applicationUser.Stocks.Add(new StockEntity{ StockSymbol = "AAPL" });

            _usersRepositoryMock.Setup(repo => repo.FindByUserIdWithStoks(userStockDto.UserId))
                .ReturnsAsync(applicationUser);

            _stocksRepositoryMock.Setup(repo => repo.RemoveStock(It.IsAny<StockEntity>()));

            await _stockService.RemoveStockFromUser(userStockDto);

            _stocksRepositoryMock.Verify(repo => repo.RemoveStock(It.IsAny<StockEntity>()), Times.Once);
        }
    }
}

