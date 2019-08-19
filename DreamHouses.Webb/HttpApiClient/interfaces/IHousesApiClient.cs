using System.Threading.Tasks;
using DreamHouses.Model;

namespace DreamHouses.Webb.HttpApiClient.interfaces
{
    public interface IHousesApiClient
    {
        Task<RealEstateAgentApiResultMapper> GetAllHousesAsync(string queryString, bool hasGarden = false);
    }
}