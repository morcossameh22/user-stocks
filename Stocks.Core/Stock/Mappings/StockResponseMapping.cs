using AutoMapper;

using Stocks.Core.Entities;
using Stocks.Core.Stock.DTO;

namespace Stocks.Core.Stock.Mappings
{
    /* The StockResponseMapping class maps data from a StockEntity object and a dictionary to a
    StockResponse object. */
    public class StockResponseMapping : Profile
    {
        public StockResponseMapping()
        {
            CreateMap<StockEntity, StockResponse>()
              .ForMember(dest => dest.StockSymbol, opt => opt.MapFrom(
                  src => src.StockSymbol
              ));

            CreateMap<Dictionary<string, object>?, StockResponse>()
              .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(
                  src => Convert.ToDouble(src["c"].ToString())
              ))
              .ForMember(dest => dest.HighestPrice, opt => opt.MapFrom(
                  src => Convert.ToDouble(src["h"].ToString())
              ))
              .ForMember(dest => dest.LowestPrice, opt => opt.MapFrom(
                  src => Convert.ToDouble(src["l"].ToString())
              ))
              .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(
                  src => Convert.ToDouble(src["o"].ToString())
              ));
        }
    }
}

