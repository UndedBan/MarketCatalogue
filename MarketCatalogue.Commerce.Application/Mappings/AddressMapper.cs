using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Mappings;

public static class AddressMapper
{
    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto
        {
            Street = address.Street,
            City = address.City,
            PostalCode = address.PostalCode,
            State = address.State,
            Country = address.Country,
            Longitude = address.Longitude,
            Latitude = address.Latitude,
        };
    }

    public static Address ToAddressEntity(this AddressDto addressDto)
    {
        return new Address
        {
            Street = addressDto.Street,
            City = addressDto.City,
            PostalCode = addressDto.PostalCode,
            State = addressDto.State,
            Country = addressDto.Country,
            Longitude = addressDto.Longitude,
            Latitude = addressDto.Latitude,
        };
    }
}
