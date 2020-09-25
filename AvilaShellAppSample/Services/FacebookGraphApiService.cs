using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;
using Newtonsoft.Json;

namespace AvilaShellAppSample.Services
{
    public class FacebookGraphApiService : IFacebookGraphApiService
    {
        // generate permanent access token:
        // https://stackoverflow.com/questions/17197970/facebook-permanent-page-access-token-
        // user token
        //string fbAccessToken = "EAAYmP2nRRLYBAEBnTIxTMBNfA7owOB0uhUmRHX7QfO1bZAlBUBho0egmHs1jzMP01onbIWiT8ZCxzF0ZB1X8bmUFaYZALITWVn0oZBrsM3flOY9xfWZByu6Lxf4gV3BW9MT68YZBngkrZAt6rLAemiHvtK5LkuoaXjcH9qUvaLsLMQZDZD";
        // page token
        string fbAccessToken = "EAAYmP2nRRLYBACExrYukSsuBmSSOKY7NPxEm2bDkcZA7uomK5yobXPZCLPbixfYK87tT8AoAMbmdd3KUvTT5Ffew6jlsMFUYn31tgHhT3VrLDztHPUbPa1x45ynLwBIlHjPMofQ8ZB0RQN4bImXx9QWJoJ3karG4xeK7jJJ1gZDZD";
        string baseUrl = "https://graph.facebook.com/v8.0/coiffeursindependants/";
                                                                                                                                                                                                         

        // https://graph.facebook.com/v8.0/coiffeursindependants/events?access_token=EAAYmP2nRRLYBACExrYukSsuBmSSOKY7NPxEm2bDkcZA7uomK5yobXPZCLPbixfYK87tT8AoAMbmdd3KUvTT5Ffew6jlsMFUYn31tgHhT3VrLDztHPUbPa1x45ynLwBIlHjPMofQ8ZB0RQN4bImXx9QWJoJ3karG4xeK7jJJ1gZDZD&debug=all&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors
        public async Task<FacebookEventDTO> GetEvents()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                try
                {
                    var json = await client.GetStringAsync("https://graph.facebook.com/v8.0/coiffeursindependants/events?debug=all&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors&access_token=" + fbAccessToken);                
                    var res = JsonConvert.DeserializeObject<FacebookEventDTO>(json);
                    return res;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        //string url = "https://graph.facebook.com/v8.0/coiffeursindependants/posts?__activeScenarioIDs=%5B%5D&__activeScenarios=%5B%5D&debug=all&fields=id%2Cparent_id%2Cmessage%2Ccreated_time%2Cpicture%2Cicon%2Cpermalink_url%2Cfull_picture%2Cevent&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors"
        public async Task<FacebookFeedDTO> GetFeed()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                try
                {
                    //var json = await client.GetStringAsync("https://graph.facebook.com/v8.0/coiffeursindependants/posts?fields=message%2Cmessage_tags%2Cpicture%2Cfull_picture%2Cpermalink_url%2Ccreated_time%2Cstory&access_token=" + fbAccessToken);
                    var json = await client.GetStringAsync("https://graph.facebook.com/v8.0/coiffeursindependants/posts?debug=all&fields=id%2Cmessage%2Ccreated_time%2Cpicture%2Cicon%2Cpermalink_url%2Cfull_picture%2Cstory%2Cstory_tags&format=json&method=get&pretty=0&suppress_http_code=1&transport=cors&access_token=" + fbAccessToken);
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
