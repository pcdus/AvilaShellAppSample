using System;
using System.Diagnostics;
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
                var eventsUri = new Uri($"{ApiConfig.FbApiPath}{ApiConfig.FbApiEventsParams}{ApiConfig.FbAccessToken}");
                using (new DisposableStopwatch(duration => _eventTracker.Api(ApiType.FacebookEvents, (long)duration.TotalMilliseconds)))
                {
                    return await _apiService.GetAndRetryWithTimeout<FacebookEventDTO>(eventsUri, ApiConfig.FbApiCallTimeout, ApiConfig.FbApiCallMaxRetries);
                }
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
                var feedUri = new Uri($"{ApiConfig.FbApiPath}{ApiConfig.FbApiPostsParams}{ApiConfig.FbAccessToken}");
                using (new DisposableStopwatch(duration => _eventTracker.Api(ApiType.FacebookPosts, (long)duration.TotalMilliseconds)))
                {
                    return await _apiService.GetAndRetryWithTimeout<FacebookFeedDTO>(feedUri, ApiConfig.FbApiCallTimeout, ApiConfig.FbApiCallMaxRetries);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("FacebookGraphApiService - GetFeed() - Exception");
                throw ex;
            }
        }
    }
}
