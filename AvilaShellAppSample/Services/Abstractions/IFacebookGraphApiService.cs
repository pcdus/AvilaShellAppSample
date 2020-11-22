using System.Threading.Tasks;
using AvilaShellAppSample.Models;

namespace AvilaShellAppSample.Services.Abstractions
{
    public interface IFacebookGraphApiService
    {
        Task<FacebookFeedDTO> GetFeed();
        Task<FacebookEventDTO> GetEvents();
    }
}
