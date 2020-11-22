using System;
using System.Threading.Tasks;
using AvilaShellApp.Monitoring;
using Xamarin.Essentials;

namespace AvilaShellApp.Infrastructure
{
    public class Map : IMap
    {
        private readonly IEventTracker _eventTracker;

        public Map()
        {
            _eventTracker = new AppCenterEventTracker();
        }

        public async Task OpenAsync(Placemark placemark, MapLaunchOptions options)
        {
            try
            {
                await Xamarin.Essentials.Map.OpenAsync(placemark, options);
            }
            catch (Exception ex)
            {
                // No map application available to open or placemark can not be located
                _eventTracker.Error(ex);
            }
        }
    }
}
