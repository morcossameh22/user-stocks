using System.Security.Claims;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stocks.Core.DTO;
using Stocks.Core.Entities;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.ServiceContracts;
using Stocks.Core.Stock.Services;
using Stocks.Core.User.ServiceContracts;
using Stocks.WebAPI.Controllers;

namespace Stocks.Tests.WebAPI.Controllers;

public class StockControllerTests
{
    private readonly IStockService _stockService;
    private readonly Mock<IStockService> _stockServiceMock;

    private readonly IMapper _mapper;
    private readonly Mock<IMapper> _mapperMock;

    private readonly Fixture _fixture;


    public StockControllerTests()
    {
        _fixture = new Fixture();

        _stockServiceMock = new Mock<IStockService>();
        _stockService = _stockServiceMock.Object;

        _mapperMock = new Mock<IMapper>();
        _mapper = _mapperMock.Object;
    }

    [Fact]
    public async Task GetUserStocks_ShouldReturnStockResponseList()
    {
        StockController stockController = new StockController(_stockService, _mapper);

        ListStocksRequest request = _fixture.Create<ListStocksRequest>();

        ICollection<StockResponse> response = _fixture.Create<ICollection<StockResponse>>();

        DefaultHttpContext context = new DefaultHttpContext();
        context.User = new ClaimsPrincipal();
        stockController.ControllerContext.HttpContext = context;

        _mapperMock
         .Setup(temp => temp.Map<ListStocksRequest>(It.IsAny<ClaimsPrincipal>()))
         .Returns(request);

        _stockServiceMock
         .Setup(temp => temp.ListUserStocks(It.IsAny<ListStocksRequest>()))
         .ReturnsAsync(response);

        ActionResult<ICollection<StockResponse>> result = await stockController.GetUserStocks();

        Assert.IsType<ActionResult<ICollection<StockResponse>>>(result);
    }

    [Fact]
    public async Task AddUserStock_ShouldReturnIActionResult()
    {
        StockController stockController = new StockController(_stockService, _mapper);

        UserStockDTO userStockDTO = _fixture.Create<UserStockDTO>();

        DefaultHttpContext context = new DefaultHttpContext();
        context.User = new ClaimsPrincipal();
        stockController.ControllerContext.HttpContext = context;

        _mapperMock
         .Setup(temp => temp.Map<UserStockDTO>(It.IsAny<ClaimsPrincipal>()))
         .Returns(userStockDTO);

        _mapperMock
         .Setup(temp => temp.Map<UserStockDTO>(It.IsAny<string>()))
         .Returns(userStockDTO);

        _stockServiceMock
         .Setup(temp => temp.AddStockToUser(It.IsAny<UserStockDTO>()));

        IActionResult result = await stockController.AddUserStock("AAPL");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RemoveUserStock_ShouldReturnIActionResult()
    {
        StockController stockController = new StockController(_stockService, _mapper);

        UserStockDTO userStockDTO = _fixture.Create<UserStockDTO>();

        DefaultHttpContext context = new DefaultHttpContext();
        context.User = new ClaimsPrincipal();
        stockController.ControllerContext.HttpContext = context;

        _mapperMock
         .Setup(temp => temp.Map<UserStockDTO>(It.IsAny<ClaimsPrincipal>()))
         .Returns(userStockDTO);

        _mapperMock
         .Setup(temp => temp.Map<UserStockDTO>(It.IsAny<string>()))
         .Returns(userStockDTO);

        _stockServiceMock
         .Setup(temp => temp.RemoveStockFromUser(It.IsAny<UserStockDTO>()));

        IActionResult result = await stockController.RemoveUserStock("AAPL");

        Assert.IsType<OkObjectResult>(result);
    }
}

