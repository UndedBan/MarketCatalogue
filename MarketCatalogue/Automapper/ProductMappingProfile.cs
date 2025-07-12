using AutoMapper;
using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Presentation.Areas.Products.Models;

namespace MarketCatalogue.Presentation.Automapper;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Shop, opt => opt.Ignore())
            .ForMember(dest => dest.Audit, opt => opt.Ignore())
            .ForMember(dest => dest.Hidden, opt => opt.Ignore())
            .ForMember(dest => dest.ExtraData, opt => opt.Ignore())
            .ForMember(dest => dest.Notes, opt => opt.Ignore());

        CreateMap<ProductCreateBindingModel, ProductCreateDto>();
        CreateMap<Product, ProductDetailsDto>()
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.ShopName));
        CreateMap<ProductDetailsDto, ProductDetailsViewModel>();
        CreateMap<EditProductBindingModel, EditProductDto>();
        CreateMap<EditProductDto, Product>()
            .ForMember(dest => dest.Shop, opt => opt.Ignore())
            .ForMember(dest => dest.Audit, opt => opt.Ignore())
            .ForMember(dest => dest.Hidden, opt => opt.Ignore())
            .ForMember(dest => dest.ExtraData, opt => opt.Ignore())
            .ForMember(dest => dest.Notes, opt => opt.Ignore());
    }
}
