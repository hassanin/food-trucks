using food_trucks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services
{
    /// <summary>
    /// Interface to convert an address to a GeoLocation
    /// </summary>
    interface IGeoDecoder
    {
        GeoLocation GetLocationFromAddress(String address);

        // reverse GeoDecoder is optional
        /// <summary>
        /// It is the responsibilty of the caller to check for null in case the API is not implemneted or failed to parse
        /// The implemnter of this API may also throw exceptions realted to faiure to obtain an address, It is 
        /// importatnt to read the documentation of the implemntation as well to see how to handle the various exceptions if desired
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        string? GetAddressFromGeoLocation(GeoLocation location) {
            return null;
        }
    }
}
