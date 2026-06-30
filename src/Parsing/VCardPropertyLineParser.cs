// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Zerlegt eine vCard-Property-Zeile in Name, Parameter und Wert.
    /// </summary>
    internal static class VCardPropertyLineParser
    {
        /// <summary>
        /// Teilt eine Zeile am ersten Doppelpunkt in Property-Teil und Wert.
        /// </summary>
        internal static bool TrySplit(string line, out string propertyPart, out string value)
        {
            var index = line.IndexOf(':');
            if (index < 0)
            {
                propertyPart = string.Empty;
                value = string.Empty;
                return false;
            }

            propertyPart = line[..index];
            value = line[(index + 1)..];
            return true;
        }

        /// <summary>
        /// Extrahiert den Property-Namen (vor dem ersten Semikolon).
        /// </summary>
        internal static string GetPropertyName(string propertyPart)
        {
            var index = propertyPart.IndexOf(';');
            return index < 0 ? propertyPart : propertyPart[..index];
        }

        /// <summary>
        /// Extrahiert einen benannten Parameter (z. B. VALUE=uri).
        /// </summary>
        internal static string? ExtractParameter(string propertyPart, string parameterName)
        {
            foreach (var part in propertyPart.Split(';').Skip(1))
            {
                var prefix = parameterName + "=";
                if (part.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return part[prefix.Length..];
                }
            }

            return null;
        }

        /// <summary>
        /// Extrahiert TYPE-Parameter (v2.1 Mehrfachparameter, v3/v4 TYPE=).
        /// </summary>
        internal static string ExtractTypeParameter(string propertyPart)
        {
            var parts = propertyPart.Split(';');
            if (parts.Length < 2)
            {
                return string.Empty;
            }

            foreach (var part in parts.Skip(1))
            {
                if (part.StartsWith("TYPE=", StringComparison.OrdinalIgnoreCase))
                {
                    return part["TYPE=".Length..];
                }
            }

            var typeParts = new List<string>();
            foreach (var part in parts.Skip(1))
            {
                if (part.StartsWith("VALUE=", StringComparison.OrdinalIgnoreCase) ||
                    part.StartsWith("LABEL=", StringComparison.OrdinalIgnoreCase) ||
                    part.StartsWith("MEDIATYPE=", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                typeParts.Add(part);
            }

            return string.Join(",", typeParts);
        }

        /// <summary>
        /// Extrahiert den LABEL-Parameter (ggf. in Anführungszeichen).
        /// </summary>
        internal static string ExtractLabelParameter(string propertyPart)
        {
            foreach (var part in propertyPart.Split(';').Skip(1))
            {
                if (!part.StartsWith("LABEL=", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var labelValue = part["LABEL=".Length..];
                if (labelValue.StartsWith('"') && labelValue.EndsWith('"') && labelValue.Length >= 2)
                {
                    return UnquoteLabel(labelValue[1..^1]);
                }

                return labelValue;
            }

            return string.Empty;
        }

        private static string UnquoteLabel(string value)
        {
            return value
                .Replace("\\n", "\n", StringComparison.Ordinal)
                .Replace("\\N", "\n", StringComparison.Ordinal)
                .Replace("\\;", ";", StringComparison.Ordinal)
                .Replace("\\,", ",", StringComparison.Ordinal)
                .Replace("\\\\", "\\", StringComparison.Ordinal);
        }

        /// <summary>
        /// Ermittelt die VERSION-Zeile aus einem vCard-Block.
        /// </summary>
        internal static string DetectVersion(string normalizedBlock)
        {
            var lines = normalizedBlock.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (TrySplit(line, out var propertyPart, out var value) &&
                    GetPropertyName(propertyPart).Equals("VERSION", StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            return string.Empty;
        }
    }
}
