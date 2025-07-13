using AutoMapper;
using MarketCatalogue.Commerce.Domain.Dtos.Orders;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Presentation.Areas.Purchasers.Models;

namespace MarketCatalogue.Presentation.Automapper;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderItem, UserOrderItemDto>()
           .ForMember(dest => dest.Total, opt => opt.Ignore())
           .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

        CreateMap<Order, UserOrderDto>()
    .ForMember(dest => dest.PurchaserId, opt => opt.MapFrom(src => src.PurchaserId))
    .ForMember(dest => dest.ArrivalDate, opt => opt.MapFrom(src => src.ArrivalDate))
    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<OrderItem, UserOrderItemDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

        CreateMap<UserOrderDto, UserOrderViewModel>()
    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()));

        CreateMap<UserOrderItemDto, UserOrderItemViewModel>();
    }
}
