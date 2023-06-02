using AutoMapper;

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
        private readonly IMapper _mapper;

        public StockService(IUsersRepository usersRepository, IStocksRepository stocksRepository, IExternalStockService externalStockService, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _stocksRepository = stocksRepository;
            _externalStockService = externalStockService;
            _mapper = mapper;
        }

        public async Task<ICollection<StockResponse>> ListUserStocks(ListStocksRequest listStocksRequest)
        {
            ApplicationUser? applicationUser = await _usersRepository.FindByUserIdWithStoks(listStocksRequest.UserId)
                      ?? throw new Exception(CoreConstants.NotFoundMessage);

            ICollection<StockResponse> stocksResponse = new List<StockResponse>();

            foreach (StockEntity stock in applicationUser.Stocks)
            {
                Dictionary<string, object>? responseDictionary =
                        await _externalStockService.GetStockPriceQuote(stock.StockSymbol);

                StockResponse stockResponse = _mapper.Map<StockResponse>(stock);
                _mapper.Map(responseDictionary, stockResponse);

                stocksResponse.Add(stockResponse);
            }

            return stocksResponse;
        }

        public async Task AddStockToUser(UserStockDTO userStockDTO)
        {
            ApplicationUser? applicationUser = await _usersRepository.FindByUserIdWithStoks(userStockDTO.UserId)
                    ?? throw new Exception(CoreConstants.NotFoundMessage);

            StockEntity stock = new()
            {
                StockSymbol = userStockDTO.StockSymbol,
                User = applicationUser
            };

            applicationUser.Stocks.Add(stock);

            IdentityResult result = await _usersRepository.UpdateUser(applicationUser);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                throw new Exception(errorMessage);
            }
        }

        public async Task RemoveStockFromUser(UserStockDTO userStockDTO)
        {
            ApplicationUser? applicationUser = await _usersRepository.FindByUserIdWithStoks(userStockDTO.UserId);

            StockEntity? stock = (applicationUser?.Stocks?.FirstOrDefault(s => s.StockSymbol == userStockDTO.StockSymbol))
                    ?? throw new Exception(CoreConstants.NotFoundMessage);

            await _stocksRepository.RemoveStock(stock);
        }
    }
}
