using CsvHelper;
using food_trucks.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
//using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services.Util
{
    public class GetInitalFoodTrucks
    {
        public static List<FoodTruck> ReadFromCSV(string fileName)
        {
            using (var reader = new StreamReader(fileName)) 
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<FoodTruckCSV>();
                return records
                    .Select(foodTruck =>foodTruck.GetFoodTruck()) // convert the DTO back to the represntaive data type
                    .ToList();
            }
        }

        // Data Transfer Object for CSV
        private class FoodTruckCSV
        {
            // Unique Identifier for the location (Sort of a Primary Key)
            public string locationid { get; set; }
            public string Applicant { get; set; }
            //public FacilityType FacilityType { get; init; }
            public string Address { get; set; }
            public string FoodItems { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string dayshours { get; init; }
            
            // Converts the Data Transfer Object back to a real FoodTruck
            public FoodTruck GetFoodTruck()
            {
                return new FoodTruck(locationid, Applicant, Address, FoodItems, new GeoLocation(Longitude, Latitude),dayshours);
               
            }
        }
        
       
    }
}
