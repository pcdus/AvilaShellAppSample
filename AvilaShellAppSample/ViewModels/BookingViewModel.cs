using System;
using System.Threading.Tasks;
using MvvmHelpers.Commands;

namespace AvilaShellAppSample.ViewModels
{
    public class BookingViewModel : AvilaViewModelBase
    {
        string avilaUrlBooking = "https://booking.wavy.pro/avila";
        public string AvilaUrlBooking
        {
            get { return avilaUrlBooking; }
            set { SetProperty(ref avilaUrlBooking, value); }
        }

        public Command RefreshCommand => new Command(this.Refresh);

        public BookingViewModel()
        {
            Title = "Booking";
        }

        public void Refresh(object sender)
        {
            try
            {
                var view = sender as Xamarin.Forms.WebView;
                view.Reload();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
