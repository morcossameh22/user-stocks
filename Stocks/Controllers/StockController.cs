using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.ServiceContracts;

namespace Stocks.WebAPI.Controllers
{
    /* This class is responsible for handling HTTP requests and returning HTTP responses related to
    stock operations. */
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public StockController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        /// <summary>
        /// This function retrieves a list of stocks belonging to the current user and returns them as a
        /// collection of StockResponse objects.
        /// </summary>
        /// <returns>
        /// The method is returning an HTTP response with a collection of StockResponse objects in the
        /// body. The response is an ActionResult object that wraps the collection of StockResponse
        /// objects and includes an HTTP status code indicating the success or failure of the request.
        /// In this case, the HTTP status code is 200 (OK) if the request is successful.
        /// </returns>
        [HttpGet("user-stocks")]
        public async Task<ActionResult<ICollection<StockResponse>>> GetUserStocks()
        {
            ListStocksRequest listStocksRequest = _mapper.Map<ListStocksRequest>(HttpContext.User);

            ICollection<StockResponse> stocksResponse = await _stockService.ListUserStocks(listStocksRequest);

            return Ok(stocksResponse);
        }

        /// <summary>
        /// This function adds a stock to a user's collection.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol is a unique series of letters assigned to a
        /// publicly traded company on a stock exchange. It is used to identify the company's stock and
        /// is often abbreviated to a few letters, such as AAPL for Apple Inc. or GOOGL for Alphabet
        /// Inc.</param>
        /// <returns>
        /// The method is returning an IActionResult with an HTTP status code of 200 (OK) and a message
        /// "Stock added successfully".
        /// </returns>
        [HttpPost("add-user-stock")]
        public async Task<IActionResult> AddUserStock(string stockSymbol)
        {
            UserStockDTO stockDTO = _mapper.Map<UserStockDTO>(HttpContext.User);
            _mapper.Map(stockSymbol, stockDTO);

            await _stockService.AddStockToUser(stockDTO);

            return Ok("Stock added successfully");
        }

        
        /// <summary>
        /// This function removes a stock from a user's collection.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol is a unique series of letters assigned to a
        /// publicly traded company's stock. It is used to identify the stock on a stock exchange. In
        /// this case, the stock symbol is used as a parameter to remove a specific stock from a user's
        /// portfolio.</param>
        /// <returns>
        /// The method is returning an IActionResult with an HTTP status code of 200 (OK) and a message
        /// "Stock removed successfully".
        /// </returns>
        [HttpDelete("remove-user-stock")]
        public async Task<IActionResult> RemoveUserStock(string stockSymbol)
        {
            UserStockDTO stockDTO = _mapper.Map<UserStockDTO>(HttpContext.User);
            _mapper.Map(stockSymbol, stockDTO);

            await _stockService.RemoveStockFromUser(stockDTO);

            return Ok("Stock removed successfully");
        }
    }
}

