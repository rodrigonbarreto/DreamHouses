using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using DreamHouses.Model;
using DreamHouses.Webb.HttpApiClient.interfaces;

namespace DreamHouses.Webb.HttpApiClient
{
    public class HousesApiClient : IHousesApiClient
    {
        public const int TimeThreadSleep = 3000;
        public const int TimeCatchApiThreadSleep = 50000;
        public const int NumberOfRequests = 20;

        private readonly HttpClient _httpClient;

        public HousesApiClient(HttpClient client)
        {
            _httpClient = client;
        }

        // If it fails and falls into the Catch by of the X number of  Requests make the application wait.
        // The wait time is X defined on the Const and if you continue with the error will throw an HttpRequestException message error,
        // in that case, you should contact the owner of the external API to setup Right time for the wait time.
        public async Task<RealEstateAgentApiResultMapper> GetAllHousesAsync(string queryString, bool hasGarden = false)
        {
            var gardenQueryString = "";
            if (hasGarden)
            {
                gardenQueryString = "tuin/";
            }
            var urlParam = $"?type=koop&zo=/amsterdam/{gardenQueryString}{queryString}";
            var result = await _httpClient.GetAsync(urlParam);
            try
            {
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsAsync<RealEstateAgentApiResultMapper>();
            }
            catch (HttpRequestException e)
            {
                Thread.Sleep(TimeCatchApiThreadSleep);
                result = await _httpClient.GetAsync(urlParam);
                if (result.IsSuccessStatusCode == false) throw new System.ArgumentException(e.Message); ;
                return await result.Content.ReadAsAsync<RealEstateAgentApiResultMapper>(); ;
            }
        }

        public async Task<List<RealEstateAgent>> GetAll(bool hasGarden = false)
        {
            var listOfRealEstateAgent = new List<RealEstateAgent>();
            var i = 0;
            var realEstateAgentJsonMapper = await GetAllHousesAsync("");

            for (var j = 1; j < (realEstateAgentJsonMapper.Paging.NumberOfPages); j++)
            {
                i++;
                var page = GetNextPage(realEstateAgentJsonMapper);
                realEstateAgentJsonMapper = await this.GetAllHousesAsync(page, hasGarden);
                if (i >= NumberOfRequests)
                {
                    i = 0;
                    Thread.Sleep(TimeThreadSleep);
                }
               
                if (realEstateAgentJsonMapper == null)
                {
                    return listOfRealEstateAgent;
                }
                listOfRealEstateAgent.AddRange(realEstateAgentJsonMapper.Objects);
            }

            return listOfRealEstateAgent;
        }

        private static string GetNextPage(RealEstateAgentApiResultMapper realEstateAgentJsonMapper)
        {
            var result = realEstateAgentJsonMapper.Paging.NextUrl.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return result[result.Length - 1];
        }
    }
}
