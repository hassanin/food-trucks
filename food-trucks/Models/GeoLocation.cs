using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonConstructorAttribute = System.Text.Json.Serialization.JsonConstructorAttribute;

namespace food_trucks.Models
{
    // A class that represents the GeoLocation {Longitiude, Latitude},
    // For exzmaple {-110, 25.1223}
    public class GeoLocation 
    {
        public static double MAX_LATITUDE = 90; // +- 90 degrees maximum latitiude
        public static double MAX_LONGTITIDE = 180; // +- 180 degrees maximum latitiude
        [JsonProperty]
        public double Longtitude { get; init; }
        [JsonProperty]
        public double Latitiude { get; init; }

        /// <summary>
        /// thhrows `FormatException` if inputs are not valid 
        /// </summary>
        /// <param name="longtitude"></param>
        /// <param name="latitiude"></param>        
        public GeoLocation(string longtitude, string latitiude)
        {

            Latitiude = double.Parse( latitiude);
            Longtitude = double.Parse(longtitude);
            if(Math.Abs(Latitiude) > MAX_LATITUDE || Math.Abs(Longtitude) > MAX_LONGTITIDE)
            {
                throw new FormatException("Expected input to be a valid langitiude and latitude and to lie inside the appropriate ranges");
            }
        }        [JsonConstructorAttribute]        public GeoLocation(double longtitude, double latitiude)
        {
            Latitiude = latitiude;
            Longtitude = longtitude;
            if (Math.Abs(Latitiude) > MAX_LATITUDE || Math.Abs(Longtitude) > MAX_LONGTITIDE)
            {
                throw new FormatException("Expected input to be a valid langitiude and latitude and to lie inside the appropriate ranges");
            }
           
        }
       
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static double Distance( GeoLocation point1, GeoLocation point2)
        {
            var xDiff = point1.Latitiude - point2.Latitiude;
            var yDiff = point1.Longtitude - point2.Longtitude;
            return Math.Sqrt(xDiff * xDiff + yDiff + yDiff);
        }

        public double Distance(GeoLocation other)
        {
            return Distance(this, other);
        }
    }
}
