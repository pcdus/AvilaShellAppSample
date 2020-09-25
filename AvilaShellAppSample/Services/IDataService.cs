using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;

namespace AvilaShellAppSample.Services
{
    public interface IDataService
    {
        Task<List<News>> GetNews();
        Task<List<Event>> GetEvents();
    }
}
