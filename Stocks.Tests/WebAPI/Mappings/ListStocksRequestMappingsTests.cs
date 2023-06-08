using System;
using System.Security.Claims;
using AutoMapper;
using Stocks.Core.Stock.DTO;
using Stocks.WebAPI.Mappings;

namespace Stocks.Tests.WebAPI.Mappings
{
	public class ListStocksRequestMappingsTests
    {
        private IMapper _mapper;

        public ListStocksRequestMappingsTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ListStocksRequestMapping>();
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

            var listStocksRequest = _mapper.Map<ListStocksRequest>(claimsPrincipal);

            Assert.Equal("123", listStocksRequest.UserId);
        }
    }
}

