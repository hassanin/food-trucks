using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace food_trucks.Models
{
    public class FoodTruck
    {
        // Unique Identifier for the location (Sort of a Primary Key)
        public string LocationId { get; init; }
        public string Applicant { get; init; }
        //public FacilityType FacilityType { get; init; }
        public string Address{ get; init; }
        public string FoodItems { get; init; }
        public GeoLocation GeoLocation { get; init; }

        //TODO: We can make a strongly typed class (Range) that captures whether the truck is open or close
        // Did not have time to do it
        public string HoursOfOperation { get; init; }
        [JsonConstructorAttribute]
        public FoodTruck(string locationID, string applicant, string address, string foodItems, GeoLocation geoLocation, string hoursOfOperation)
        {
            LocationId = locationID;
            Address = address;
            Applicant = applicant;
            FoodItems = foodItems;
            GeoLocation = geoLocation;
            HoursOfOperation = hoursOfOperation;
        }
        // For better readabilty, Note we could have used the record type instead of class here
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    //public enum FacilityType
    //{
    //    Truck,
    //    PushCart,
    //    Other
    //}
}
