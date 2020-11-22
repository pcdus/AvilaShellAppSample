using System.Collections.Generic;
using System.Threading.Tasks;
using AvilaShellApp.Models;

namespace AvilaShellApp.Services.Abstractions
{
    public interface IDataService
    {
        Task<List<News>> GetNews(bool forceRefresh);
        Task<List<Event>> GetEvents(bool forceRefresh);
        Task<(List<News> news, List<Event> events)> GetNewsAndEvents(bool forceRefresh);
    }
}
