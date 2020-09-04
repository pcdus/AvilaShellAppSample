using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;
using Newtonsoft.Json;

namespace AvilaShellAppSample.Services
{
    public class ApiFacebookGraphService
    {
        string fbAccessToken = "EAAYmP2nRRLYBAN5SH92tfCiOKwYIBH1ZBMDBBWmAxDJy10MtslcCcfFx2uEZArBQpz1QtmJoxHI187lYrOzXf4KqtqAKrrIkUD9ZCdeS6HYNeObOV3rL9hCRjDWFi5yMRASkPB9Asd5cSZACDq31CFFvlMTsPPTvoohQiDw21CT2m8p3VqSEKCqGS6ZC8ngJiFKZASE5TgtwZDZD";

        public async Task<FacebookFeedDTO> GetFacebookNewsAsync()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                try
                {
                    //var json = await client.GetStringAsync("https://graph.facebook.com/v8.0/hourrapps/posts?access_token=" + fbAccessToken);
                    var json = await client.GetStringAsync("https://graph.facebook.com/v8.0/hourrapps/posts?fields=message%2Cmessage_tags%2Cpicture%2Cfull_picture%2Cpermalink_url%2Ccreated_time%2Cstory&access_token=" + fbAccessToken);
                    var res = JsonConvert.DeserializeObject<FacebookFeedDTO>(json);
                    return res;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
