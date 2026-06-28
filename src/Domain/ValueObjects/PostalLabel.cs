// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Eigenständige LABEL-Property (vCard 2.1/3.0), Text für ein physisches Adressetikett.
    /// </summary>
    public class PostalLabel
    {
        /// <summary>
        /// Typ der Adresse (z. B. HOME, WORK).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Etikettentext (mehrzeilig möglich).
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }
}
