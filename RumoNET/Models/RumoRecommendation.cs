using System.Collections.Generic;

namespace RumoNET.Models
{
    public class RumoRecommendation
    {
        public string id { get; set; }

        public List<RumoSuggestion> content { get; set; }
    }
}
