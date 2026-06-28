// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Services
{
    /// <summary>
    /// Öffentliche Fassade für Parse- und Serialize-Operationen der SOWI.vCard-Bibliothek.
    /// </summary>
    public class VCardService : IVCardService
    {
        private readonly IVCardParser _parser;
        private readonly IVCardSerializer _serializer;
        private readonly IPhotoDataProvider? _photoDataProvider;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="VCardService"/>-Klasse.
        /// </summary>
        /// <param name="parser">vCard-Parser.</param>
        /// <param name="serializer">vCard-Serializer.</param>
        /// <param name="photoDataProvider">Optionaler Photo-Daten-Provider.</param>
        public VCardService(
            IVCardParser parser,
            IVCardSerializer serializer,
            IPhotoDataProvider? photoDataProvider = null)
        {
            this._parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this._serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this._photoDataProvider = photoDataProvider;
        }

        /// <summary>
        /// Erstellt eine Standard-Instanz mit Parser, Serializer und optionalem Photo-Provider.
        /// </summary>
        /// <param name="includePhotoProvider">True, wenn FilePhotoDataProvider registriert werden soll.</param>
        /// <returns>Konfigurierte Fassade.</returns>
        public static VCardService CreateDefault(bool includePhotoProvider = false)
        {
            var lineReader = new Parsing.VCardLineReader();
            var parser = new Parsing.VCardParser(lineReader);
            var serializer = new Serialization.VCardSerializer();
            IPhotoDataProvider? photoProvider = includePhotoProvider ? new FilePhotoDataProvider() : null;
            return new VCardService(parser, serializer, photoProvider);
        }

        /// <inheritdoc />
        public VCard Parse(string vCardText) => this._parser.Parse(vCardText);

        /// <inheritdoc />
        public IReadOnlyList<VCard> ParseDocument(string vCardText) => this._parser.ParseDocument(vCardText);

        /// <inheritdoc />
        public Task<VCard> ParseAsync(Stream stream, CancellationToken cancellationToken = default)
            => this._parser.ParseAsync(stream, cancellationToken);

        /// <inheritdoc />
        public Task<IReadOnlyList<VCard>> ParseDocumentAsync(Stream stream, CancellationToken cancellationToken = default)
            => this._parser.ParseDocumentAsync(stream, cancellationToken);

        /// <inheritdoc />
        public async Task<VCard> ParseFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var text = await Parsing.VCardTextReader.ReadFileAsync(filePath, cancellationToken).ConfigureAwait(false);
            return this.Parse(text);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<VCard>> ParseDocumentFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var text = await Parsing.VCardTextReader.ReadFileAsync(filePath, cancellationToken).ConfigureAwait(false);
            return this.ParseDocument(text);
        }

        /// <inheritdoc />
        public string Serialize(VCard vCard) => this._serializer.Serialize(vCard);

        /// <inheritdoc />
        public string SerializeDocument(IReadOnlyList<VCard> vCards) => this._serializer.SerializeDocument(vCards);

        /// <inheritdoc />
        public async Task SerializeToFileAsync(VCard vCard, string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Der Dateipfad darf nicht leer sein.", nameof(filePath));
            }

            var text = this.Serialize(vCard);
            await File.WriteAllTextAsync(filePath, text, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task SerializeDocumentToFileAsync(IReadOnlyList<VCard> vCards, string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Der Dateipfad darf nicht leer sein.", nameof(filePath));
            }

            var text = this.SerializeDocument(vCards);
            await File.WriteAllTextAsync(filePath, text, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public byte[]? GetPhotoBytes(Photo? photo)
        {
            if (photo == null)
            {
                return this._photoDataProvider?.LoadDefaultPortrait();
            }

            if (photo.Data != null && photo.Data.Length > 0)
            {
                return photo.Data;
            }

            return this._photoDataProvider?.LoadPhotoData(photo);
        }
    }
}
