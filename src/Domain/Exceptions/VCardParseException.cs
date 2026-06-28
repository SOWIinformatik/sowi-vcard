// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.Exceptions
{
    /// <summary>
    /// Signalisiert einen Fehler beim Parsen oder Validieren von vCard-Daten gemäss RFC 6350.
    /// </summary>
    public class VCardParseException : Exception
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="VCardParseException"/>-Klasse.
        /// </summary>
        /// <param name="message">Fehlermeldung.</param>
        /// <param name="propertyName">Optional: betroffene vCard-Property (z. B. FN, ADR).</param>
        /// <param name="lineNumber">Optional: Zeilennummer im vCard-Text (1-basiert).</param>
        public VCardParseException(string message, string? propertyName = null, int? lineNumber = null)
            : base(message)
        {
            this.PropertyName = propertyName;
            this.LineNumber = lineNumber;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="VCardParseException"/>-Klasse mit Inner Exception.
        /// </summary>
        /// <param name="message">Fehlermeldung.</param>
        /// <param name="inner">Ursprüngliche Exception.</param>
        public VCardParseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz mit Property-Kontext und Inner Exception.
        /// </summary>
        public VCardParseException(string message, string? propertyName, Exception inner)
            : base(message, inner)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Name der betroffenen vCard-Property, sofern bekannt.
        /// </summary>
        public string? PropertyName { get; }

        /// <summary>
        /// Zeilennummer im vCard-Text (1-basiert), sofern bekannt.
        /// </summary>
        public int? LineNumber { get; }
    }
}
