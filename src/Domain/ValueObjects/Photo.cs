// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Foto gemäss vCard-Property PHOTO.
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// vCard-Version des PHOTO-Formats (2.1, 3.0, 4.0, …).
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Bildtyp (z. B. JPEG, image/jpeg).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Encoding (z. B. BASE64, b).
        /// </summary>
        public string Encoding { get; set; } = string.Empty;

        /// <summary>
        /// URL oder Klartext-Wert, wenn keine Binärdaten eingebettet sind.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Eingebettete Bilddaten (Base64-dekodiert).
        /// </summary>
        public byte[]? Data { get; set; }

        /// <summary>
        /// Zeilenlänge für mehrzeilige Base64-Ausgabe (Serialisierungsmetadaten).
        /// </summary>
        internal int DataLineLength { get; set; }
    }
}
