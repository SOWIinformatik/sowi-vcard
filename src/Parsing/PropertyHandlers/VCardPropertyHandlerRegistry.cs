// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Parsing.VersionStrategies;

namespace SOWI.vCard.Parsing.PropertyHandlers
{
    /// <summary>
    /// Registriert und liefert alle Standard-Property-Handler.
    /// </summary>
    internal static class VCardPropertyHandlerRegistry
    {
        /// <summary>
        /// Erstellt die Handler-Liste in Verarbeitungsreihenfolge.
        /// </summary>
        internal static IReadOnlyList<IVCardPropertyHandler> CreateDefault()
        {
            return new IVCardPropertyHandler[]
            {
                new BeginEndPropertyHandler(),
                new VersionPropertyHandler(),
                new ActionPropertyHandler("FN", ctx => ctx.VCard.FullName = ctx.Value),
                new ActionPropertyHandler("N", ctx => ctx.VCard.PersonName = VCardPropertyParsers.ParsePersonName(ctx.Value)),
                new ActionPropertyHandler("NICKNAME", ctx => ctx.VCard.Nickname = ctx.Value),
                new MediaPropertyHandler("PHOTO", (v, l, b) => v.Photo = MediaPropertyParser.ParsePhoto(l, b)),
                new MediaPropertyHandler("LOGO", (v, l, b) => v.Logo = MediaPropertyParser.ParseLogo(l, b)),
                new MediaPropertyHandler("SOUND", (v, l, b) => v.Sound = MediaPropertyParser.ParseSound(l, b)),
                new DateTimePropertyHandler("BDAY", (v, d) => v.Birthday = d),
                new DateTimePropertyHandler("ANNIVERSARY", (v, d) => v.Anniversary = d),
                new DateTimePropertyHandler("CREATED", (v, d) => v.Created = d),
                new DateTimePropertyHandler("DTSTAMP", (v, d) => v.Timestamp = d),
                new DateTimePropertyHandler("REV", (v, d) => v.Revision = d),
                new ActionPropertyHandler("GENDER", ctx => ctx.VCard.Gender = ctx.Value),
                new CategoriesPropertyHandler(),
                new ActionPropertyHandler("TZ", ctx => ctx.VCard.Timezone = ctx.Value),
                new GeoPropertyHandler(),
                new LanguagesPropertyHandler(),
                new AddressPropertyHandler(),
                new TelephonePropertyHandler(),
                new EmailPropertyHandler(),
                new ActionPropertyHandler("IMPP", ctx => ctx.VCard.Impps.Add(ctx.Value)),
                new RelatedPropertyHandler(),
                new ActionPropertyHandler("MEMBER", ctx => ctx.VCard.Members.Add(ctx.Value)),
                new ActionPropertyHandler("TITLE", ctx => ctx.VCard.Title = ctx.Value),
                new ActionPropertyHandler("ROLE", ctx => ctx.VCard.Role = ctx.Value),
                new ActionPropertyHandler("ORG", ctx => ctx.VCard.Organization = ctx.Value),
                new ActionPropertyHandler("URL", ctx => ctx.VCard.Url = ctx.Value),
                new ActionPropertyHandler("NOTE", ctx => ctx.VCard.Note = ctx.Value),
                new ActionPropertyHandler("MAILER", ctx => ctx.VCard.Mailer = ctx.Value),
                new ActionPropertyHandler("PROFILE", ctx => ctx.VCard.Profile = ctx.Value),
                new ActionPropertyHandler("XML", ctx => ctx.VCard.Xml = ctx.Value),
                new ActionPropertyHandler("SOURCE", ctx => ctx.VCard.Source = ctx.Value),
                new ActionPropertyHandler("NAME", ctx => ctx.VCard.Name = ctx.Value),
                new ActionPropertyHandler("KIND", ctx => ctx.VCard.Kind = ctx.Value),
                new ActionPropertyHandler("AGENT", ctx => ctx.VCard.Agent = ctx.Value),
                new ActionPropertyHandler("UID", ctx => ctx.VCard.Uid = ctx.Value),
                new ActionPropertyHandler("CLASS", ctx => ctx.VCard.Classification = ctx.Value),
                new KeyPropertyHandler(),
                new ActionPropertyHandler("BIRTHPLACE", ctx => ctx.VCard.Birthplace = VCardPropertyParsers.ParseTextValueProperty(ctx.Line, "BIRTHPLACE")),
                new DateTimePropertyHandler("DEATHDATE", (v, d) => v.DeathDate = d),
                new ActionPropertyHandler("DEATHPLACE", ctx => ctx.VCard.DeathPlace = ctx.Value),
                new LeveledPropertyHandler("EXPERTISE", (v, item) => v.Expertises.Add(item)),
                new LeveledPropertyHandler("HOBBY", (v, item) => v.Hobbies.Add(item)),
                new LeveledPropertyHandler("INTEREST", (v, item) => v.Interests.Add(item)),
                new ActionPropertyHandler("ORG-DIRECTORY", ctx => ctx.VCard.OrgDirectory = ctx.Value),
                new ActionPropertyHandler("PRODID", ctx => ctx.VCard.ProductIdentifier = ctx.Value),
                new ActionPropertyHandler("SORT-STRING", ctx => ctx.VCard.SortString = ctx.Value),
                new ClientPidMapPropertyHandler(),
                new ActionPropertyHandler("FBURL", ctx => ctx.VCard.FreeBusyUrl = ctx.Value),
                new ActionPropertyHandler("CALADRURI", ctx => ctx.VCard.CalendarAddressUri = ctx.Value),
                new ActionPropertyHandler("CALURI", ctx => ctx.VCard.CalendarUri = ctx.Value),
                new PostalLabelPropertyHandler(),
                new XExtensionPropertyHandler(),
                new OthersPropertyHandler(),
            };
        }
    }

