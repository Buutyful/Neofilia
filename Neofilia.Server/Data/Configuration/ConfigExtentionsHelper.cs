using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neofilia.Domain;

namespace Neofilia.Server.Data.Configuration;

public static class ConfigExtentionsHelper
{
    public static PropertyBuilder<NotEmptyString> ConfigureNotEmptyString(
               this PropertyBuilder<NotEmptyString> propertyBuilder)
    {
        return propertyBuilder.HasConversion(
            value => value.Value,
            stringValue => new NotEmptyString(stringValue));
    }
}
