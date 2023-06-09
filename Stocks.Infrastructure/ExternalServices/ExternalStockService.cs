using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Stocks.Core.Stock.ServiceContracts;

namespace Stocks.Infrastructure.ExternalServices
{
    /* The ExternalStockService class retrieves a stock price quote from the Finnhub API using a
    provided stock symbol and returns it as a dictionary object. */
    public class ExternalStockService : IExternalStockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ExternalStockService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// This function retrieves a stock price quote from the Finnhub API using a provided stock
        /// symbol and returns it as a dictionary object.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol for which the price quote is being
        /// requested.</param>
        /// <returns>
        /// The method is returning a `Task` that will eventually produce a `Dictionary<string, object>`
        /// object.
        /// </returns>
        public async Task<Dictionary<string, object>> GetStockPriceQuote(string? stockSymbol)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new()
            {
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                Method = HttpMethod.Get
            };

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            Stream stream = httpResponseMessage.Content.ReadAsStream();

            StreamReader streamReader = new(stream);

            string response = streamReader.ReadToEnd();
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response)
                  ?? throw new InvalidOperationException(InfrastructureConstants.FinnhubError);

            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            return responseDictionary;
        }
    }
}

