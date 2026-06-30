// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Abstractions
{
    /// <summary>
    /// Öffentliche Fassade der SOWI.vCard-Bibliothek für Parse- und Serialize-Operationen.
    /// </summary>
    public interface IVCardService
    {
        /// <summary>
        /// Parst vCard-Text in ein Domain-Objekt.
        /// </summary>
        /// <param name="vCardText">vCard-Text (ein Block oder Dokument mit erstem Block).</param>
        /// <returns>Geparstes vCard-Objekt.</returns>
        VCard Parse(string vCardText);

        /// <summary>
        /// Parst ein vCard-Dokument mit allen enthaltenen vCard-Blöcken.
        /// </summary>
        /// <param name="vCardText">vCard-Dokument.</param>
        /// <returns>Alle geparsten vCard-Objekte.</returns>
        IReadOnlyList<VCard> ParseDocument(string vCardText);

        /// <summary>
        /// Parst einen vCard-Block asynchron aus einem Stream.
        /// </summary>
        /// <param name="stream">Eingabe-Stream.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        /// <returns>Geparstes vCard-Objekt.</returns>
        Task<VCard> ParseAsync(Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Parst ein vCard-Dokument asynchron aus einem Stream.
        /// </summary>
        /// <param name="stream">Eingabe-Stream.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        /// <returns>Alle geparsten vCard-Objekte.</returns>
        Task<IReadOnlyList<VCard>> ParseDocumentAsync(Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Liest und parst eine vCard-Datei asynchron.
        /// </summary>
        /// <param name="filePath">Pfad zur .vcf-Datei.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        /// <returns>Geparstes vCard-Objekt.</returns>
        Task<VCard> ParseFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Liest und parst ein vCard-Dokument asynchron aus einer Datei.
        /// </summary>
        /// <param name="filePath">Pfad zur .vcf-Datei.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        /// <returns>Alle geparsten vCard-Objekte.</returns>
        Task<IReadOnlyList<VCard>> ParseDocumentFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Serialisiert ein vCard-Objekt zu vCard-Text.
        /// </summary>
        /// <param name="vCard">vCard-Objekt.</param>
        /// <returns>vCard-Text.</returns>
        string Serialize(VCard vCard);

        /// <summary>
        /// Serialisiert mehrere vCards zu einem Dokument.
        /// </summary>
        /// <param name="vCards">vCard-Objekte.</param>
        /// <returns>vCard-Dokument.</returns>
        string SerializeDocument(IReadOnlyList<VCard> vCards);

        /// <summary>
        /// Serialisiert eine vCard asynchron in eine Datei.
        /// </summary>
        /// <param name="vCard">vCard-Objekt.</param>
        /// <param name="filePath">Zielpfad.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        Task SerializeToFileAsync(VCard vCard, string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Serialisiert ein vCard-Dokument asynchron in eine Datei.
        /// </summary>
        /// <param name="vCards">vCard-Objekte.</param>
        /// <param name="filePath">Zielpfad.</param>
        /// <param name="cancellationToken">Abbruch-Token.</param>
        Task SerializeDocumentToFileAsync(IReadOnlyList<VCard> vCards, string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lädt Photo-Bytes über den optionalen Photo-Data-Provider.
        /// </summary>
        /// <param name="photo">Photo-Objekt oder null für das Standard-Portrait.</param>
        /// <returns>Bild-Bytes oder null.</returns>
        byte[]? GetPhotoBytes(Photo? photo);
    }
}
