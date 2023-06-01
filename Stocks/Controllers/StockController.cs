using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using Stocks.Core.Entities;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.ServiceContracts;

namespace Stocks.WebAPI.Controllers
{
  public class StockController : ControllerBase
  {
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
      _stockService = stockService;
    }

    [HttpGet("user-stocks")]
    public async Task<ActionResult<ICollection<StockEntity>>> GetUserStocks()
    {
      ListStocksRequest listStocksRequest = getListStocksRequest();

      ICollection<StockResponse> stocksResponse = await _stockService.listUserStocks(listStocksRequest);

      return Ok(stocksResponse);
    }

    [HttpPost("add-user-stock")]
    public async Task<IActionResult> AddUserStock(string stockSymbol)
    {
      UserStockDTO stockDTO = getStockDTO(stockSymbol);

      await _stockService.addStockToUser(stockDTO);

      return Ok("Stock added successfully");
    }

    [HttpDelete("remove-user-stock")]
    public async Task<IActionResult> RemoveUserStock(string stockSymbol)
    {
      UserStockDTO stockDTO = getStockDTO(stockSymbol);

      await _stockService.removeStockFromUser(stockDTO);

      return Ok("Stock removed successfully");
    }

    private ListStocksRequest getListStocksRequest()
    {
      ClaimsPrincipal user = HttpContext.User;

      string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Not found");
      return new ListStocksRequest()
      {
        UserId = userId
      };
    }

    private UserStockDTO getStockDTO(string stockSymbol)
    {
      ClaimsPrincipal user = HttpContext.User;

      string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Not found");
      return new UserStockDTO()
      {
        StockSymbol = stockSymbol,
        UserId = userId
      };
    }
  }
}

