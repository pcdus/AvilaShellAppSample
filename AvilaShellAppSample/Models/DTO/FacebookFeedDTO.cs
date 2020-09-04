using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AvilaShellAppSample.Models
{
    public class FacebookFeedDTO
    {
        /*
        public long id { get; set; }
        public DateTime created_time { get; set; }
        public string message { get; set; }
        public string story { get; set; }
        */

        /*
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }

        public class Datum
        {
            public DateTime created_time { get; set; }
            public string message { get; set; }
            public string id { get; set; }
            public string story { get; set; }
        }

        public class Cursors
        {
            public string before { get; set; }
            public string after { get; set; }
        }

        public class Paging
        {
            public Cursors cursors { get; set; }
        }
        */

        [JsonProperty("data")]
        public List<Datum> Data;

        [JsonProperty("paging")]
        public Paging paging;

        public class Datum
        {
            [JsonProperty("message")]
            public string Message;

            [JsonProperty("picture")]
            public string Picture;

            [JsonProperty("full_picture")]
            public string FullPicture;

            [JsonProperty("permalink_url")]
            public string PermalinkUrl;

            [JsonProperty("created_time")]
            public DateTime CreatedTime;

            [JsonProperty("id")]
            public string Id;

            [JsonProperty("story")]
            public string Story;
        }

        public class Cursors
        {
            [JsonProperty("before")]
            public string Before;

            [JsonProperty("after")]
            public string After;
        }

        public class Paging
        {
            [JsonProperty("cursors")]
            public Cursors Cursors;
        }

    }
}
