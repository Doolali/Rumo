using System.Collections.Generic;

namespace Rumo.Models
{
    public class RumoContent
    {
        public string id { get; set; }

        public string label { get; set; }

        public Dictionary<string, List<RumoCategory>> categories { get; set; }

        public RumoFilters filters { get; set; }
    }
}
