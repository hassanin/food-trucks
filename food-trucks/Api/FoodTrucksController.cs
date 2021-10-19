using food_trucks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using food_trucks.Models;

namespace food_trucks.Controllers
{
    [ApiController]
    [Route("/api/food-trucks")]
    public class FoodTrucksController : ControllerBase
    {
        private ILogger logger;
        private IFoodTruckContext context;

      /// <summary>
      /// Gets All Food trucks in the SF Area
      /// </summary>
      /// <returns></returns>
        [HttpGet("all")]
       public async Task<List<FoodTruck>> AllTrucks()
        {
            logger.LogInformation("Received All food trucks request");
            return context.GetFoodTrucks(null).foodTrucks;
            //return new string[] { "Love","Hop"};
        }

        /// <summary>
        /// Gets the closest food truck to the desired location
        /// </summary>
        /// <param name="location"> current location as geo location</param>
        /// <param name="maxItems"> max numbers of trucks to return in the response</param>
        /// <returns></returns>
        [HttpPost("location")]
        public async Task<List<FoodTruck>> GetTrucks(GeoLocation location, int maxItems=5)
        {
            logger.LogInformation("Received Restaruatn request");
            return context.FindTrucks(location, maxItems);
            //return context.GetFoodTrucks(null).foodTrucks.Select(elem => elem.ToString()).ToArray();
            //return new string[] { "Love","Hop"};
        }
        /// <summary>
        /// Searches for a truck using a search term, for example noodles and return the closest to the 
        /// provided location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="searchTerm"></param>
        /// <param name="maxItems"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<List<FoodTruck>> SearchTrucks(GeoLocation location, string searchTerm, int maxItems = 5)
        {
            var tokens = searchTerm.Split(' ');
            logger.LogInformation("Received Restaruatn request");
            return context.FindTrucks(location, tokens.ToList(), maxItems);
            //return context.GetFoodTrucks(null).foodTrucks.Select(elem => elem.ToString()).ToArray();
            //return new string[] { "Love","Hop"};
        }
        public FoodTrucksController(ILogger<FoodTrucksController> logger, IFoodTruckContext context)
        {
            this.logger = logger;
            this.context = context;
            logger.LogInformation("Constructed Food Trucks controller");
        }
    }
}
