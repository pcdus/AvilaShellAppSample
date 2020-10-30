using System;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Command = MvvmHelpers.Commands.Command;
using System.Collections.Generic;
using AvilaShellAppSample.Monitoring;
using AvilaShellAppSample.Infrastructure;
using Map = AvilaShellAppSample.Infrastructure.Map;

namespace AvilaShellAppSample.ViewModels
{
    public class HomeViewModel : AvilaViewModelBase
    {
        private readonly IEventTracker _eventTracker;
        private readonly IContact _contact;
        private readonly IMap _map;

        string avilaPhoneNumber = "03 88 23 05 43";
        public string AvilaPhoneNumber
        {
            get { return avilaPhoneNumber; }
            set { SetProperty(ref avilaPhoneNumber, value); }
        }

        Address avilaAddress = new Address
        {
            Street = "69 rue des Grandes Arcades",
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
            _eventTracker = new AppCenterEventTracker();
            _contact = new Contact();
            _map = new Map();

            Title = "Home";

            // debug : get assembly files list
            /*
            var assembly = this.GetType().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            */

            _eventTracker.Display(EventPage.HomePage);
        }

        private void Call()
        {
            _eventTracker.Click(EventName.AvilaCall, EventPage.HomePage, EventPage.NativeCallApp);
            _contact.Call(AvilaPhoneNumber);
        }

        private async Task OpenMapAsync()
        {
            _eventTracker.Click(EventName.AvilaMap, EventPage.HomePage, EventPage.NativeMapApp);
            var placemark = new Placemark
            {
                CountryName = AvilaAddress.Country,
                Thoroughfare = AvilaAddress.Street,
                Locality = AvilaAddress.City
            };
            var options = new MapLaunchOptions
            {
                Name = "Avila"
            };
            await _map.OpenAsync(placemark, options);
        }

        private async Task SendEmailAsync()
        {
            _eventTracker.Click(EventName.AvilaMail, EventPage.HomePage, EventPage.NativeMailApp);
            await _contact.SendEmailAsync(AvilaEmail);
        }
    }
}
