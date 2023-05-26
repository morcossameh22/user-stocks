using System;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stocks.Core.DTO;
using Stocks.Core.Entities;
using Stocks.Core.Identity;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.ServiceContracts;
using Stocks.Infrastructure.DbContext;
using System.Text.Json;

namespace Stocks.WebAPI.Controllers
{
	public class StockController : ControllerBase
	{

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public StockController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
            _userManager = userManager;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet("user-stocks")]
        public async Task<ActionResult<ICollection<StockEntity>>> GetUserStocks()
        {
            ClaimsPrincipal user = HttpContext.User;

            string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                return NotFound();
            }

            ApplicationUser? applicationUser = await _userManager.Users
                .Include(u => u.Stocks)
                .FirstOrDefaultAsync(u => u.Id == new Guid(userId));

            if (applicationUser == null)
            {
                return NotFound();
            }

            ICollection<StockResponse> stocksResponse = new List<StockResponse>();

            foreach (StockEntity stock in applicationUser.Stocks)
            {
                Dictionary<string, object>? responseDictionary = await GetStockPriceQuote(stock.StockSymbol);

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

            return Ok(stocksResponse);
        }

        [HttpPost("add-user-stock")]
        public async Task<IActionResult> AddUserStock(UserStockDTO stockDTO)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }

            ClaimsPrincipal user = HttpContext.User;

            string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound();
            }

            ApplicationUser? applicationUser = await _userManager.Users
                .Include(u => u.Stocks)
                .FirstOrDefaultAsync(u => u.Id == new Guid(userId));

            StockEntity stock = new StockEntity()
            {
                StockSymbol = stockDTO.StockSymbol,
                User = applicationUser
            };

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.Stocks.Add(stock);

            IdentityResult result = await _userManager.UpdateAsync(applicationUser);

            if (result.Succeeded)
            {
                return Ok("Stock Added Successfully.");
            }
            else
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }

        [HttpDelete("remove-user-stock")]
        public async Task<IActionResult> RemoveUserStock(UserStockDTO stockDTO)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }

            ClaimsPrincipal user = HttpContext.User;

            string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound();
            }

            ApplicationUser? applicationUser = await _userManager.Users
                .Include(u => u.Stocks)
                .FirstOrDefaultAsync(u => u.Id == new Guid(userId));

            StockEntity? stock = applicationUser?.Stocks?.FirstOrDefault(s => s.StockSymbol == stockDTO.StockSymbol);

            if(stock == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stock);

            int result = await _context.SaveChangesAsync();

            if (result == 1)
            {
                return NoContent();
            }
            else
            {
                return Problem("Remove Stock failed");
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);

                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from finnhub server");

                if (responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

                return responseDictionary;
            }
        }
    }
}

