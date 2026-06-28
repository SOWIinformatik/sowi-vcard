// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.Exceptions;
using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Parsing.VersionStrategies
{
    /// <summary>
    /// Gemeinsame Hilfsmethoden für Version-Strategies.
    /// </summary>
    internal abstract class VCardVersionStrategyBase : IVCardVersionStrategy
    {
        /// <inheritdoc />
        public abstract string Version { get; }

        /// <inheritdoc />
        public virtual DateTime? ParseDateTime(string value) => VCardDateTimeParser.Parse(value);

        /// <inheritdoc />
        public abstract string FormatDate(DateTime value);

        /// <inheritdoc />
        public abstract string FormatDateTime(DateTime value);

        /// <inheritdoc />
        public abstract GeoLocation ParseGeoLocation(string value);

        /// <inheritdoc />
        public abstract string WriteGeoLocation(GeoLocation geo);

        /// <inheritdoc />
        public abstract PhoneNumber ParsePhoneNumber(string propertyPart, string value);

        /// <inheritdoc />
        public abstract string WritePhoneNumber(PhoneNumber phone);

        /// <summary>
        /// Parst Breiten- und Längengrad aus einem Wertestring.
        /// </summary>
        protected static GeoLocation ParseGeoLatLon(string value, string separator)
        {
            var components = value.Split(new[] { separator }, StringSplitOptions.None);
            if (components.Length == 2 &&
                double.TryParse(components[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var latitude) &&
                double.TryParse(components[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var longitude))
            {
                return new GeoLocation
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Separator = separator,
                    IsUriFormat = separator == ",",
                };
            }

            throw new VCardParseException("Ungültiges GEO-Format: Breite/Länge fehlen.", propertyName: "GEO");
        }

        /// <summary>
        /// Parst TEL mit TYPE-Parametern.
        /// </summary>
        protected static PhoneNumber ParsePhoneWithTypes(string propertyPart, string value, bool useUriValue)
        {
            var useUri = useUriValue ||
                VCardPropertyLineParser.ExtractParameter(propertyPart, "VALUE")
                    ?.Equals("uri", StringComparison.OrdinalIgnoreCase) == true;

            var number = value;
            if (!useUri && number.StartsWith("tel:", StringComparison.OrdinalIgnoreCase))
            {
                number = number["tel:".Length..];
            }

            return new PhoneNumber
            {
                Type = VCardPropertyLineParser.ExtractTypeParameter(propertyPart),
                Number = number,
                UseUriValue = useUri,
            };
        }
    }
}
