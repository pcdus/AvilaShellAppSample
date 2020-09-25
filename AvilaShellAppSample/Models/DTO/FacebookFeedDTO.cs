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
        public FPaging Paging;

        public class Post
        {
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("created_time")]
            public DateTime CreatedTime;

            [JsonProperty("message")]
            public string Message;

            [JsonProperty("picture")]
            public string Picture;

            [JsonProperty("full_picture")]
            public string FullPicture;

            [JsonProperty("permalink_url")]
            public string PermalinkUrl;

            [JsonProperty("icon")]
            public string Icon;

            [JsonProperty("story")]
            public string Story;

            [JsonProperty("story_tags")]
            public List<StoryTag> StoryTags;
        }

        public class StoryTag
        {
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("type")]
            public string Type;

            [JsonProperty("offset")]
            public int Offset;

            [JsonProperty("length")]
            public int Length;
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
