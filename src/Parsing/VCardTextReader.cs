// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using System.Text;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Liest vCard-Text aus Streams und Dateien.
    /// </summary>
    internal static class VCardTextReader
    {
        internal static async Task<string> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);

            using var reader = new StreamReader(
                stream,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 4096,
                leaveOpen: true);

            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        internal static async Task<string> ReadFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Der Dateipfad darf nicht leer sein.", nameof(filePath));
            }

            await using var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true);

            return await ReadAsync(stream, cancellationToken).ConfigureAwait(false);
        }
    }
}
