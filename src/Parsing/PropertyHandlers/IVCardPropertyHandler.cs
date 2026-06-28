// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Parsing.PropertyHandlers
{
    /// <summary>
    /// Verarbeitet eine einzelne vCard-Property-Zeile.
    /// </summary>
    internal interface IVCardPropertyHandler
    {
        /// <summary>
        /// Prüft, ob dieser Handler die Property verarbeiten kann.
        /// </summary>
        bool CanHandle(string propertyName);

        /// <summary>
        /// Wendet den Handler auf den Parse-Kontext an.
        /// </summary>
        void Apply(VCardParseContext context);
    }
}
