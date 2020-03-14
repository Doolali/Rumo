using Rumo.Enums;
using System;

namespace Rumo.Models
{
    public class RumoInteraction
    {
        public RumoKey key { get; set; }
        
        public RumoInteractionType type { get; set; }
        
        public string contentId { get; set; }

        public DateTime timestamp { get; set; }
    }
}
