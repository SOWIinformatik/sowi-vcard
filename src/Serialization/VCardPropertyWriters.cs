// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Parsing.VersionStrategies;

namespace SOWI.vCard.Serialization
{
    /// <summary>
    /// Schreibt Value Objects als vCard-Property-Zeilen.
    /// </summary>
    internal static class VCardPropertyWriters
    {
        internal static string WritePersonNameValue(PersonName name)
        {
            var components = new[]
            {
                name.LastName ?? string.Empty,
                name.FirstName ?? string.Empty,
                name.MiddleName ?? string.Empty,
                name.Prefix ?? string.Empty,
                name.Suffix ?? string.Empty,
            };
            return string.Join(";", components);
        }

        internal static string WriteAddress(Address address, string version)
        {
            var value = $"{address.PostOfficeBox};{address.ExtendedAddress};{address.Street};{address.Locality};{address.Region};{address.PostalCode};{address.Country}";
            var parameters = new List<string>();

            if (!string.IsNullOrEmpty(address.Type))
            {
                parameters.Add($"TYPE={address.Type}");
            }

            if (version == "4.0" && !string.IsNullOrEmpty(address.Label))
            {
                parameters.Add($"LABEL=\"{EscapeLabel(address.Label)}\"");
            }

            return parameters.Count == 0
                ? $"ADR:{value}"
                : $"ADR;{string.Join(";", parameters)}:{value}";
        }

        internal static string WritePostalLabel(PostalLabel label)
        {
            var text = label.Text.Replace("\n", "\\n", StringComparison.Ordinal);
            return string.IsNullOrEmpty(label.Type)
                ? $"LABEL:{text}"
                : $"LABEL;TYPE={label.Type}:{text}";
        }

        private static string EscapeLabel(string label)
        {
            return label
                .Replace("\\", "\\\\", StringComparison.Ordinal)
                .Replace("\n", "\\n", StringComparison.Ordinal)
                .Replace(";", "\\;", StringComparison.Ordinal)
                .Replace(",", "\\,", StringComparison.Ordinal);
        }

        internal static string WriteEmail(Email email)
        {
            return string.IsNullOrEmpty(email.Type)
                ? $"EMAIL:{email.Address}"
                : $"EMAIL;TYPE={email.Type}:{email.Address}";
        }

        internal static string WriteRelatedPerson(RelatedPerson related)
        {
            return string.IsNullOrEmpty(related.Type)
                ? $"RELATED:{related.Value}"
                : $"RELATED;TYPE={related.Type}:{related.Value}";
        }

        internal static string WriteClientPidMap(ClientPidMap map) => $"CLIENTPIDMAP:{map.Pid};{map.Uri}";

        internal static string WritePhoneNumber(PhoneNumber phone, IVCardVersionStrategy strategy)
            => strategy.WritePhoneNumber(phone);

        internal static string WriteGeoLocation(GeoLocation geo, IVCardVersionStrategy strategy)
            => strategy.WriteGeoLocation(geo);

        internal static string WritePhoto(Photo photo) => WriteMedia("PHOTO", photo);

        internal static string WriteLogo(Photo logo) => WriteMedia("LOGO", logo);

        internal static string WriteSound(Photo sound) => WriteMedia("SOUND", sound);

        internal static string WriteKey(PublicKey key)
        {
            if (key.Version == "2.1")
            {
                return key.Data != null
                    ? $"KEY;{key.Type};ENCODING=BASE64:{GetBase64WithLineBreaks(key.Data, key.DataLineLength)}"
                    : string.IsNullOrEmpty(key.Type)
                        ? $"KEY:{key.Value}"
                        : $"KEY;{key.Type}:{key.Value}";
            }

            if (key.Version == "3.0")
            {
                return key.Data != null
                    ? $"KEY;TYPE={key.Type};ENCODING=b:{GetBase64WithLineBreaks(key.Data, key.DataLineLength)}"
                    : $"KEY;TYPE={key.Type}:{key.Value}";
            }

            if (key.Version == "4.0")
            {
                var mediaType = key.Type.Contains('/', StringComparison.Ordinal)
                    ? key.Type
                    : $"application/{key.Type.ToLowerInvariant()}-keys";

                if (key.Data != null)
                {
                    return $"KEY:data:{mediaType};base64,{Convert.ToBase64String(key.Data)}";
                }

                return $"KEY;MEDIATYPE={mediaType}:{key.Value}";
            }

            return string.IsNullOrEmpty(key.Type)
                ? $"KEY:{key.Value}"
                : $"KEY;TYPE={key.Type}:{key.Value}";
        }

