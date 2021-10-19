using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services
{
    // This interface, takes
    public interface IFullTextSearchFoodTruck
    {
        // The List can be of type `FoodTruck` to make it more concrete and to avoid a Jump to the backing store
        // to retrieve the actual `FoodTruck` object. I implemnted this way to decouple the implemntation 
        public List<string> GetDocumentIds(List<string> terms, int maxItems = 10);

        // Helper function that if given a description, retrieves the constitent words and calls the indexing function
        public void AddFoodTruckDescrption(string foodTruckId, string description);
      
        public void RemoveFoodTruck(string foodTruck);
    }
}
