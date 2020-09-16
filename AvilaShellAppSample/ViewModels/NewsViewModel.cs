using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AvilaShellAppSample.Services;
using AvilaShellAppSample.Models;
using System.Diagnostics;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;

namespace AvilaShellAppSample.ViewModels
{
    public class NewsViewModel : AvilaViewModelBase
    {

        private readonly ApiFacebookGraphService _facebookApi;

        FacebookFeedDTO fbFeed = null;
        public FacebookFeedDTO FbFeed
        {
            get { return fbFeed; }
            set { SetProperty(ref fbFeed, value); }
        }

        public ObservableCollection<News> FbNews { get; }

        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public AsyncCommand RefreshCommand => new AsyncCommand(this.GetFbNewsAsync);

        public NewsViewModel()
        {
            Title = "News";
            FbNews = new ObservableCollection<News>();
            _facebookApi = new ApiFacebookGraphService();

            //Task.Run(async () => { await GetFbNewsAsync(); }).Wait();
            //Task.Run(async () => await GetFbNewsAsync());
        }


        private async Task GetFbNewsAsync()
        {
            IsRefreshing = true;
            fbFeed = await _facebookApi.GetFacebookNewsAsync();
            if (fbFeed == null)
            {
                Debug.WriteLine("null");
            }
            else
            {
                Debug.WriteLine("not null");
                foreach (var item in fbFeed.Posts)
                {
                    FbNews.Add(new News
                    {
                        Date = item.CreatedTime,
                        Description = item.Message,
                        Id = item.Id,
                        Image = item.FullPicture
                    });
                }
            }
            IsRefreshing = false;
        }
    }
}

