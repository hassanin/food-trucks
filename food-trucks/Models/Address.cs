using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Models
{
    //TODO: NOT USED IN PROJECT: We can represent the Address as strongly typed address instead of a string
    //Or perhaps use a 3rd library implemntation

    /// <summary>
    /// This is a very rudimentary Address Object, it is heavily biased towards US/Canada based addresses and there may be other represnattion more 
    /// applicable to international addresses
    /// </summary>
    
    public class Address
    {
        // Stores the RawAddress provided in the constructor
        public string RawAddress { get; init; }
        public string Street { get; init; }
        public string Number { get; init; }
        
        public string City { get; init; }

        public string State { get; init; }
        public string Country { get; init; }
        public string ZipCode { get; init; }

    }
}
