using System;
using System.Threading.Tasks;
using AvilaShellAppSample.Monitoring;
using MvvmHelpers.Commands;

namespace AvilaShellAppSample.ViewModels
{
    public class BookingViewModel : AvilaViewModelBase
    {
        private readonly IEventTracker _eventTracker;

        string avilaUrlBooking = "https://booking.wavy.pro/avila";
        public string AvilaUrlBooking
        {
            get { return avilaUrlBooking; }
            set { SetProperty(ref avilaUrlBooking, value); }
        }

        public Command RefreshCommand => new Command(this.Refresh);

        public BookingViewModel()
        {
            _eventTracker = new AppCenterEventTracker();

            Title = "Booking";
        }

        public void Refresh(object sender)
        {
            try
            {
                _eventTracker?.Click(EventName.RefreshBookingWebview, EventPage.BookingPage, EventPage.BookingPage);
                var view = sender as Xamarin.Forms.WebView;
                view.Reload();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
