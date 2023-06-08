using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Stocks.Core.Entities;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.ServiceContracts;

namespace Stocks.WebAPI.Controllers
{
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public StockController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        [HttpGet("user-stocks")]
        public async Task<ActionResult<ICollection<StockResponse>>> GetUserStocks()
        {
            ListStocksRequest listStocksRequest = _mapper.Map<ListStocksRequest>(HttpContext.User);

            ICollection<StockResponse> stocksResponse = await _stockService.ListUserStocks(listStocksRequest);

            return Ok(stocksResponse);
        }

        [HttpPost("add-user-stock")]
        public async Task<IActionResult> AddUserStock(string stockSymbol)
        {
            UserStockDTO stockDTO = _mapper.Map<UserStockDTO>(HttpContext.User);
            _mapper.Map(stockSymbol, stockDTO);

            await _stockService.AddStockToUser(stockDTO);

            return Ok("Stock added successfully");
        }

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

