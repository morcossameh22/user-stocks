using System;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Stocks.Core.Entities;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.Mappings;
using Stocks.WebAPI.Mappings;

namespace Stocks.Tests.Core.Stock.Mappings
{
	public class StockResponseMappingTests
    {
        private IMapper _mapper;

        public StockResponseMappingTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StockResponseMapping>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapStockEntityToStockResponse()
        {
            var stockEntity = new StockEntity {
                StockSymbol = "AAPL",
                UserId = Guid.NewGuid()
            };

            var stockResponse = _mapper.Map<StockResponse>(stockEntity);

            Assert.Equal("AAPL", stockResponse.StockSymbol);
        }

        [Fact]
        public void ShouldMapDictionayToStockResponse()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary["c"] = 189.4;
            dictionary["h"] = 191.2;
            dictionary["l"] = 182.5;
            dictionary["o"] = 184.3;

            var stockResponse = _mapper.Map<StockResponse>(dictionary);

            Assert.Equal(dictionary["c"], stockResponse.CurrentPrice);
            Assert.Equal(dictionary["h"], stockResponse.HighestPrice);
            Assert.Equal(dictionary["l"], stockResponse.LowestPrice);
            Assert.Equal(dictionary["o"], stockResponse.OpenPrice);
        }
    }
}

