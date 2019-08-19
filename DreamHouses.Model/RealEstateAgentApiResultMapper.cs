using System.Collections.Generic;

namespace DreamHouses.Model
{
    public class RealEstateAgentApiResultMapper
    {
        public string AccountStatus { get; set; }
        public IEnumerable<RealEstateAgent> Objects { get; set; }
        public Paging Paging { get; set; }
    }
}
