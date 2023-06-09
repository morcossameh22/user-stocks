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
    /* The StockService class implements methods to list, add, and remove stocks for a user using
    repositories and an external stock service. */
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

        /// <summary>
        /// This function lists the stocks owned by a user and retrieves their current price quotes from
        /// an external stock service.
        /// </summary>
        /// <param name="ListStocksRequest">A request object that contains the user ID for which the
        /// stocks need to be listed.</param>
        /// <returns>
        /// The method `ListUserStocks` returns a `Task` that resolves to an `ICollection` of
        /// `StockResponse` objects.
        /// </returns>
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

        /// <summary>
        /// This function adds a stock to a user's list of stocks and updates the user's information in
        /// the repository.
        /// </summary>
        /// <param name="UserStockDTO">UserStockDTO is a data transfer object that contains information
        /// about a user and a stock. It has two properties: UserId (string) and StockSymbol
        /// (string).</param>
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

        /// <summary>
        /// This function removes a stock from a user's list of stocks.
        /// </summary>
        /// <param name="UserStockDTO">UserStockDTO is a data transfer object that contains information
        /// about a user's stock. It likely includes properties such as UserId, StockSymbol, and
        /// possibly other information related to the stock.</param>
        public async Task RemoveStockFromUser(UserStockDTO userStockDTO)
        {
            ApplicationUser? applicationUser = await _usersRepository.FindByUserIdWithStoks(userStockDTO.UserId);

            StockEntity? stock = (applicationUser?.Stocks?.FirstOrDefault(s => s.StockSymbol == userStockDTO.StockSymbol))
                    ?? throw new Exception(CoreConstants.NotFoundMessage);

            await _stocksRepository.RemoveStock(stock);
        }
    }
}
