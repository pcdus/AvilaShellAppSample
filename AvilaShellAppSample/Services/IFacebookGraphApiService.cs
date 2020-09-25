using System;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;

namespace AvilaShellAppSample.Services
{
    public interface IFacebookGraphApiService
    {
        Task<FacebookFeedDTO> GetFeed();
        Task<FacebookEventDTO> GetEvents();
    }
}
