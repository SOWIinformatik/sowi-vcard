// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Öffentlicher Schlüssel gemäss vCard-Property KEY.
    /// </summary>
    public class PublicKey
    {
        /// <summary>
        /// vCard-Version des KEY-Formats (2.1, 3.0, 4.0).
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Schlüsseltyp (z. B. PGP) oder Media-Type (z. B. application/pgp-keys).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Encoding (z. B. BASE64, b).
        /// </summary>
        public string Encoding { get; set; } = string.Empty;

        /// <summary>
        /// URL, Klartext oder data:-URI, wenn keine Binärdaten eingebettet sind.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Eingebettete Schlüsseldaten (Base64-dekodiert).
        /// </summary>
        public byte[]? Data { get; set; }

        /// <summary>
        /// Zeilenlänge für mehrzeilige Base64-Ausgabe (Serialisierungsmetadaten).
        /// </summary>
        internal int DataLineLength { get; set; }
    }
}
