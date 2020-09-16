using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AvilaShellAppSample.Models
{
    public class FacebookFeedDTO
    {
        [JsonProperty("data")]
        public List<Post> Posts;

        [JsonProperty("paging")]
        public Paging paging;

        public class Post
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
