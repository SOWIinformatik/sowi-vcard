// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain;
using SOWI.vCard.Parsing.VersionStrategies;

namespace SOWI.vCard.Serialization
{
    /// <summary>
    /// Serialisiert <see cref="VCard"/>-Aggregate in RFC-vCard-Text.
    /// </summary>
    public class VCardSerializer : IVCardSerializer
    {
        /// <inheritdoc />
        public string Serialize(VCard vCard)
        {
            ArgumentNullException.ThrowIfNull(vCard);

            var strategy = VCardVersionStrategyResolver.Resolve(vCard.Version);
            var lines = new List<string> { "BEGIN:VCARD" };

            AddSimpleLine(lines, "VERSION", vCard.Version);
            AddSimpleLine(lines, "FN", vCard.FullName);
            AddSimpleLine(lines, "N", VCardPropertyWriters.WritePersonNameValue(vCard.PersonName));
            AddSimpleLine(lines, "NICKNAME", vCard.Nickname);
            AddSimpleLine(lines, "KIND", vCard.Kind);

            if (vCard.Photo != null)
            {
                lines.Add(VCardPropertyWriters.WritePhoto(vCard.Photo));
            }

            if (vCard.Logo != null)
            {
                lines.Add(VCardPropertyWriters.WriteLogo(vCard.Logo));
            }

            if (vCard.Sound != null)
            {
                lines.Add(VCardPropertyWriters.WriteSound(vCard.Sound));
            }

            if (vCard.Birthday.HasValue)
            {
                lines.Add($"BDAY:{strategy.FormatDate(vCard.Birthday.Value)}");
            }

            lines.AddRange(vCard.Addresses.Select(a => VCardPropertyWriters.WriteAddress(a, vCard.Version)));
            lines.AddRange(vCard.PostalLabels.Select(VCardPropertyWriters.WritePostalLabel));
            lines.AddRange(vCard.PhoneNumbers.Select(p => VCardPropertyWriters.WritePhoneNumber(p, strategy)));
            lines.AddRange(vCard.Emails.Select(VCardPropertyWriters.WriteEmail));
            lines.AddRange(vCard.Impps.Select(impp => $"IMPP:{impp}"));
            lines.AddRange(vCard.Members.Select(member => $"MEMBER:{member}"));

            AddSimpleLine(lines, "TITLE", vCard.Title);
            AddSimpleLine(lines, "ROLE", vCard.Role);
            AddSimpleLine(lines, "ORG", vCard.Organization);
            AddSimpleLine(lines, "URL", vCard.Url);
            AddSimpleLine(lines, "NOTE", vCard.Note);
            AddSimpleLine(lines, "MAILER", vCard.Mailer);
            AddSimpleLine(lines, "PROFILE", vCard.Profile);
            AddSimpleLine(lines, "XML", vCard.Xml);
            AddSimpleLine(lines, "SOURCE", vCard.Source);
            AddSimpleLine(lines, "NAME", vCard.Name);
            AddSimpleLine(lines, "AGENT", vCard.Agent);

            if (vCard.Anniversary.HasValue)
            {
                lines.Add($"ANNIVERSARY:{strategy.FormatDate(vCard.Anniversary.Value)}");
            }

            AddSimpleLine(lines, "GENDER", vCard.Gender);

            if (vCard.Categories.Count > 0)
            {
                lines.Add($"CATEGORIES:{string.Join(",", vCard.Categories)}");
            }

            AddSimpleLine(lines, "TZ", vCard.Timezone);

            if (vCard.Geo.IsValid())
            {
                lines.Add(VCardPropertyWriters.WriteGeoLocation(vCard.Geo, strategy));
            }

            if (vCard.Languages.Count > 0)
            {
                lines.Add($"LANG:{string.Join(",", vCard.Languages)}");
            }

            lines.AddRange(vCard.Related.Select(VCardPropertyWriters.WriteRelatedPerson));

            if (vCard.Created.HasValue)
            {
                lines.Add($"CREATED:{strategy.FormatDateTime(vCard.Created.Value)}");
            }

            if (vCard.Timestamp.HasValue)
            {
                lines.Add($"DTSTAMP:{strategy.FormatDateTime(vCard.Timestamp.Value)}");
            }

            if (vCard.Revision.HasValue)
            {
                lines.Add($"REV:{strategy.FormatDateTime(vCard.Revision.Value)}");
            }

            AddSimpleLine(lines, "UID", vCard.Uid);
            AddSimpleLine(lines, "CLASS", vCard.Classification);

            if (vCard.Key != null)
            {
                lines.Add(VCardPropertyWriters.WriteKey(vCard.Key));
            }

            if (!string.IsNullOrEmpty(vCard.Birthplace))
            {
                lines.Add(VCardPropertyWriters.WriteBirthplace(vCard.Birthplace));
            }

            if (vCard.DeathDate.HasValue)
            {
                lines.Add($"DEATHDATE:{strategy.FormatDate(vCard.DeathDate.Value)}");
            }

            AddSimpleLine(lines, "DEATHPLACE", vCard.DeathPlace);
            lines.AddRange(vCard.Expertises.Select(e => VCardPropertyWriters.WriteLeveledText("EXPERTISE", e)));
            lines.AddRange(vCard.Hobbies.Select(h => VCardPropertyWriters.WriteLeveledText("HOBBY", h)));
            lines.AddRange(vCard.Interests.Select(i => VCardPropertyWriters.WriteLeveledText("INTEREST", i)));
            AddSimpleLine(lines, "ORG-DIRECTORY", vCard.OrgDirectory);

            AddSimpleLine(lines, "PRODID", vCard.ProductIdentifier);
            AddSimpleLine(lines, "SORT-STRING", vCard.SortString);
            lines.AddRange(vCard.ClientPidMaps.Select(VCardPropertyWriters.WriteClientPidMap));
            AddSimpleLine(lines, "FBURL", vCard.FreeBusyUrl);
            AddSimpleLine(lines, "CALADRURI", vCard.CalendarAddressUri);
            AddSimpleLine(lines, "CALURI", vCard.CalendarUri);

            foreach (var extension in vCard.Extensions.Values)
            {
                lines.Add(extension);
            }

            foreach (var other in vCard.Others.Values)
            {
                lines.Add(other);
            }

            lines.Add("END:VCARD");
            lines.Add(string.Empty);
            return string.Join("\r\n", lines);
        }

        /// <inheritdoc />
        public string SerializeDocument(IReadOnlyList<VCard> vCards)
        {
            ArgumentNullException.ThrowIfNull(vCards);

            if (vCards.Count == 0)
            {
                return string.Empty;
            }

            return string.Join("\r\n", vCards.Select(this.Serialize).Select(text => text.TrimEnd('\r', '\n')));
        }

        private static void AddSimpleLine(List<string> lines, string name, string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                lines.Add($"{name}:{value}");
            }
        }
    }
}
