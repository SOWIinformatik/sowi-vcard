// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Geo-Koordinaten gemäss vCard-Property GEO.
    /// </summary>
    public class GeoLocation
    {
        /// <summary>
        /// Breitengrad.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Längengrad.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Trennzeichen zwischen Breite und Länge (; oder ,).
        /// </summary>
        public string Separator { get; set; } = ";";

        /// <summary>
        /// Gibt an, ob das geo:-URI-Format (vCard 4.0) verwendet wird.
        /// </summary>
        public bool IsUriFormat { get; set; }

        /// <summary>
        /// Prüft, ob gültige Koordinaten gesetzt sind.
        /// </summary>
        public bool IsValid()
        {
            const double epsilon = 0.000001;

            return this.Latitude is >= -90 and <= 90
                && this.Longitude is >= -180 and <= 180
                && (Math.Abs(this.Latitude) > epsilon || Math.Abs(this.Longitude) > epsilon);
        }
    }
}
