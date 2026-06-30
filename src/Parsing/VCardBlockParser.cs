// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Parsing.PropertyHandlers;
using SOWI.vCard.Parsing.VersionStrategies;

namespace SOWI.vCard.Parsing
{
    /// <summary>
    /// Parst einen normalisierten vCard-Block in ein <see cref="VCard"/>-Aggregate.
    /// </summary>
    internal static class VCardBlockParser
    {
        private static readonly IReadOnlyList<IVCardPropertyHandler> Handlers = VCardPropertyHandlerRegistry.CreateDefault();
        private static readonly IVCardPropertyHandler FallbackHandler = Handlers[^1];

        internal static VCard Parse(string normalizedBlock)
        {
            var version = VCardPropertyLineParser.DetectVersion(normalizedBlock);
            var strategy = VCardVersionStrategyResolver.Resolve(version);
            var vCard = new VCard();
            var lines = normalizedBlock.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!VCardPropertyLineParser.TrySplit(line, out var propertyPart, out var value))
                {
                    continue;
                }

                var propertyName = VCardPropertyLineParser.GetPropertyName(propertyPart);
                var context = new VCardParseContext
                {
                    VCard = vCard,
                    Strategy = strategy,
                    NormalizedBlock = normalizedBlock,
                    Lines = lines,
                    LineIndex = i,
                    Line = line,
                    PropertyPart = propertyPart,
                    Value = value,
                    PropertyName = propertyName,
                };

                var handler = Handlers.FirstOrDefault(h => h != FallbackHandler && h.CanHandle(propertyName))
                    ?? FallbackHandler;
                handler.Apply(context);
                strategy = context.Strategy;
                i = context.LineIndex;
            }

            return vCard;
        }
    }
}
