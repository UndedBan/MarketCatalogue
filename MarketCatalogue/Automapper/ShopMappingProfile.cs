using AutoMapper;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.ValueObjects;
using MarketCatalogue.Presentation.Areas.Shops.Models.BindingModels;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

namespace MarketCatalogue.Presentation.Automapper;

public class ShopMappingProfile : Profile
{
    public ShopMappingProfile()
    {
        // === Domain Entity ↔ DTOs ===
        CreateMap<Shop, RepresentativeShopDto>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0))
            .ForMember(dest => dest.MarketRepresentative, opt => opt.MapFrom(src => src.MarketRepresentative))
            .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => src.Schedule));

        CreateMap<Shop, ShopSummaryDto>();
        CreateMap<Schedule, ScheduleDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<RepresentativeShopDto, Shop>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.MarketRepresentative, opt => opt.MapFrom(src => src.MarketRepresentative))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => src.Schedule));

        CreateMap<ShopCreateDto, Shop>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.MarketRepresentative, opt => opt.Ignore());

        CreateMap<Shop, EditShopDto>();
        CreateMap<EditShopDto, Shop>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.MarketRepresentative, opt => opt.Ignore());

        // === DTOs ↔ ViewModels ===
        CreateMap<RepresentativeShopDto, RepresentativeShopsViewModel>();
        CreateMap<EditShopDto, EditShopViewModel>();
        CreateMap<ProductDto, ProductViewModel>();
        CreateMap<ShopWithProductsDto, ShopWithProductsViewModel>();

        // === DTOs ↔ Domain Value Objects ===
        CreateMap<AddressDto, Address>().ReverseMap();
        CreateMap<ScheduleDto, Schedule>().ReverseMap();

        // === DTOs ↔ ViewModels ===
        CreateMap<AddressDto, AddressViewModel>().ReverseMap();
        CreateMap<ScheduleDto, ScheduleViewModel>().ReverseMap();

        // === ViewModels ↔ BindingModels ===
        CreateMap<AddressBindingModel, AddressViewModel>();
        CreateMap<ScheduleBindingModel, ScheduleViewModel>();

        // === BindingModels → DTOs ===
        CreateMap<ShopCreateBindingModel, ShopCreateDto>();
        CreateMap<AddressBindingModel, AddressDto>();
        CreateMap<ScheduleBindingModel, ScheduleDto>();
        CreateMap<EditShopBindingModel, EditShopDto>();

        // === BindingModels → ViewModels ===
        CreateMap<ShopCreateBindingModel, ShopCreateViewModel>();
        CreateMap<EditShopBindingModel, EditShopViewModel>();
    }
}
