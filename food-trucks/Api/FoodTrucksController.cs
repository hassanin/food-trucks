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

       [HttpGet("all")]
       public async Task<List<FoodTruck>> AllTrucks()
        {
            logger.LogInformation("Received All food trucks request");
            return context.GetFoodTrucks(null).foodTrucks;
            //return new string[] { "Love","Hop"};
        }

        [HttpPost("location")]
        public async Task<List<FoodTruck>> GetTrucks(GeoLocation location, int maxItems=5)
        {
            logger.LogInformation("Received Restaruatn request");
            return context.FindTrucks(location, maxItems);
            //return context.GetFoodTrucks(null).foodTrucks.Select(elem => elem.ToString()).ToArray();
            //return new string[] { "Love","Hop"};
        }

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
