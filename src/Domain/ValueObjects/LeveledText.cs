// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Text mit LEVEL-Parameter (EXPERTISE, HOBBY, INTEREST gemäss RFC 6715).
    /// </summary>
    public class LeveledText
    {
        /// <summary>
        /// Niveau (z. B. expert, high).
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Beschreibungstext.
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }
}
