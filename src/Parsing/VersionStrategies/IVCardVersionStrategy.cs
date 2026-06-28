// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Parsing.VersionStrategies
{
    /// <summary>
    /// Versionsspezifische Formate für Parse- und Serialize-Operationen.
    /// </summary>
    internal interface IVCardVersionStrategy
    {
        /// <summary>
        /// vCard-Versionskennung (2.1, 3.0, 4.0).
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Parst ein Datum oder einen Zeitstempel.
        /// </summary>
        DateTime? ParseDateTime(string value);

        /// <summary>
        /// Formatiert BDAY/ANNIVERSARY.
        /// </summary>
        string FormatDate(DateTime value);

        /// <summary>
        /// Formatiert REV/CREATED/DTSTAMP.
        /// </summary>
        string FormatDateTime(DateTime value);

        /// <summary>
        /// Parst GEO-Wert.
        /// </summary>
        GeoLocation ParseGeoLocation(string value);

        /// <summary>
        /// Serialisiert GEO.
        /// </summary>
        string WriteGeoLocation(GeoLocation geo);

        /// <summary>
        /// Parst TEL-Zeile.
        /// </summary>
        PhoneNumber ParsePhoneNumber(string propertyPart, string value);

        /// <summary>
        /// Serialisiert TEL.
        /// </summary>
        string WritePhoneNumber(PhoneNumber phone);
    }
}
