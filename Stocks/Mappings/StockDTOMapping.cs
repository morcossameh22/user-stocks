using System.Security.Claims;

using AutoMapper;

using Stocks.Core.Stock.DTO;

namespace Stocks.WebAPI.Mappings
{
    /* The StockDTOMapping class maps ClaimsPrincipal and string objects to UserStockDTO objects. */
    public class StockDTOMapping : Profile
    {
        /* This is a constructor for the `StockDTOMapping` class that defines two mappings using
        AutoMapper. */
        public StockDTOMapping()
        {
            CreateMap<ClaimsPrincipal, UserStockDTO>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(
                src => src.FindFirstValue(ClaimTypes.NameIdentifier)
              ));

            CreateMap<string, UserStockDTO>()
              .ForMember(dest => dest.StockSymbol, opt => opt.MapFrom(
                src => src
              ));
        }
    }
}

