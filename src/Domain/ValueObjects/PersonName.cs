// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// Strukturierter Name gemäss vCard-Property N.<br />
    /// Syntax: Nachname;Vorname;weitere Vornamen;Präfix;Suffix
    /// </summary>
    public class PersonName
    {
        /// <summary>
        /// Präfix (z. B. Dr., Prof.).
        /// </summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// Vorname.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Weitere Vornamen.
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Nachname.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Suffix (z. B. Jr., Sr.).
        /// </summary>
        public string Suffix { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Join(" ", new[] { this.Prefix, this.FirstName, this.MiddleName, this.LastName, this.Suffix }
                .Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}
