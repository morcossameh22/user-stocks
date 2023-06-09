using Stocks.Core.Stock.DTO;
using System.Security.Claims;
using AutoMapper;

namespace Stocks.WebAPI.Mappings
{
    /* The class `ListStocksRequestMapping` maps a `ClaimsPrincipal` object to a `ListStocksRequest`
    object.*/
    public class ListStocksRequestMapping : Profile
    {
        /* This is a constructor method for the `ListStocksRequestMapping` class. It creates a mapping
        between a `ClaimsPrincipal` object and a `ListStocksRequest` object using AutoMapper. It
        sets the `UserId` property of the `ListStocksRequest` object to the value of the
        `NameIdentifier` claim of the `ClaimsPrincipal` object. This mapping is used to convert a
        `ClaimsPrincipal` object to a `ListStocksRequest` object in the context of the Stocks
        WebAPI. */
        public ListStocksRequestMapping()
        {
            CreateMap<ClaimsPrincipal, ListStocksRequest>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(
                src => src.FindFirstValue(ClaimTypes.NameIdentifier)
              ));
        }
    }
}