    internal sealed class BeginEndPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName)
            => propertyName is "BEGIN" or "END";

        public void Apply(VCardParseContext context)
        {
        }
    }

    internal sealed class VersionPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "VERSION";

        public void Apply(VCardParseContext context)
        {
            context.VCard.Version = context.Value;
            context.Strategy = VCardVersionStrategyResolver.Resolve(context.Value);
        }
    }

    internal sealed class ActionPropertyHandler : IVCardPropertyHandler
    {
        private readonly string _propertyName;
        private readonly Action<VCardParseContext> _action;

        public ActionPropertyHandler(string propertyName, Action<VCardParseContext> action)
        {
            this._propertyName = propertyName;
            this._action = action;
        }

        public bool CanHandle(string propertyName) => propertyName == this._propertyName;

        public void Apply(VCardParseContext context) => this._action(context);
    }

    internal sealed class DateTimePropertyHandler : IVCardPropertyHandler
    {
        private readonly string _propertyName;
        private readonly Action<VCard, DateTime?> _setter;

        public DateTimePropertyHandler(string propertyName, Action<VCard, DateTime?> setter)
        {
            this._propertyName = propertyName;
            this._setter = setter;
        }

        public bool CanHandle(string propertyName) => propertyName == this._propertyName;

        public void Apply(VCardParseContext context)
            => this._setter(context.VCard, context.Strategy.ParseDateTime(context.Value));
    }

    internal sealed class CategoriesPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "CATEGORIES";

        public void Apply(VCardParseContext context)
            => context.VCard.Categories.AddRange(context.Value.Split(','));
    }

    internal sealed class GeoPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "GEO";

        public void Apply(VCardParseContext context)
            => context.VCard.Geo = context.Strategy.ParseGeoLocation(context.Value);
    }

    internal sealed class LanguagesPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "LANG";

        public void Apply(VCardParseContext context) => context.VCard.Languages.Add(context.Value);
    }

    internal sealed class AddressPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "ADR";

        public void Apply(VCardParseContext context)
            => context.VCard.Addresses.Add(VCardPropertyParsers.ParseAddress(context.Line));
    }

    internal sealed class TelephonePropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "TEL";

        public void Apply(VCardParseContext context)
            => context.VCard.PhoneNumbers.Add(context.Strategy.ParsePhoneNumber(context.PropertyPart, context.Value));
    }

    internal sealed class EmailPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "EMAIL";

        public void Apply(VCardParseContext context)
            => context.VCard.Emails.Add(VCardPropertyParsers.ParseEmail(context.Line));
    }

    internal sealed class RelatedPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "RELATED";

        public void Apply(VCardParseContext context)
            => context.VCard.Related.Add(VCardPropertyParsers.ParseRelatedPerson(context.Line));
    }

    internal sealed class ClientPidMapPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "CLIENTPIDMAP";

        public void Apply(VCardParseContext context)
            => context.VCard.ClientPidMaps.Add(VCardPropertyParsers.ParseClientPidMap(context.Line));
    }

    internal sealed class PostalLabelPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "LABEL";

        public void Apply(VCardParseContext context)
            => context.VCard.PostalLabels.Add(VCardPropertyParsers.ParsePostalLabel(context.Line));
    }

    internal sealed class KeyPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => propertyName == "KEY";

        public void Apply(VCardParseContext context)
            => context.VCard.Key = KeyPropertyParser.Parse(context.Line, context.NormalizedBlock);
    }

    internal sealed class LeveledPropertyHandler : IVCardPropertyHandler
    {
        private readonly string _propertyName;
        private readonly Action<VCard, LeveledText> _adder;

        public LeveledPropertyHandler(string propertyName, Action<VCard, LeveledText> adder)
        {
            this._propertyName = propertyName;
            this._adder = adder;
        }

        public bool CanHandle(string propertyName) => propertyName == this._propertyName;

        public void Apply(VCardParseContext context)
            => this._adder(context.VCard, VCardPropertyParsers.ParseLeveledProperty(context.Line, this._propertyName));
    }

    internal sealed class XExtensionPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName)
            => propertyName.StartsWith("X-", StringComparison.OrdinalIgnoreCase);

        public void Apply(VCardParseContext context)
        {
            context.VCard.Extensions[context.PropertyPart] = context.Line;

            for (var j = context.LineIndex + 1; j < context.Lines.Length; j++)
            {
                if (context.Lines[j].Contains(':'))
                {
                    break;
                }

                context.VCard.Extensions[$"{context.PropertyPart}{context.LineIndex}"] = context.Lines[j];
                context.LineIndex = j;
            }
        }
    }

    internal sealed class MediaPropertyHandler : IVCardPropertyHandler
    {
        private readonly string _propertyName;
        private readonly Action<VCard, string, string> _setter;

        public MediaPropertyHandler(string propertyName, Action<VCard, string, string> setter)
        {
            this._propertyName = propertyName;
            this._setter = setter;
        }

        public bool CanHandle(string propertyName) => propertyName == this._propertyName;

        public void Apply(VCardParseContext context)
            => this._setter(context.VCard, context.Line, context.NormalizedBlock);
    }

    internal sealed class OthersPropertyHandler : IVCardPropertyHandler
    {
        public bool CanHandle(string propertyName) => true;

        public void Apply(VCardParseContext context)
        {
            context.VCard.Others[context.PropertyPart] = context.Line;

            for (var j = context.LineIndex + 1; j < context.Lines.Length; j++)
            {
                if (context.Lines[j].Contains(':'))
                {
                    break;
                }

                context.VCard.Others[$"{context.PropertyPart}{context.LineIndex}"] = context.Lines[j];
                context.LineIndex = j;
            }
        }
    }
}
