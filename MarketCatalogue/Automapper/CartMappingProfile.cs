using AutoMapper;
using MarketCatalogue.Commerce.Domain.Dtos.Cart;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Presentation.Areas.Purchasers.Models;

namespace MarketCatalogue.Presentation.Automapper;

public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<AddToCartDto, CartItem>()
            .ForMember(des => des.CartId, opt => opt.Ignore());
        CreateMap<ViewCartItemDto, ViewCartItemViewModel>();
        CreateMap<CartItem, ViewCartItemDto>()
        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
        .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price))
        .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.Product.Quantity));
        CreateMap<UpdateQuantityBindingModel, UpdateQuantityDto>();
    }
}
