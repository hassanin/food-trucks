using food_trucks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services
{
    public interface IFoodTruckInitailizer
    {
        List<FoodTruck> GetFoodTrucks();
    }
}
