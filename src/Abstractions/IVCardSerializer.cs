// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;

namespace SOWI.vCard.Abstractions
{
    /// <summary>
    /// Port zum Serialisieren von Domain-Objekten in vCard-Text.
    /// </summary>
    public interface IVCardSerializer
    {
        /// <summary>
        /// Serialisiert eine vCard zu RFC-Text.
        /// </summary>
        /// <param name="vCard">Das vCard-Objekt.</param>
        /// <returns>vCard-Text mit CRLF-Zeilenenden.</returns>
        string Serialize(VCard vCard);

        /// <summary>
        /// Serialisiert mehrere vCards zu einem vCard-Dokument.
        /// </summary>
        /// <param name="vCards">Liste von vCard-Objekten.</param>
        /// <returns>vCard-Dokument als Text.</returns>
        string SerializeDocument(IReadOnlyList<VCard> vCards);
    }
}
