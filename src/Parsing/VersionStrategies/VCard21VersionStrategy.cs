// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Parsing.VersionStrategies
{
    /// <summary>
    /// Formate gemäss vCard 2.1.
    /// </summary>
    internal sealed class VCard21VersionStrategy : VCardVersionStrategyBase
    {
        /// <inheritdoc />
        public override string Version => "2.1";

        /// <inheritdoc />
        public override string FormatDate(DateTime value) => value.ToString("yyyy-MM-dd");

        /// <inheritdoc />
        public override string FormatDateTime(DateTime value) => value.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");

        /// <inheritdoc />
        public override GeoLocation ParseGeoLocation(string value) => ParseGeoLatLon(value, ";");

        /// <inheritdoc />
        public override string WriteGeoLocation(GeoLocation geo)
            => $"GEO:{geo.Latitude:0.0000};{geo.Longitude:0.0000}";

        /// <inheritdoc />
        public override PhoneNumber ParsePhoneNumber(string propertyPart, string value)
            => ParsePhoneWithTypes(propertyPart, value, useUriValue: false);

        /// <inheritdoc />
        public override string WritePhoneNumber(PhoneNumber phone)
        {
            if (string.IsNullOrEmpty(phone.Type))
            {
                return $"TEL:{phone.Number}";
            }

            // v2.1: TEL;WORK;VOICE:nummer
            var types = phone.Type.Split(',');
            return types.Length > 1
                ? $"TEL;{string.Join(";", types)}:{phone.Number}"
                : $"TEL;{phone.Type}:{phone.Number}";
        }
    }
}
