using food_trucks.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks.Services.Dummy
{
    public class BasicGeoDecoder : IGeoDecoder
    {
        private ILogger<BasicGeoDecoder> logger;
        public BasicGeoDecoder(ILogger<BasicGeoDecoder> logger)
        {
            this.logger = logger;
        }
        public GeoLocation GetLocationFromAddress(string address)
        {
            logger.LogError("Method Not Implemented, Dummy Version only");
            throw new NotImplementedException();
        }
    }
}
