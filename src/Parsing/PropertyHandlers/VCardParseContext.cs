// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Parsing.VersionStrategies;

namespace SOWI.vCard.Parsing.PropertyHandlers
{
    /// <summary>
    /// Kontext für das Anwenden eines Property-Handlers beim Parsen.
    /// </summary>
    internal sealed class VCardParseContext
    {
        /// <summary>
        /// Das zu befüllende Aggregate.
        /// </summary>
        public VCard VCard { get; set; } = null!;

        /// <summary>
        /// Versionsspezifische Strategie (wird bei VERSION-Zeile aktualisiert).
        /// </summary>
        public IVCardVersionStrategy Strategy { get; set; } = null!;

        /// <summary>
        /// Normalisierter vCard-Block (für eingebettete Medien).
        /// </summary>
        public string NormalizedBlock { get; set; } = string.Empty;

        /// <summary>
        /// Alle Zeilen des Blocks.
        /// </summary>
        public string[] Lines { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Aktueller Zeilenindex (kann vom Handler fortgeschrieben werden).
        /// </summary>
        public int LineIndex { get; set; }

        /// <summary>
        /// Aktuelle Zeile.
        /// </summary>
        public string Line { get; set; } = string.Empty;

        /// <summary>
        /// Property-Teil vor dem Doppelpunkt.
        /// </summary>
        public string PropertyPart { get; set; } = string.Empty;

        /// <summary>
        /// Property-Wert nach dem Doppelpunkt.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Property-Name ohne Parameter.
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;
    }
}
