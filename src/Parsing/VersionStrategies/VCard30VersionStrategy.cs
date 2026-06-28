// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Parsing.VersionStrategies
{
    /// <summary>
    /// Formate gemäss vCard 3.0.
    /// </summary>
    internal sealed class VCard30VersionStrategy : VCardVersionStrategyBase
    {
        /// <inheritdoc />
        public override string Version => "3.0";

        /// <inheritdoc />
        public override string FormatDate(DateTime value) => value.ToString("yyyy-MM-dd");

        /// <inheritdoc />
        public override string FormatDateTime(DateTime value) => value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

        /// <inheritdoc />
        public override GeoLocation ParseGeoLocation(string value)
        {
            var separator = value.Contains(';') ? ";" : ",";
            return ParseGeoLatLon(value, separator);
        }

        /// <inheritdoc />
        public override string WriteGeoLocation(GeoLocation geo)
            => $"GEO:{geo.Latitude:0.0000}{geo.Separator}{geo.Longitude:0.0000}";

        /// <inheritdoc />
        public override PhoneNumber ParsePhoneNumber(string propertyPart, string value)
            => ParsePhoneWithTypes(propertyPart, value, useUriValue: false);

        /// <inheritdoc />
        public override string WritePhoneNumber(PhoneNumber phone)
        {
            return string.IsNullOrEmpty(phone.Type)
                ? $"TEL:{phone.Number}"
                : $"TEL;TYPE={phone.Type}:{phone.Number}";
        }
    }
}
