using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DreamHouses.Model;
using DreamHouses.Webb.HttpApiClient;
using DreamHouses.Webb.Services;
using Xunit;

namespace DreamHouses.Test
{
    public class HousesApiClientTest : BaseApiTest
    {
        [Fact]
        public async Task PagingObjectTest()
        {
            var result = await GetClient().GetAllHousesAsync("");
            Assert.True(result.Paging.NumberOfPages >= 15);
            Assert.Equal(1, result.Paging.CurrentPage);
            Assert.Contains("/koop/amsterdam/p2/", result.Paging.NextUrl );

        }

        [Fact]
        public async Task HouseObjectsTest()
        {
            var result = await GetClient().GetAllHousesAsync("");
            Assert.True(result.Objects.Count() >= 15);
        }

        [Fact]
        public async Task RealEstateAgentObjectTypeTest()
        {
            var request = await GetClient().GetAllHousesAsync("");
            var realEstateAgentObject = request.Objects.FirstOrDefault();

            Assert.IsType<RealEstateAgent>(realEstateAgentObject);
        }

        [Fact]
             public async Task GetAllHouses()
        {
            var request = await GetClient().GetAll();
            Assert.True(request.Count() >= 2800 );
        }

        [Fact]
        public async Task GetAllHousesWithGarden()
        {
            var request = await GetClient().GetAll(true);

            Assert.True(request.Count() >= 500);
        }

        [Fact]
        public async Task GetNextPageTest()
        {
            var request = await GetClient().GetAllHousesAsync("p2");
            
            var result = request.Paging.NextUrl.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var nextPage = result[result.Length - 1];

            Assert.Equal("p3", nextPage);
        }
    }
}
