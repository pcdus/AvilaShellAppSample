using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AvilaShellAppSample.Models
{
    public class FacebookEventDTO
    {
        [JsonProperty("data")]
        public List<Event> Events;

        [JsonProperty("paging")]
        public FPaging Paging;

        public class Event
        {
            [JsonProperty("description")]
            public string Description;

            [JsonProperty("start_time")]
            public DateTime StartTime;

            [JsonProperty("end_time")]
            public DateTime EndTime;

            [JsonProperty("id")]
            public string Id;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("place")]
            public Place Place;
        }

        public class Place
        {
            [JsonProperty("name")]
            public string Name;

            [JsonProperty("id")]
            public string Id;

            [JsonProperty("location")]
            public Location Location;
        }

        public class Location
        {
            [JsonProperty("city")]
            public string City;

            [JsonProperty("country")]
            public string Country;

            [JsonProperty("latitude")]
            public double Latitude;

            [JsonProperty("longitude")]
            public double Longitude;

            [JsonProperty("street")]
            public string Street;

            [JsonProperty("zip")]
            public string Zip;
        }

        public class Cursors
        {
            [JsonProperty("before")]
            public string Before;

            [JsonProperty("after")]
            public string After;
        }

        public class FPaging
        {
            [JsonProperty("cursors")]
            public Cursors Cursors;
        }
    }
}
