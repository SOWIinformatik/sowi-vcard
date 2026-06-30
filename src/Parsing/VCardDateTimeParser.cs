// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using System.Globalization;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Parst Datums- und Zeitwerte aus vCard-Properties.
    /// </summary>
    internal static class VCardDateTimeParser
    {
        private static readonly string[] Formats =
        {
            "yyyyMMddTHHmmssZ",
            "yyyy-MM-ddTHH:mm:ssZ",
            "dd.MM.yyyy",
            "MM/dd/yyyy",
            "yyyy-MM-dd",
            "yyyyMMdd",
        };

        internal static DateTime? Parse(string value)
        {
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime) ||
                DateTime.TryParseExact(value, Formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime))
            {
                if (value.EndsWith('Z'))
                {
                    dateTime = dateTime.ToUniversalTime();
                }

                return dateTime;
            }

            return null;
        }
    }
}
