using System;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Command = MvvmHelpers.Commands.Command;
using System.Collections.Generic;

namespace AvilaShellAppSample.ViewModels
{
    public class HomeViewModel : AvilaViewModelBase
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
            Street = "69,rue des Grandes Arcades",
            ZipCode = "67 000",
            City = "STRASBOURG",
            Country = "France"
        };
        public Address AvilaAddress
        {
            get { return avilaAddress; }
            set { SetProperty(ref avilaAddress, value); }
        }

        string avilaEmail = "avila.coiffure@voila.fr";
        public string AvilaEmail
        {
            get { return avilaEmail; }
            set { SetProperty(ref avilaEmail, value); }
        }

        public Command CallCommand => new Command(this.Call);
        public AsyncCommand OpenMapCommand => new AsyncCommand(this.OpenMapAsync);
        public AsyncCommand SendEmailCommand => new AsyncCommand(this.SendEmailAsync);

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

        private async Task OpenMapAsync()
        {
            var placemark = new Placemark
            {
                CountryName = AvilaAddress.Country,
                //Thoroughfare = String.Format("{0} {1}", AvilaAddress.StreetNumber, AvilaAddress.StreetName),
                Thoroughfare = AvilaAddress.Street,
                Locality = AvilaAddress.City
            };
            var options = new MapLaunchOptions
            {
                Name = "Avila"
            };

            try
            {
                await Map.OpenAsync(placemark, options);
            }
            catch (Exception ex)
            {
                // No map application available to open or placemark can not be located
            }
        }

        private async Task SendEmailAsync()
        {
            try
            {
                var message = new EmailMessage
                {
                    To = new List<string>() { AvilaEmail }
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}
