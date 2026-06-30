// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.Exceptions;
using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Parst einzelne vCard-Value-Objects aus Property-Zeilen.
    /// </summary>
    internal static class VCardPropertyParsers
    {
        internal static PersonName ParsePersonName(string value)
        {
            var components = value.Split(';');
            return new PersonName
            {
                LastName = components.Length > 0 ? components[0] : string.Empty,
                FirstName = components.Length > 1 ? components[1] : string.Empty,
                MiddleName = components.Length > 2 ? components[2] : string.Empty,
                Prefix = components.Length > 3 ? components[3] : string.Empty,
                Suffix = components.Length > 4 ? components[4] : string.Empty,
            };
        }

        internal static Address ParseAddress(string line)
        {
            if (!VCardPropertyLineParser.TrySplit(line, out var propertyPart, out var value))
            {
                throw new VCardParseException("Ungültiges ADR-Format.", propertyName: "ADR");
            }

            if (!propertyPart.StartsWith("ADR", StringComparison.Ordinal))
            {
                throw new VCardParseException("Ungültiges ADR-Format.", propertyName: "ADR");
            }

            var address = new Address
            {
                Type = VCardPropertyLineParser.ExtractTypeParameter(propertyPart),
                Label = VCardPropertyLineParser.ExtractLabelParameter(propertyPart),
            };

            var components = value.Split(';');
            address.PostOfficeBox = components.Length > 0 ? components[0] : string.Empty;
            address.ExtendedAddress = components.Length > 1 ? components[1] : string.Empty;
            address.Street = components.Length > 2 ? components[2] : string.Empty;
            address.Locality = components.Length > 3 ? components[3] : string.Empty;
            address.Region = components.Length > 4 ? components[4] : string.Empty;
            address.PostalCode = components.Length > 5 ? components[5] : string.Empty;
            address.Country = components.Length > 6 ? components[6] : string.Empty;
            return address;
        }

        internal static Email ParseEmail(string line)
        {
            if (!VCardPropertyLineParser.TrySplit(line, out var propertyPart, out var value))
            {
                throw new VCardParseException("Ungültiges EMAIL-Format.", propertyName: "EMAIL");
            }

            if (!propertyPart.StartsWith("EMAIL", StringComparison.Ordinal))
            {
                throw new VCardParseException("Ungültiges EMAIL-Format.", propertyName: "EMAIL");
            }

            return new Email
            {
                Type = VCardPropertyLineParser.ExtractTypeParameter(propertyPart),
                Address = value,
            };
        }

        internal static RelatedPerson ParseRelatedPerson(string line)
        {
            if (!VCardPropertyLineParser.TrySplit(line, out var propertyPart, out var value))
            {
                throw new VCardParseException("Ungültiges RELATED-Format.", propertyName: "RELATED");
            }

            if (!propertyPart.StartsWith("RELATED", StringComparison.Ordinal))
            {
                throw new VCardParseException("Ungültiges RELATED-Format.", propertyName: "RELATED");
            }

            return new RelatedPerson
            {
                Type = VCardPropertyLineParser.ExtractTypeParameter(propertyPart),
                Value = value,
            };
        }

        internal static ClientPidMap ParseClientPidMap(string line)
        {
            var components = line.Split(':');
            if (components.Length == 3 && int.TryParse(components[1], out var pid))
            {
                return new ClientPidMap { Pid = pid, Uri = components[2] };
            }

            throw new VCardParseException("Ungültiges CLIENTPIDMAP-Format.", propertyName: "CLIENTPIDMAP");
        }

        internal static LeveledText ParseLeveledProperty(string line, string propertyName)
        {
            if (!VCardPropertyLineParser.TrySplit(line, out var propertyPart, out var value))
            {
                throw new VCardParseException($"Ungültiges {propertyName}-Format.", propertyName: propertyName);
            }

            return new LeveledText
            {
                Level = VCardPropertyLineParser.ExtractParameter(propertyPart, "LEVEL") ?? string.Empty,
                Text = value,
            };
        }

        internal static string ParseTextValueProperty(string line, string propertyName)
        {
            if (!VCardPropertyLineParser.TrySplit(line, out _, out var value))
            {
                throw new VCardParseException($"Ungültiges {propertyName}-Format.", propertyName: propertyName);
            }

            return value;
        }

        internal static PostalLabel ParsePostalLabel(string line)
        {
            if (!VCardPropertyLineParser.TrySplit(line, out var propertyPart, out var value))
            {
                throw new VCardParseException("Ungültiges LABEL-Format.", propertyName: "LABEL");
            }

            if (!propertyPart.StartsWith("LABEL", StringComparison.Ordinal))
            {
                throw new VCardParseException("Ungültiges LABEL-Format.", propertyName: "LABEL");
            }

            return new PostalLabel
            {
                Type = VCardPropertyLineParser.ExtractTypeParameter(propertyPart),
                Text = value.Replace("\\n", "\n", StringComparison.Ordinal),
            };
        }
    }
}
