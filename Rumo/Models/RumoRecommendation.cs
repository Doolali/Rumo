using System.Collections.Generic;

namespace Rumo.Models
{
    public class RumoRecommendation
    {
        public string id { get; set; }

        public List<RumoSuggestion> content { get; set; }
    }
}
