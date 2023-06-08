using System;
using Moq;
using Stocks.Infrastructure.ExternalServices;
using System.Net;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using RichardSzalay.MockHttp;
using System.Net.Http;

namespace Stocks.Tests.Infrastructure.ExternalServices
{
	public class ExternalStockServiceTests
	{
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly Mock<HttpResponseMessage> _httpResponseMessageMock;
        private readonly ExternalStockService _externalStockService;

        public ExternalStockServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationMock = new Mock<IConfiguration>();
            _httpClientMock = new Mock<HttpClient>();
            _httpResponseMessageMock = new Mock<HttpResponseMessage>();

            _externalStockService = new ExternalStockService(_httpClientFactoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task GetStockPriceQuote_ValidSymbol_ReturnsResponseDictionary()
        {

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://finnhub.io/api/v1/quote?symbol=AAPL&token=YOUR_FINNHUB_TOKEN")
                    .Respond("application/json", GetMockResponseJson());

            var client = new HttpClient(mockHttp);

            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            _configurationMock.Setup(c => c["FinnhubToken"]).Returns("YOUR_FINNHUB_TOKEN");

            var responseDictionary = await _externalStockService.GetStockPriceQuote("AAPL");

            Assert.NotNull(responseDictionary);
            Assert.Equal("AAPL", responseDictionary["symbol"].ToString());
            Assert.Equal("100.0", responseDictionary["price"].ToString());
        }

        [Fact]
        public async Task GetStockPriceQuote_ErrorResponse_ThrowsInvalidOperationException()
        {
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(_httpClientMock.Object);
            _configurationMock.Setup(c => c["FinnhubToken"]).Returns("YOUR_FINNHUB_TOKEN");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _externalStockService.GetStockPriceQuote("AAPL"));
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static string GetMockResponseJson()
        {
            return @"{
                ""symbol"": ""AAPL"",
                ""price"": 100.0
            }";
        }
    }
}

