using System.Threading.Tasks;
using AvilaShellApp.Models;

namespace AvilaShellApp.Services.Abstractions
{
    public interface IFacebookGraphApiService
    {
        Task<FacebookFeedDTO> GetFeed();
        Task<FacebookEventDTO> GetEvents();
    }
}
