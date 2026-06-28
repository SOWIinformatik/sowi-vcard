// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.Exceptions;
using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Parsing.VersionStrategies
{
    /// <summary>
    /// Formate gemäss vCard 4.0.
    /// </summary>
    internal sealed class VCard40VersionStrategy : VCardVersionStrategyBase
    {
        /// <inheritdoc />
        public override string Version => "4.0";

        /// <inheritdoc />
        public override string FormatDate(DateTime value) => value.ToString("yyyyMMdd");

        /// <inheritdoc />
        public override string FormatDateTime(DateTime value) => value.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");

        /// <inheritdoc />
        public override GeoLocation ParseGeoLocation(string value)
        {
            if (value.StartsWith("geo:", StringComparison.OrdinalIgnoreCase))
            {
                var coordinates = value["geo:".Length..].Replace("\\,", ",", StringComparison.Ordinal);
                var separator = coordinates.Contains(',') ? "," : ";";
                return ParseGeoLatLon(coordinates, separator);
            }

            return ParseGeoLatLon(value, value.Contains(';') ? ";" : ",");
        }

        /// <inheritdoc />
        public override string WriteGeoLocation(GeoLocation geo)
            => $"GEO:geo:{geo.Latitude:0.0000}\\,{geo.Longitude:0.0000}";

        /// <inheritdoc />
        public override PhoneNumber ParsePhoneNumber(string propertyPart, string value)
            => ParsePhoneWithTypes(propertyPart, value, useUriValue: true);

        /// <inheritdoc />
        public override string WritePhoneNumber(PhoneNumber phone)
        {
            var number = phone.Number;
            if (phone.UseUriValue && !number.StartsWith("tel:", StringComparison.OrdinalIgnoreCase))
            {
                number = $"tel:{number}";
            }

            if (string.IsNullOrEmpty(phone.Type))
            {
                return phone.UseUriValue
                    ? $"TEL;VALUE=uri:{number}"
                    : $"TEL:{number}";
            }

            return phone.UseUriValue
                ? $"TEL;TYPE={phone.Type};VALUE=uri:{number}"
                : $"TEL;TYPE={phone.Type}:{number}";
        }
    }
}
