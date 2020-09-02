using System;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AvilaShellAppSample.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        ImageSource avilaIndoorImageSource = null;
        public ImageSource AvilaIndoorImageSource
        {
            get { return avilaIndoorImageSource; }
            set { SetProperty(ref avilaIndoorImageSource, value); }
        }

        string avilaPhoneNumber = "03 88 23 05 43";
        public string AvilaPhoneNumber
        {
            get { return avilaPhoneNumber; }
            set { SetProperty(ref avilaPhoneNumber, value); }
        }

        Address avilaAddress = new Address
        {
            StreetNumber = 69,
            StreetName = "rue des Grandes Arcades",
            ZipCode = "67 000",
            City = "STRASBOURG",
            Country = "France"
        };
        public Address AvilaAddress
        {
            get { return avilaAddress; }
            set { SetProperty(ref avilaAddress, value); }
        }

        public Command CallCommand => new Command(this.Call);
        public Command OpenMapCommand => new Command(this.OpenMapAsync);

        public HomeViewModel()
        {
            Title = "Home";

            var assembly = this.GetType().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }

            AvilaIndoorImageSource = ImageSource.FromResource("AvilaShellAppSample.Resources.avila_indoor.jpg");

        }

        private void Call()
        {
            try
            {
                PhoneDialer.Open(AvilaPhoneNumber);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

        }

        /*
        private async Task OpenMapAsync()
        {

        }
        */
    }
}
