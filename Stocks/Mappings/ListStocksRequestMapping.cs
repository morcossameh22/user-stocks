using Stocks.Core.Stock.DTO;
using System.Security.Claims;
using AutoMapper;

namespace Stocks.WebAPI.Mappings
{
    public class ListStocksRequestMapping : Profile
    {
        public ListStocksRequestMapping()
        {
            CreateMap<ClaimsPrincipal, ListStocksRequest>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(
                src => src.FindFirstValue(ClaimTypes.NameIdentifier)
              ));
        }
    }
}

