using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AvilaShellAppSample.Helpers;
using AvilaShellAppSample.Models;
using AvilaShellAppSample.Monitoring;
using AvilaShellAppSample.Services.Abstractions;


namespace AvilaShellAppSample.Services
{
    public class FacebookGraphApiService : IFacebookGraphApiService
    {
        // generate permanent access token:
        // https://stackoverflow.com/questions/17197970/facebook-permanent-page-access-token-
        // user token
        //string fbAccessToken = "EAAYmP2nRRLYBAEBnTIxTMBNfA7owOB0uhUmRHX7QfO1bZAlBUBho0egmHs1jzMP01onbIWiT8ZCxzF0ZB1X8bmUFaYZALITWVn0oZBrsM3flOY9xfWZByu6Lxf4gV3BW9MT68YZBngkrZAt6rLAemiHvtK5LkuoaXjcH9qUvaLsLMQZDZD";
        // page token

        private readonly IEventTracker _eventTracker;
        private readonly IApiService _apiService;

        public FacebookGraphApiService()
        {
            Debug.WriteLine("FacebookGraphApiService - Ctor()");
            _eventTracker = new AppCenterEventTracker();
            _apiService = new ApiService();
        }

        public async Task<FacebookEventDTO> GetEvents()
        {
            Debug.WriteLine("FacebookGraphApiService - GetEvents()");
            try
            {
                var eventsUri = new Uri($"{ApiConfig.FbApiPath}{ApiConfig.FbApiEventsParams}&access_token={ApiConfig.FbAccessToken}");
                var res = await _apiService.GetAndRetryWithTimeout<FacebookEventDTO>(eventsUri, 15, 3);
                return res;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("FacebookGraphApiService - GetEvents() - Exception");
                throw ex;
            }
        }

        public async Task<FacebookFeedDTO> GetFeed()
        {
            Debug.WriteLine("FacebookGraphApiService - GetFeed()");
            try
            {
                var feedUri = new Uri($"{ApiConfig.FbApiPath}{ApiConfig.FbApiPostsParams}&access_token={ApiConfig.FbAccessToken}");
                var res = await _apiService.GetAndRetryWithTimeout<FacebookFeedDTO>(feedUri, 15, 3);
                return res;
                    
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FacebookGraphApiService - GetFeed() - Exception");
                throw ex;
            }
        }
    }
}
