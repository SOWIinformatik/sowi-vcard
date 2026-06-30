// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;

namespace SOWI.vCard.Abstractions
{
    /// <summary>
    /// Port zum Parsen von vCard-Text in Domain-Objekte.
    /// </summary>
    public interface IVCardParser
    {
        /// <summary>
        /// Parst einen einzelnen vCard-Block.
        /// </summary>
        /// <param name="vCardText">vCard-Text mit genau einem BEGIN:VCARD … END:VCARD-Block.</param>
        /// <returns>Geparstes vCard-Objekt.</returns>
        VCard Parse(string vCardText);

        /// <summary>
        /// Parst ein vCard-Dokument mit einem oder mehreren vCard-Blöcken.
        /// </summary>
        /// <param name="vCardText">vCard-Text mit einem oder mehreren BEGIN:VCARD … END:VCARD-Blöcken.</param>
        /// <returns>Liste geparster vCard-Objekte.</returns>
        IReadOnlyList<VCard> ParseDocument(string vCardText);

        /// <summary>
        /// Parst einen vCard-Block asynchron aus einem Stream.
        /// </summary>
        /// <param name="stream">Eingabe-Stream mit vCard-Text.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        /// <returns>Geparstes vCard-Objekt.</returns>
        Task<VCard> ParseAsync(Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Parst ein vCard-Dokument asynchron aus einem Stream.
        /// </summary>
        /// <param name="stream">Eingabe-Stream mit vCard-Text.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        /// <returns>Alle geparsten vCard-Objekte.</returns>
        Task<IReadOnlyList<VCard>> ParseDocumentAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}
