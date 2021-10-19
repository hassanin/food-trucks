using food_trucks.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services.Implemntation
{
    public class CSVTruckInatilizer : IFoodTruckInitailizer
    {
        private ILogger<CSVTruckInatilizer> logger;
        private List<FoodTruck> foodTrucks;
        private string FileName { get; init; }
        public CSVTruckInatilizer(ILogger<CSVTruckInatilizer> logger, string fileName)
        {
            this.FileName = fileName;
            this.logger = logger;
            logger.LogInformation("Inatilizing CSV file");
            InitFoodTrucks();
        }
        public List<FoodTruck> GetFoodTrucks()
        {
            return foodTrucks;
           
        }
        private void InitFoodTrucks()
        {
            logger.LogInformation("Begiggning to intalize food truck from CSV file");
            this.foodTrucks = Util.GetInitalFoodTrucks.ReadFromCSV(FileName);
            logger.LogInformation("Initailzied successfully");
        }
    }
}
