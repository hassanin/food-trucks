using System;
using food_trucks.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services
{
    public interface IFoodTruckContext
    {
        void AddOrUpdateFoodTruck(FoodTruck foodTruck);
        bool RemoveFoodTruck(FoodTruck foodTruck);

        bool RemoveFoodTruck(string locationID);

        DataTruckResultObject GetFoodTrucks(object? additionalInformation);

        List<FoodTruck> FindTrucks(GeoLocation geoLocation, List<string> searchTerms, int maxItems = 5);

        List<FoodTruck> FindTrucks(GeoLocation geoLocation, int maxItems = 5);


    }

    // An object that isresponsible for transferring the foodTrucks back to the caller
    // As well as any other extre information that may be useful, for example continuation tokens or pagination info
    // in case the response needs  to be broken down into several peices
    public record DataTruckResultObject
    {
        public List<FoodTruck> foodTrucks;
        public object? auxillaryInformation;
       
    }
}
