using food_trucks.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services.Implemntation
{
    public class MemoryBacked : IFoodTruckContext
    {
        private ILogger<MemoryBacked> logger;
        // backing store for the food trucks
        private List<FoodTruck> foodTrucks;
        private IFullTextSearchFoodTruck FullTextSearchFoodTruck { get; init; }
        // main index for the loation ID
        private IDictionary<string, FoodTruck> mainIndex = new Dictionary<string, FoodTruck>();
        private Object lockObject = new object();

        public MemoryBacked(ILogger<MemoryBacked> logger,IFoodTruckInitailizer initailizer, IFullTextSearchFoodTruck fullTextSearchFoodTruck)
        {
            this.logger = logger;
            foodTrucks = initailizer.GetFoodTrucks();
            // initalize the index
            foodTrucks.ForEach(truck => mainIndex.Add(truck.LocationId, truck));

            // initailize the full text search cache
            FullTextSearchFoodTruck = fullTextSearchFoodTruck;

            foodTrucks.ForEach(truck => FullTextSearchFoodTruck.AddFoodTruckDescrption(truck.LocationId, truck.FoodItems));
        }
        /// <summary>
        /// Adds or Updates a record completely, similar to upsert, but not quite an upsert operation
        /// </summary>
        /// <param name="foodTruck"></param>
        /// <returns> true if the record was s </returns>
        public void AddOrUpdateFoodTruck(FoodTruck foodTruck)
        {
            lock (lockObject)
            {
                if (mainIndex.ContainsKey(foodTruck.LocationId))
                {
                    RemoveFoodTruck(foodTruck);
                }
                mainIndex.Add(foodTruck.LocationId, foodTruck);
                // Note we only index the desciption of the food trucks items, we can also index the address, special tags, anything that the user
                // may be intersted in finding quickly
                FullTextSearchFoodTruck.AddFoodTruckDescrption(foodTruck.LocationId, foodTruck.FoodItems);
                foodTrucks.Add(foodTruck);
            }
        }

        /// <summary>
        /// Not guarnteed to return any result if searched for terms do not found in the foodTrucks data
        /// </summary>
        /// <param name="geoLocation"></param>
        /// <param name="searchTerms"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<FoodTruck> FindTrucks(GeoLocation geoLocation, List<string> searchTerms, int items = 5)
        {
            return FullTextSearchFoodTruck. 
                GetDocumentIds(searchTerms). // Retrieve the list of documents matching the searchTerms
                AsEnumerable()               // Convert to strea,
               .Select(truckID => mainIndex[truckID])    // retreived the correpdoning Truck object from the main backing store
               .Select(truck => new { distance = truck.GeoLocation.Distance(geoLocation), truck = truck }) // for each truck, compute the distance from the current location
               .OrderBy(elem => elem.distance) // order by the smallest distance
               .Take(items) // take only the required items
               .Select(elem => elem.truck) // select the truck itself, (prokect away the distance)
               .ToList(); 
           

        }
        // Retrieves the closest food trucks to the user, guarnteed to return exactly 5 items or more (up to the total number of carts in SF) or less if the caller so desires
        /// <summary>
        /// Guarnteed to return exactly n items based on the provided input param (up to the max number of trucks in SF)
        /// </summary>
        /// <param name="geoLocation"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<FoodTruck> FindTrucks(GeoLocation geoLocation, int items = 5)
        {
            return foodTrucks
               .Select(truck => new { distance = truck.GeoLocation.Distance(geoLocation), truck = truck }) // for each truck, compute the distance from the current location
               .OrderBy(elem => elem.distance) // order by the smallest distance
               .Take(items) // take only the required items
               .Select(elem => elem.truck) // select the truck itself, (prokect away the distance)
               .ToList();
        }

        //TODO: In the additional information we can implement a Query language where we pass the query, or we can use
        // LINQ and the query can be the FoodTruck object itself, similarly we can use a continuation token here when we are
        // retrieving data that is much bigger to fit in one Round Trip
        public DataTruckResultObject GetFoodTrucks(object additionalInformation)
        {
            var response = new DataTruckResultObject {auxillaryInformation=null,foodTrucks=foodTrucks };
            return response;
        }

        // Removes by referebce to the actual FoodTruck object
        public bool RemoveFoodTruck(FoodTruck foodTruck)
        {
            return RemoveFoodTruck(foodTruck.LocationId);
        }

        // Removes by ID
        public bool RemoveFoodTruck(string locationID)
        {
            if (mainIndex.ContainsKey(locationID))
            {
                var truckToBeRemoved = mainIndex[locationID];
                foodTrucks.Remove(truckToBeRemoved);
                mainIndex.Remove(locationID);
                FullTextSearchFoodTruck.RemoveFoodTruck(locationID);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
