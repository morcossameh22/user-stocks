using Microsoft.AspNetCore.Identity;
using Stocks.Core.Entities;
using Stocks.Core.Identity;
using Stocks.Core.Stock.Domain.RepositoryContracts;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.ServiceContracts;
using Stocks.Core.User.Domain.RepositoryContracts;

namespace Stocks.Core.Stock.Services
{
  public class StockService : IStockService
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IStocksRepository _stocksRepository;
    private readonly IExternalStockService _externalStockService;

    public StockService(IUsersRepository usersRepository, IStocksRepository stocksRepository, IExternalStockService externalStockService)
    {
      _usersRepository = usersRepository;
      _stocksRepository = stocksRepository;
      _externalStockService = externalStockService;
    }

    public async Task<ICollection<StockResponse>> listUserStocks(ListStocksRequest listStocksRequest)
    {
      ApplicationUser? applicationUser = await _usersRepository.FindByUserIdWithStoks(listStocksRequest.UserId);

      if (applicationUser == null)
      {
        throw new Exception("Not found");
      }

      ICollection<StockResponse> stocksResponse = new List<StockResponse>();

      foreach (StockEntity stock in applicationUser.Stocks)
      {
        Dictionary<string, object>? responseDictionary =
                await _externalStockService.GetStockPriceQuote(stock.StockSymbol);

        StockResponse stockResponse = new StockResponse()
        {
          StockSymbol = stock.StockSymbol,
          CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
          HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
          LowestPrie = Convert.ToDouble(responseDictionary["l"].ToString()),
          OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString())
        };

        stocksResponse.Add(stockResponse);
      }

      return stocksResponse;
    }

    public async Task addStockToUser(UserStockDTO userStockDTO)
    {
      ApplicationUser? applicationUser = await _usersRepository.FindByUserIdWithStoks(userStockDTO.UserId);

      if (applicationUser == null)
      {
        throw new Exception("Not found");
      }

      StockEntity stock = new StockEntity()
      {
        StockSymbol = userStockDTO.StockSymbol,
        User = applicationUser
      };

      applicationUser.Stocks.Add(stock);

      IdentityResult result = await _usersRepository.UpdateUser(applicationUser);

      if (result.Succeeded)
      {
        return;
      }
      else
      {
        string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
        throw new Exception(errorMessage);
      }
    }

    public async Task removeStockFromUser(UserStockDTO userStockDTO)
    {
      ApplicationUser? applicationUser = await _usersRepository.FindByUserIdWithStoks(userStockDTO.UserId);

      StockEntity? stock = applicationUser?.Stocks?.FirstOrDefault(s => s.StockSymbol == userStockDTO.StockSymbol);

      if (stock == null)
      {
        throw new Exception("Not found");
      }

      await _stocksRepository.removeStock(stock);
    }
  }
}

