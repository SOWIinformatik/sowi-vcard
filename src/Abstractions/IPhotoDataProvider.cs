// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Abstractions
{
    /// <summary>
    /// Optionaler Zugriff auf Photo-Binärdaten (z. B. Default-Portrait aus Ressourcen).
    /// </summary>
    public interface IPhotoDataProvider
    {
        /// <summary>
        /// Lädt das Standard-Portrait, wenn kein Photo eingebettet ist.
        /// </summary>
        /// <returns>Portrait-Bytes oder null.</returns>
        byte[]? LoadDefaultPortrait();

        /// <summary>
        /// Lädt die Bytes eines Photo-Objekts (eingebettet oder Fallback).
        /// </summary>
        /// <param name="photo">Photo-Objekt.</param>
        /// <returns>Bild-Bytes oder null.</returns>
        byte[]? LoadPhotoData(Photo photo);
    }
}
