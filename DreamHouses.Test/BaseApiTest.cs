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
    public class BaseApiTest
    {
        public HousesApiClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress =
                    new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f")
            };
            return new HousesApiClient(client); ;
        }
    }
}
