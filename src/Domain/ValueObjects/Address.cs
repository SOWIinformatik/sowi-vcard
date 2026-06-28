// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Physische Anschrift gemäss vCard-Property ADR.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Typ der Adresse (z. B. home, work).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Postfach.
        /// </summary>
        public string PostOfficeBox { get; set; } = string.Empty;

        /// <summary>
        /// Erweiterte Adresszeile.
        /// </summary>
        public string ExtendedAddress { get; set; } = string.Empty;

        /// <summary>
        /// Strasse.
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Ort.
        /// </summary>
        public string Locality { get; set; } = string.Empty;

        /// <summary>
        /// Region / Kanton.
        /// </summary>
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Postleitzahl.
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Land.
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Adressetikett-Text (LABEL-Parameter in vCard 4.0 oder eigenständige LABEL-Property in 2.1/3.0).
        /// </summary>
        public string Label { get; set; } = string.Empty;
    }
}
