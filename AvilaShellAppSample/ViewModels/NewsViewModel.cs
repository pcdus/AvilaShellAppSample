using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AvilaShellAppSample.Services;
using AvilaShellAppSample.Models;
using System.Diagnostics;

namespace AvilaShellAppSample.ViewModels
{
    public class NewsViewModel : AvilaViewModelBase
    {

        private readonly ApiFacebookGraphService _facebookApi;

        FacebookFeedDTO fbNews = null;
        public FacebookFeedDTO FbNews
        {
            get { return fbNews; }
            set { SetProperty(ref fbNews, value); }
        }


        public NewsViewModel()
        {
            Title = "News";
            _facebookApi = new ApiFacebookGraphService();

            //Task.Run(async () => { await GetFbNewsAsync(); }).Wait();
            Task.Run(async () => await GetFbNewsAsync());
        }


        private async Task GetFbNewsAsync()
        {
            fbNews = await _facebookApi.GetFacebookNewsAsync();
            if (fbNews == null)
            {
                Debug.WriteLine("null");
            }
            else
            {
                Debug.WriteLine("not null");
            }
        }
    }
}

