// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Telefonnummer gemäss vCard-Property TEL.
    /// </summary>
    public class PhoneNumber
    {
        /// <summary>
        /// Typ (z. B. WORK, HOME, CELL).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Telefonnummer.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gibt an, ob der Wert als URI (tel:) in vCard 4.0 serialisiert wird.
        /// </summary>
        public bool UseUriValue { get; set; }
    }
}
