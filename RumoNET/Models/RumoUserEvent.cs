using System;

namespace RumoNET.Models
{
    public class RumoUserEvent
    {
        /// <summary>
        /// String map with the identification of the source and the userId.
        /// </summary>
        public RumoKey key { get; set; }

        /// <summary>
        /// String identifying the $interactionType, which must be one of Click, Play, Watch, Bookmark, Preview, Purchase, RateLike, or RateDislike.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// String used to identify the content piece.
        /// </summary>
        public string contentId { get; set; }

        /// <summary>
        /// String identifying the timestamp.
        /// </summary>
        public DateTime timestamp { get; set; }
    }
}
