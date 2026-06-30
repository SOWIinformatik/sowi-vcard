// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using System.Text;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Liest und normalisiert vCard-Text: Line Folding und Escaping gemäss RFC 6350.
    /// </summary>
    public class VCardLineReader : IVCardLineReader
    {
        private static readonly string[] LineSeparators = { "\r\n", "\n", "\r" };

        /// <inheritdoc />
        public IReadOnlyList<string> ReadLines(string vCardText)
        {
            if (string.IsNullOrEmpty(vCardText))
            {
                return Array.Empty<string>();
            }

            var rawLines = vCardText.Split(LineSeparators, StringSplitOptions.None);
            var unfolded = this.UnfoldLines(rawLines);

            return unfolded
                .Select(UnescapeLine)
                .Where(line => line.Length > 0)
                .ToList();
        }

        /// <inheritdoc />
        public string Normalize(string vCardText)
        {
            return string.Join("\r\n", this.ReadLines(vCardText));
        }

        /// <summary>
        /// Führt RFC-6350-Line-Folding zusammen: Fortsetzungszeilen beginnen mit Leerzeichen oder Tabulator.
        /// </summary>
        private List<string> UnfoldLines(string[] rawLines)
        {
            var result = new List<string>();

            foreach (var line in rawLines)
            {
                if (result.Count > 0 && (line.StartsWith(' ') || line.StartsWith('\t')))
                {
                    result[^1] += line[1..];
                }
                else
                {
                    result.Add(line);
                }
            }

            return result;
        }

        /// <summary>
        /// Wandelt RFC-6350-Escape-Sequenzen in der Property-Zeile in echte Zeichen um.<br />
        /// In Anführungszeichen (z. B. ADR LABEL) bleiben \n-Sequenzen erhalten.
        /// </summary>
        private static string UnescapeLine(string line)
        {
            if (!line.Contains('\\', StringComparison.Ordinal))
            {
                return line;
            }

            var sb = new StringBuilder(line.Length);
            var inQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '"' && (i == 0 || line[i - 1] != '\\'))
                {
                    inQuotes = !inQuotes;
                    sb.Append(line[i]);
                    continue;
                }

                if (line[i] == '\\' && i + 1 < line.Length)
                {
                    if (inQuotes && (line[i + 1] == 'n' || line[i + 1] == 'N'))
                    {
                        sb.Append(line[i]);
                        sb.Append(line[i + 1]);
                        i++;
                        continue;
                    }

                    switch (line[i + 1])
                    {
                        case 'n':
                        case 'N':
                            sb.Append('\n');
                            i++;
                            break;
                        case '\\':
                            sb.Append('\\');
                            i++;
                            break;
                        case ';':
                            sb.Append(';');
                            i++;
                            break;
                        case ',':
                            sb.Append(',');
                            i++;
                            break;
                        default:
                            sb.Append(line[i]);
                            break;
                    }
                }
                else
                {
                    sb.Append(line[i]);
                }
            }

            return sb.ToString();
        }
    }
}
