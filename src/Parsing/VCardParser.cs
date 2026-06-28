// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Parst vCard-Text in <see cref="VCard"/>-Aggregate.<br />
    /// Nutzt <see cref="IVCardLineReader"/> für Line Folding und Escaping.
    /// </summary>
    public class VCardParser : IVCardParser
    {
        private static readonly Regex VCardBlockRegex = new(
            @"BEGIN:VCARD\r?\n(?<content>.*?)\r?\nEND:VCARD",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IVCardLineReader _lineReader;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="VCardParser"/>-Klasse.
        /// </summary>
        /// <param name="lineReader">Zeilenleser für Normalisierung.</param>
        public VCardParser(IVCardLineReader lineReader)
        {
            this._lineReader = lineReader ?? throw new ArgumentNullException(nameof(lineReader));
        }

        /// <inheritdoc />
        public VCard Parse(string vCardText)
        {
            if (string.IsNullOrWhiteSpace(vCardText))
            {
                throw new VCardParseException("vCard-Text darf nicht leer sein.");
            }

            var normalized = this._lineReader.Normalize(vCardText.Trim());

            if (!normalized.StartsWith("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
            {
                throw new VCardParseException("Ungültiger vCard-Beginn: BEGIN:VCARD fehlt.", lineNumber: 1);
            }

            return VCardBlockParser.Parse(normalized);
        }

        /// <inheritdoc />
        public IReadOnlyList<VCard> ParseDocument(string vCardText)
        {
            if (string.IsNullOrWhiteSpace(vCardText))
            {
                throw new VCardParseException("vCard-Text darf nicht leer sein.");
            }

            var normalized = this._lineReader.Normalize(vCardText.Trim());
            var matches = VCardBlockRegex.Matches(normalized);

            if (matches.Count == 0)
            {
                throw new VCardParseException("Kein gültiger vCard-Block (BEGIN:VCARD … END:VCARD) gefunden.");
            }

            var result = new List<VCard>(matches.Count);

            foreach (Match match in matches)
            {
                var block = $"BEGIN:VCARD\r\n{match.Groups["content"].Value}\r\nEND:VCARD";
                result.Add(VCardBlockParser.Parse(block));
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<VCard> ParseAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            var text = await VCardTextReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false);
            return this.Parse(text);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<VCard>> ParseDocumentAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            var text = await VCardTextReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false);
            return this.ParseDocument(text);
        }
    }
}
