using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DreamHouses.Model;
using DreamHouses.Webb.HttpApiClient;
using DreamHouses.Webb.Services;
using Xunit;

namespace DreamHouses.Test
{
   public class RealEstateAgentCalculatorTest : BaseApiTest
    {

        [Fact]
        public async Task RealEstateAgentCalculationResultMappersTest()
        {
            var result = await GetClient().GetAll(true);
            var realEstateAgentCalculationResultMappers = RealEstateAgentCalculator.RealEstateAgentCalculationResultMappers(result);
            Assert.True(realEstateAgentCalculationResultMappers.Count() >= RealEstateAgentCalculator.NumberOfRealEstateAgent);
        }
    }
}
