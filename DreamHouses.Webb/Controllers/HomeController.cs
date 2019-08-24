using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamHouses.Model;
using Microsoft.AspNetCore.Mvc;
using DreamHouses.Webb.Models;
using Microsoft.Extensions.Caching.Memory;
using DreamHouses.Webb.HttpApiClient;
using DreamHouses.Webb.Services;

namespace DreamHouses.Webb.Controllers
{
    public class HomeController : Controller
    {
        public const int TimeCacheForRealStateAgentList = 300;

        public HomeController(
            HousesApiClient api, 
            IMemoryCache memoryCache, 
            IList<RealEstateAgentCalculationResultMapper> realEstateAgentCalculationResultMappers,
            List<RealEstateAgent> realStateAgentList
            )
        {
            _api = api;
            _cache = memoryCache;
            _realStateAgentList = realStateAgentList;
            _realEstateAgentCalculationResultMappers = realEstateAgentCalculationResultMappers;
        }

        private readonly HousesApiClient _api;
        private readonly IMemoryCache _cache;
        private List<RealEstateAgent> _realStateAgentList;
        private IEnumerable<RealEstateAgentCalculationResultMapper> _realEstateAgentCalculationResultMappers;

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Houses()
        {

            _realStateAgentList = await GetAllRealStateAgents(_realStateAgentList, "Houses");
            _realEstateAgentCalculationResultMappers = RealEstateAgentCalculator.RealEstateAgentCalculationResultMappers(_realStateAgentList);

            var model = new RealEStateAgentViewModel
            {
                RealEstateAgentCalculation = _realEstateAgentCalculationResultMappers.ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> HousesWithGarden()
        {
            _realStateAgentList = await GetAllRealStateAgents(_realStateAgentList, "HousesWithGarden", true);
            _realEstateAgentCalculationResultMappers = RealEstateAgentCalculator.RealEstateAgentCalculationResultMappers(_realStateAgentList);

            var model = new RealEStateAgentViewModel
            {
                RealEstateAgentCalculation = _realEstateAgentCalculationResultMappers.ToList()
            };
            return View(model);
        }

        public async Task<List<RealEstateAgent>> GetAllRealStateAgents(List<RealEstateAgent> realStateAgentList,string cacheListName, bool hasGarden = false)
        {
            if (realStateAgentList == null) throw new ArgumentNullException(nameof(realStateAgentList));

            if (_cache.TryGetValue(cacheListName, out realStateAgentList)) return realStateAgentList;
            if (realStateAgentList == null || realStateAgentList.Count <= 0)
            {
                realStateAgentList = await _api.GetAll(hasGarden);
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(TimeCacheForRealStateAgentList));

            _cache.Set(cacheListName, realStateAgentList, cacheEntryOptions);

            return realStateAgentList;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
