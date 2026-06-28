// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.Exceptions;
using SOWI.vCard.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Parst KEY-Properties (URL, eingebettete Base64-Daten, data:-URI).
    /// </summary>
    internal static class KeyPropertyParser
    {
        internal static PublicKey Parse(string line, string vCardContent)
        {
            if (!VCardPropertyLineParser.TrySplit(line, out var propertyPart, out var value))
            {
                throw new VCardParseException("Ungültiges KEY-Format.", propertyName: "KEY");
            }

            var key = new PublicKey { Value = value };
            var mediaType = VCardPropertyLineParser.ExtractParameter(propertyPart, "MEDIATYPE");
            var type = VCardPropertyLineParser.ExtractTypeParameter(propertyPart);
            var encoding = VCardPropertyLineParser.ExtractParameter(propertyPart, "ENCODING");

            if (!string.IsNullOrEmpty(mediaType))
            {
                key.Version = "4.0";
                key.Type = mediaType;
            }
            else if (value.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                key.Version = "4.0";
                key.Type = ExtractDataUriMediaType(value);
                var base64Part = value[(value.IndexOf(',', StringComparison.Ordinal) + 1)..];
                key.Data = Convert.FromBase64String(base64Part);
            }
            else if (!string.IsNullOrEmpty(type))
            {
                key.Version = propertyPart.Contains("TYPE=", StringComparison.Ordinal) ? "3.0" : "2.1";
                key.Type = type.Split(',')[0];
                key.Encoding = encoding ?? string.Empty;

                if (encoding != null &&
                    (encoding.Equals("b", StringComparison.OrdinalIgnoreCase) ||
                     encoding.Equals("BASE64", StringComparison.OrdinalIgnoreCase)))
                {
                    key.Data = Convert.FromBase64String(value);
                }
            }
            else if (propertyPart.Contains(';'))
            {
                key.Version = "2.1";
                var parts = propertyPart.Split(';');
                key.Type = parts.Length > 1 ? parts[1] : string.Empty;
                key.Encoding = encoding ?? string.Empty;

                if (encoding == "BASE64")
                {
                    key.Data = Convert.FromBase64String(value);
                }
            }

            LoadEmbeddedData(key, vCardContent);
            return key;
        }

        private static string ExtractDataUriMediaType(string dataUri)
        {
            var withoutScheme = dataUri["data:".Length..];
            var semicolonIndex = withoutScheme.IndexOf(';', StringComparison.Ordinal);
            return semicolonIndex > 0 ? withoutScheme[..semicolonIndex] : withoutScheme;
        }

        private static void LoadEmbeddedData(PublicKey key, string vCardContent)
        {
            if (key.Data != null)
            {
                return;
            }

            const string pattern = @"KEY.*?;ENCODING=BASE64.*?:(?<keyData>[A-Za-z0-9+/=\s]+)";
            var match = Regex.Match(vCardContent, pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return;
            }

            var keyData = match.Groups["keyData"].Value;
            var base64 = string.Empty;
            var lines = keyData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var dataLine in lines)
            {
                if (dataLine.Trim().Length == 0)
                {
                    break;
                }

                base64 += dataLine.Trim();
                if (key.DataLineLength < dataLine.Trim().Length)
                {
                    key.DataLineLength = dataLine.Trim().Length;
                }
            }

            base64 = base64.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);

            if (base64.Length % 4 != 0)
            {
                return;
            }

            try
            {
                key.Data = Convert.FromBase64String(base64);
            }
            catch (FormatException ex)
            {
                throw new VCardParseException(
                    $"Fehler bei der Base64-Dekodierung von KEY: {ex.Message}",
                    propertyName: "KEY",
                    inner: ex);
            }
        }
    }
}
