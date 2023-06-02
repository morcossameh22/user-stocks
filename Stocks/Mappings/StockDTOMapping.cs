using System.Security.Claims;

using AutoMapper;

using Stocks.Core.Stock.DTO;

namespace Stocks.WebAPI.Mappings
{
    public class StockDTOMapping : Profile
    {
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

