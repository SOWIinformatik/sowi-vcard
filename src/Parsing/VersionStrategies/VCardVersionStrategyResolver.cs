// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Parsing.VersionStrategies
{
    /// <summary>
    /// Liefert die passende Version-Strategy für eine vCard-Version.
    /// </summary>
    internal static class VCardVersionStrategyResolver
    {
        private static readonly IVCardVersionStrategy Strategy21 = new VCard21VersionStrategy();
        private static readonly IVCardVersionStrategy Strategy30 = new VCard30VersionStrategy();
        private static readonly IVCardVersionStrategy Strategy40 = new VCard40VersionStrategy();

        internal static IVCardVersionStrategy Resolve(string? version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return Strategy30;
            }

            return version.Trim() switch
            {
                "2.1" => Strategy21,
                "4.0" => Strategy40,
                _ => Strategy30,
            };
        }
    }
}
