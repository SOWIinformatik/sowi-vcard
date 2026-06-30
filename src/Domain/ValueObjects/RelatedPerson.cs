// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Verwandte Person gemäss vCard-Property RELATED.
    /// </summary>
    public class RelatedPerson
    {
        /// <summary>
        /// Beziehungstyp (z. B. friend, spouse).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Referenz (mailto-URL oder UUID).
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
