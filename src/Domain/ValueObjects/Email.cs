// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// E-Mail-Adresse gemäss vCard-Property EMAIL.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Typ (z. B. INTERNET, WORK).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// E-Mail-Adresse.
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}
