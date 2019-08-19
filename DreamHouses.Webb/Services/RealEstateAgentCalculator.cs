using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamHouses.Model;

namespace DreamHouses.Webb.Services
{
    public static class RealEstateAgentCalculator
    {
        public const int NumberOfRealEstateAgent = 10;

        public static IEnumerable<RealEstateAgentCalculationResultMapper> RealEstateAgentCalculationResultMappers(List<RealEstateAgent> realStateAgentList)
        {
            var query = realStateAgentList.GroupBy(info => info.Name)
                .Select(group => new RealEstateAgentCalculationResultMapper
                {
                    Name = @group.Key,
                    Total = @group.Count()
                })
                .OrderByDescending(x => x.Total).Take(NumberOfRealEstateAgent);
            return query.ToList();
        }
    }
}
