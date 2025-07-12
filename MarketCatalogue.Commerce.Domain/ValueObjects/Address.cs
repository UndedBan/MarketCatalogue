using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.ValueObjects;

[Owned]
public class Address
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public Address() { }

    public Address(
        string? street = null,
        string? city = null,
        string? state = null,
        string? postalCode = null,
        string? country = null,
        double? latitude = null,
        double? longitude = null)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
    }
}
