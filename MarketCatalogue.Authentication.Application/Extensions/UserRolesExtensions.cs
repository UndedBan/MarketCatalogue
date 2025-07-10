using MarketCatalogue.Authentication.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Authentication.Application.Extensions;

public static class UserRolesExtensions
{
    public static string ToRoleName(this UserRoles role) => role switch
    {
        UserRoles.MarketRepresentative => "Market Representative",
        UserRoles.Purchaser => "Purchaser",
        _ => throw new ArgumentOutOfRangeException()
    };
}