        internal static string WriteLeveledText(string propertyName, LeveledText item)
        {
            return string.IsNullOrEmpty(item.Level)
                ? $"{propertyName}:{item.Text}"
                : $"{propertyName};LEVEL={item.Level}:{item.Text}";
        }

        internal static string WriteBirthplace(string birthplace)
            => $"BIRTHPLACE;VALUE=text:{birthplace.Replace(",", "\\,", StringComparison.Ordinal)}";

        private static string WriteMedia(string propertyName, Photo media)
        {
            if (media.Version == "2.1")
            {
                return media.Data != null
                    ? $"{propertyName};{media.Type};ENCODING=BASE64:{GetBase64WithLineBreaks(media)}"
                    : $"{propertyName};{media.Type}:{media.Value}";
            }

            if (media.Version == "3.0")
            {
                if (media.Data != null)
                {
                    return $"{propertyName};TYPE={media.Type};ENCODING=b:{GetBase64WithLineBreaks(media)}";
                }

                if (!string.IsNullOrEmpty(media.Value) &&
                    (media.Value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                     media.Value.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                {
                    return $"{propertyName};VALUE=URL;TYPE={media.Type}:{media.Value}";
                }

                return $"{propertyName};TYPE={media.Type}:{media.Value}";
            }

            if (media.Version == "3.a")
            {
                return media.Data != null
                    ? $"{propertyName};TYPE={media.Type};ENCODING=BASE64;VALUE=BINARY:{GetBase64WithLineBreaks(media)}"
                    : $"{propertyName};TYPE={media.Type}:{media.Value}";
            }

            if (media.Version == "4.0")
            {
                var mediaType = media.Type.Contains('/', StringComparison.Ordinal)
                    ? media.Type
                    : propertyName switch
                    {
                        "SOUND" => $"audio/{media.Type.ToLowerInvariant()}",
                        _ => $"image/{media.Type.ToLowerInvariant()}",
                    };

                return media.Data != null
                    ? $"{propertyName};MEDIATYPE={mediaType};{GetBase64WithLineBreaks(media)}"
                    : $"{propertyName};MEDIATYPE={mediaType}:{media.Value}";
            }

            return media.Data != null
                ? $"{propertyName};TYPE={media.Type};ENCODING=BASE64:{GetBase64WithLineBreaks(media)}"
                : $"{propertyName};TYPE={media.Type}:{media.Value}";
        }

        private static string GetBase64WithLineBreaks(Photo photo)
            => GetBase64WithLineBreaks(photo.Data, photo.DataLineLength);

        private static string GetBase64WithLineBreaks(byte[]? data, int dataLineLength)
        {
            if (data == null)
            {
                return string.Empty;
            }

            var base64String = Convert.ToBase64String(data);
            if (dataLineLength == 0)
            {
                return base64String;
            }

            var base64Lines = new List<string>();
            for (var i = 0; i < base64String.Length; i += dataLineLength)
            {
                var line = i > 0 ? "  " : string.Empty;
                line += i + dataLineLength < base64String.Length
                    ? base64String.Substring(i, dataLineLength)
                    : base64String.Substring(i);
                base64Lines.Add(line);
            }

            base64Lines.Add("  ");
            return string.Join("\r\n", base64Lines);
        }
    }
}
