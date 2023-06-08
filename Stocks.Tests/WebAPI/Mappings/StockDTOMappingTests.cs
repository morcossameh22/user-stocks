using System;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Stocks.Core.Stock.DTO;
using Stocks.WebAPI.Mappings;

namespace Stocks.Tests.WebAPI.Mappings
{
	public class StockDTOMappingTests
	{
        private IMapper _mapper;

        public StockDTOMappingTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StockDTOMapping>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapClaimsPrincipalToUserStockDTO()
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "John Doe")
            }));

            var userStockDTO = _mapper.Map<UserStockDTO>(claimsPrincipal);

            Assert.Equal("123", userStockDTO.UserId);
            Assert.Equal("", userStockDTO.StockSymbol);
        }

        [Fact]
        public void ShouldMapStringToUserStockDTO()
        {
            var stockSymbol = "AAPL";

            var userStockDTO = _mapper.Map<UserStockDTO>(stockSymbol);

            Assert.Equal("", userStockDTO.UserId);
            Assert.Equal("AAPL", userStockDTO.StockSymbol);
        }
    }
}

