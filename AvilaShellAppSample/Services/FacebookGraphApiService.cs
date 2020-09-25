using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;
using Newtonsoft.Json;

namespace AvilaShellAppSample.Services
{
    public class ApiFacebookGraphService
    {
        // generate permanent access token:
        // https://stackoverflow.com/questions/17197970/facebook-permanent-page-access-token-
        // user token
        //string fbAccessToken = "EAAYmP2nRRLYBAEBnTIxTMBNfA7owOB0uhUmRHX7QfO1bZAlBUBho0egmHs1jzMP01onbIWiT8ZCxzF0ZB1X8bmUFaYZALITWVn0oZBrsM3flOY9xfWZByu6Lxf4gV3BW9MT68YZBngkrZAt6rLAemiHvtK5LkuoaXjcH9qUvaLsLMQZDZD";
        // page token
        string fbAccessToken = "EAAYmP2nRRLYBACExrYukSsuBmSSOKY7NPxEm2bDkcZA7uomK5yobXPZCLPbixfYK87tT8AoAMbmdd3KUvTT5Ffew6jlsMFUYn31tgHhT3VrLDztHPUbPa1x45ynLwBIlHjPMofQ8ZB0RQN4bImXx9QWJoJ3karG4xeK7jJJ1gZDZD";

        //string url = "https://graph.facebook.com/v8.0/coiffeursindependants/posts?__activeScenarioIDs=%5B%5D&__activeScenarios=%5B%5D&debug=all&fields=id%2Cparent_id%2Cmessage%2Ccreated_time%2Cpicture%2Cicon%2Cpermalink_url%2Cfull_picture%2Cevent&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors"
        public async Task<FacebookFeedDTO> GetFacebookNewsAsync()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                try
                {
                    //var json = await client.GetStringAsync("https://graph.facebook.com/v8.0/hourrapps/posts?access_token=" + fbAccessToken);
                    var json = await client.GetStringAsync("https://graph.facebook.com/v8.0/coiffeursindependants/posts?fields=message%2Cmessage_tags%2Cpicture%2Cfull_picture%2Cpermalink_url%2Ccreated_time%2Cstory&access_token=" + fbAccessToken);
                    //var url = "https://graph.facebook.com/v8.0/coiffeursindependants/posts?access_token=EAAYmP2nRRLYBANUXJbBfLTNGRqnVEpXh58kqHvwxPvg3pZAnfxuyIvrUaj8oOCiWD8d9ZBuo35dg8RWOsLYM4I9jeRZBrgyDYtJr2eFauZCNnkjP0VyQXUvD5KttsLqYTaFIrsQ8XwN9ZBwMOe1qzUbHYRu6NmLIyEwRmQPCZAo0XD4ypXWxJXssmP2P9QumIZD&debug=all&fields=message%2Cmessage_tags%2Cpicture%2Cfull_picture%2Cpermalink_url%2Ccreated_time%2Cstory&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors";
                    //var url = "https://graph.facebook.com/v8.0/coiffeursindependants/posts?access_token=EAAYmP2nRRLYBAOaMklMn7z1HwjdQeUXa8pQgf3TJEuKgQ8wbuAqIFAVC3OR5e9MWLeycZCgiFIBj35BJvW6gtccTTYQrVtkccpwEZCFWvtVxsffCWdYP2vS8NhYnqIjRLJ5Xoo6GGF3nez40s528S4pwTttNwpawBgKi2YZBHHxZABVGB5wbi1exbEPtpm4ZD&debug=all&fields=message%2Cmessage_tags%2Cpicture%2Cfull_picture%2Cpermalink_url%2Ccreated_time%2Cstory&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors";

                    //var json = await client.GetStringAsync(url);
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
