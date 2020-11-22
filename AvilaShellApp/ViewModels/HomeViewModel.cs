using System.Threading.Tasks;
using AvilaShellApp.Models;
using Xamarin.Essentials;
using MvvmHelpers.Commands;
using Command = MvvmHelpers.Commands.Command;
using AvilaShellApp.Monitoring;
using AvilaShellApp.Infrastructure;
using Map = AvilaShellApp.Infrastructure.Map;

namespace AvilaShellApp.ViewModels
{
    public class HomeViewModel : AvilaViewModelBase
    {
        private readonly IEventTracker _eventTracker;
        private readonly IContact _contact;
        private readonly IMap _map;

        string avilaPhoneNumber;
        public string AvilaPhoneNumber
        {
            get { return avilaPhoneNumber; }
            set { SetProperty(ref avilaPhoneNumber, value); }
        }

        Address avilaAddress;
        public Address AvilaAddress
        {
            get { return avilaAddress; }
            set { SetProperty(ref avilaAddress, value); }
        }

        string avilaEmail;
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

            this.Title = Strings.Strings.HomePageTitle;
            this.AvilaPhoneNumber = Strings.Strings.HomePageAvilaPhoneNumber;
            this.AvilaAddress = new Address
            {
                Name = Strings.Strings.HomePageAvilaAddressName,
                Street = Strings.Strings.HomePageAvilaAddressStreet,
                ZipCode = Strings.Strings.HomePageAvilaAddressZipCode,
                City = Strings.Strings.HomePageAvilaAddressCity,
                Country = Strings.Strings.HomePageAvilaAddressCountry
            };
            this.AvilaEmail = Strings.Strings.HomePageAvilaEmail;

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
                Name = AvilaAddress.Name
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
