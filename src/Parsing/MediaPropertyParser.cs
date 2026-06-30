// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

// Ignore Spelling: jpeg

using SOWI.vCard.Domain.Exceptions;
using SOWI.vCard.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Parst binäre Medien-Properties (PHOTO, LOGO, SOUND).
    /// </summary>
    internal static class MediaPropertyParser
    {
        internal static Photo ParsePhoto(string line, string vCardContent)
            => Parse("PHOTO", line, vCardContent);

        internal static Photo ParseLogo(string line, string vCardContent)
            => Parse("LOGO", line, vCardContent);

        internal static Photo ParseSound(string line, string vCardContent)
            => Parse("SOUND", line, vCardContent);

        internal static Photo Parse(string propertyName, string line, string vCardContent)
        {
            var v2_1Pattern = $"{propertyName};(?<type>JPEG|PNG|GIF|OGG);?(ENCODING=(?<encoding>BASE64))?:(?<value>.*)";
            var v3_0UrlPattern = $"{propertyName};VALUE=URL;TYPE=(?<type>JPEG|PNG|GIF|OGG):(?<value>.*)";
            var v3_0Pattern = $"{propertyName};TYPE=(?<type>JPEG|PNG|GIF|OGG);?(ENCODING=(?<encoding>[Bb])):(?<value>.*)";
            var v3APattern = $"{propertyName};TYPE=(?<type>[^;]+);ENCODING=(?<encoding>[^;]+);VALUE=(?<value>[^:]+):(?<binaryData>.+)";
            var v4_0Pattern = $"{propertyName};MEDIATYPE=(?<type>[^:]+):(?<value>.*)";
            var v4_0DataPattern = $"{propertyName}:(?<value>data:[^,]+,.*)";

            var media = new Photo();

            if (Regex.Match(line, v2_1Pattern) is { Success: true } matchV21)
            {
                media.Version = "2.1";
                media.Type = matchV21.Groups["type"].Value;
                media.Encoding = matchV21.Groups["encoding"].Value;
                media.Value = matchV21.Groups["value"].Value;
                if (media.Encoding == "BASE64")
                {
                    media.Data = Convert.FromBase64String(media.Value);
                }
            }
            else if (Regex.Match(line, v3_0UrlPattern) is { Success: true } matchV30Url)
            {
                media.Version = "3.0";
                media.Type = matchV30Url.Groups["type"].Value;
                media.Value = matchV30Url.Groups["value"].Value;
            }
            else if (Regex.Match(line, v3_0Pattern) is { Success: true } matchV30)
            {
                media.Version = "3.0";
                media.Type = matchV30.Groups["type"].Value;
                media.Encoding = matchV30.Groups["encoding"].Value;
                media.Value = matchV30.Groups["value"].Value;
                if (media.Encoding == "b")
                {
                    media.Data = Convert.FromBase64String(media.Value);
                }
            }
            else if (Regex.Match(line, v3APattern) is { Success: true } matchV3A)
            {
                media.Version = "3.a";
                media.Type = matchV3A.Groups["type"].Value;
                media.Encoding = matchV3A.Groups["encoding"].Value;
                media.Value = matchV3A.Groups["value"].Value;
                if (media.Encoding == "BASE64" && media.Value == "BINARY")
                {
                    media.Data = Convert.FromBase64String(matchV3A.Groups["binaryData"].Value);
                }
            }
            else if (Regex.Match(line, v4_0Pattern) is { Success: true } matchV40)
            {
                media.Version = "4.0";
                media.Type = matchV40.Groups["type"].Value;
                media.Value = matchV40.Groups["value"].Value;
            }
            else if (Regex.Match(line, v4_0DataPattern) is { Success: true } matchV40Data)
            {
                media.Version = "4.0";
                media.Value = matchV40Data.Groups["value"].Value;
                var base64Part = media.Value[(media.Value.IndexOf(',', StringComparison.Ordinal) + 1)..];
                media.Data = Convert.FromBase64String(base64Part);
            }
            else
            {
                throw new VCardParseException($"Ungültiges {propertyName}-Format.", propertyName: propertyName);
            }

            LoadEmbeddedData(propertyName, media, vCardContent);
            return media;
        }

        private static void LoadEmbeddedData(string propertyName, Photo media, string vCardContent)
        {
            var pattern = $@"{propertyName}.*?;ENCODING=BASE64.*?:(?<mediaData>[A-Za-z0-9+/=\s]+)";
            var match = Regex.Match(vCardContent, pattern, RegexOptions.Multiline);
            if (!match.Success)
            {
                return;
            }

            var mediaData = match.Groups["mediaData"].Value;
            var base64 = string.Empty;
            var lines = mediaData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var dataLine in lines)
            {
                if (dataLine.Trim().Length == 0 || dataLine.Trim() == "\t")
                {
                    break;
                }

                base64 += dataLine.Trim();
                if (media.DataLineLength < dataLine.Trim().Length)
                {
                    media.DataLineLength = dataLine.Trim().Length;
                }
            }

            base64 = base64.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
            base64 = base64.Replace('-', '+').Replace('_', '/');

            if (base64.Length % 4 != 0 ||
                !Regex.IsMatch(base64, "^[a-zA-Z0-9+/]*={0,2}$", RegexOptions.None))
            {
                return;
            }

            try
            {
                media.Data = Convert.FromBase64String(base64);
            }
            catch (FormatException ex)
            {
                throw new VCardParseException(
                    $"Fehler bei der Base64-Dekodierung von {propertyName}: {ex.Message}",
                    propertyName: propertyName,
                    inner: ex);
            }
        }
    }
}
