// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Domain.ValueObjects
{
    /// <summary>
    /// CLIENTPIDMAP-Eintrag für Synchronisation.
    /// </summary>
    public class ClientPidMap
    {
        /// <summary>
        /// Process Identifier.
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        /// URI der Client-Stelle.
        /// </summary>
        public string Uri { get; set; } = string.Empty;
    }
}
