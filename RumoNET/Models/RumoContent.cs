using System.Collections.Generic;

namespace RumoNET.Models
{
    public class RumoContent
    {
        public string id { get; set; }

        public string label { get; set; }

        public Dictionary<string, List<RumoCategory>> categories { get; set; }

        public RumoFilters filters { get; set; }
    }
}
