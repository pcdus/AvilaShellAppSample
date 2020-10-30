using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvilaShellAppSample.Monitoring;
using Xamarin.Essentials;

namespace AvilaShellAppSample.Infrastructure
{
    public class Contact : IContact
    {
        private readonly IEventTracker _eventTracker;

        public Contact()
        {
            _eventTracker = new AppCenterEventTracker();
        }

        public void Call(string phoneNumber)
        {
            try
            {
                PhoneDialer.Open(phoneNumber);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                _eventTracker.Error(anEx);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Phone Dialer is not supported on this device
                _eventTracker.Error(fnsEx);
            }
            catch (Exception ex)
            {
                // Other error has occurred
                _eventTracker.Error(ex);
            }
        }

        public async Task SendEmailAsync(string email)
        {
            var message = new EmailMessage
            {
                To = new List<string>() { email }
            };

            try
            {
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Email is not supported on this device
                _eventTracker.Error(fnsEx);
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                _eventTracker.Error(ex);
            }
        }
    }
}
