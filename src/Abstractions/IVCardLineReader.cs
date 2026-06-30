// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Abstractions
{
    /// <summary>
    /// Liest und normalisiert vCard-Text (Line Folding, Escaping) gemäss RFC 6350.
    /// </summary>
    public interface IVCardLineReader
    {
        /// <summary>
        /// Liest den vCard-Text und liefert entfaltete, unescaped logische Zeilen.
        /// </summary>
        /// <param name="vCardText">Roher vCard-Text.</param>
        /// <returns>Liste logischer Property-Zeilen.</returns>
        IReadOnlyList<string> ReadLines(string vCardText);

        /// <summary>
        /// Normalisiert den vCard-Text zu einem String mit entfalteten Zeilen (CRLF-getrennt).
        /// </summary>
        /// <param name="vCardText">Roher vCard-Text.</param>
        /// <returns>Normalisierter vCard-Text.</returns>
        string Normalize(string vCardText);
    }
}
