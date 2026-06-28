// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Services
{
    /// <summary>
    /// Lädt Photo-Daten aus eingebetteter Ressource oder dem Ausgabeverzeichnis.
    /// </summary>
    public class FilePhotoDataProvider : IPhotoDataProvider
    {
        /// <inheritdoc />
        public byte[]? LoadDefaultPortrait()
        {
            return this.LoadEmbeddedPortrait() ?? this.LoadPortraitFromOutputDirectory();
        }

        /// <inheritdoc />
        public byte[]? LoadPhotoData(Photo photo)
        {
            ArgumentNullException.ThrowIfNull(photo);

            if (photo.Data != null && photo.Data.Length > 0)
            {
                return photo.Data;
            }

            if (!string.IsNullOrEmpty(photo.Value) && File.Exists(photo.Value))
            {
                return File.ReadAllBytes(photo.Value);
            }

            return this.LoadDefaultPortrait();
        }

        private byte[]? LoadEmbeddedPortrait()
        {
            var assembly = typeof(FilePhotoDataProvider).Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith("Portrait.jpg", StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
            {
                return null;
            }

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return null;
            }

            using var memory = new MemoryStream();
            stream.CopyTo(memory);
            return memory.ToArray();
        }

        private byte[]? LoadPortraitFromOutputDirectory()
        {
            var assembly = typeof(FilePhotoDataProvider).Assembly;
            var directory = Path.GetDirectoryName(assembly.Location);
            if (directory == null)
            {
                return null;
            }

            var path = Path.Combine(directory, "Resources", "Portrait.jpg");
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }
    }
}
