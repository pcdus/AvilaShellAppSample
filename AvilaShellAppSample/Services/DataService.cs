using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;
using AvilaShellAppSample.Services.Abstractions;

namespace AvilaShellAppSample.Services
{
    public class DataService : IDataService
    {
        private readonly IFacebookGraphApiService _facebookGraphApiService;

        private FacebookEventDTO fbEvents = null;
        private FacebookFeedDTO fbFeed = null;

        public DataService()
        {
            Debug.WriteLine("DataService - Ctor()");
            _facebookGraphApiService = new FacebookGraphApiService();
        }

        public async Task<List<Event>> GetEvents(bool forceRefresh = false)
        {
            Debug.WriteLine("DataService - GetEvents()");
            var results = new List<Event>();
            await GetFbData(forceRefresh);
            if (fbFeed != null && fbEvents != null)
            {
                foreach (var item in fbFeed.Posts)
                {
                    if (item.Icon == "https://www.facebook.com/images/icons/event.gif" &&
                        item.StoryTags != null && item.StoryTags.Count > 1)
                    {
                        var newEvent = new Event();
                        var eventStoryTag = item.StoryTags.FirstOrDefault(x => x.Type == "event");
                        if (eventStoryTag != null)
                        {
                            newEvent.Id = eventStoryTag.Id;
                            newEvent.Image = item.FullPicture;
                            newEvent.Url = item.PermalinkUrl;
                            var fbEvent = fbEvents.Events.SingleOrDefault(x => x.Id == eventStoryTag.Id);
                            var fbEventPlace = fbEvent.Place;
                            if (fbEvent != null)
                            {
                                newEvent.StartDate = fbEvent.StartTime;
                                newEvent.EndDate = fbEvent.EndTime;
                                newEvent.Name = fbEvent.Name;
                                newEvent.Description = fbEvent.Description;
                                if (fbEventPlace != null)
                                {
                                    Address eventLocation = new Address
                                    {
                                        Name = fbEventPlace.Name,
                                        City = fbEventPlace.Location?.City,
                                        Country = fbEventPlace.Location?.Country,
                                        Street = fbEventPlace.Location?.Street,
                                        ZipCode = fbEventPlace.Location?.Zip
                                    };
                                    newEvent.Address = eventLocation;
                                }
                                results.Add(newEvent);
                            }
                        }
                    }
                }
            }
            results = results.OrderByDescending(x => x.StartDate).Take(ApiConfig.FbMaxEventsDisplayed).ToList();
            return results;
        }

        public async Task<List<News>> GetNews(bool forceRefresh = false)
        {
            Debug.WriteLine("DataService - GetNews()");
            var results = new List<News>();
            await GetFbData(forceRefresh);
            if (fbFeed != null)
            { 
                foreach (var item in fbFeed.Posts)
                {
                    if ((item.Icon == "https://www.facebook.com/images/icons/photo.gif"
                      || item.Icon == "https://www.facebook.com/images/icons/video.gif")
                      && (!string.IsNullOrEmpty(item.Message)))
                    {
                        results.Add(new News
                        {
                            Date = item.CreatedTime,
                            Description = item.Message,
                            Id = item.Id,
                            Image = item.FullPicture,
                            Url = item.PermalinkUrl
                        });
                    }
                }
            }
            results = results.OrderByDescending(x => x.Date).Take(ApiConfig.FbMaxPostsDisplayed).ToList();
            return results;
        }

        public async Task<(List<News> news, List<Event> events)> GetNewsAndEvents(bool forceRefresh)
        {
            Debug.WriteLine("DataService - GetNewsAndEvents()");
            await GetFbData(forceRefresh);
            var news = AdaptFbNews();
            var events = AdaptFbEvents();
            return (news, events);
        }

        private async Task GetFbData(bool forceRefresh = false)
        {
            Debug.WriteLine("DataService - GetFbData()");
            if (fbFeed == null || forceRefresh)
            {
                await GetFbFeed();
                //fbFeed = await _facebookGraphApiService.GetFeed();
                if (fbFeed == null)
                {
                    Debug.WriteLine("fbFeed null");
                }
                else
                {
                    Debug.WriteLine("fbFeed not null");
                }
            }
            if (fbEvents == null || forceRefresh)
            {
                await GetFbEvents();
                //fbEvents = await _facebookGraphApiService.GetEvents();
                if (fbEvents == null)
                {
                    Debug.WriteLine("fbEvents null");
                }
                else
                {
                    Debug.WriteLine("fbEvents not null");
                }
            }
        }

        private async Task GetFbFeed()
        {
            Debug.WriteLine("DataService - GetFbFeed()");
            try
            {
                fbFeed = await _facebookGraphApiService.GetFeed();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DataService - GetFbFeed() - Exception");
                //await userDialogs.AlertAsync("Unable to get data", "Error", "Ok");
                throw ex;
            }

        }

        private async Task GetFbEvents()
        {
            Debug.WriteLine("DataService - GetFbEvents()");
            try
            {
                fbEvents = await _facebookGraphApiService.GetEvents();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DataService - GetFbEvents() - Exception");
                //await userDialogs.AlertAsync("Unable to get data", "Error", "Ok");
                throw ex;
            }
        }

        private List<News> AdaptFbNews()
        {
            Debug.WriteLine("DataService - AdaptFbNews()");
            var results = new List<News>();
            if (fbFeed != null)
            {
                foreach (var item in fbFeed.Posts)
                {
                    if ((item.Icon == "https://www.facebook.com/images/icons/photo.gif"
                      || item.Icon == "https://www.facebook.com/images/icons/video.gif")
                      && (!string.IsNullOrEmpty(item.Message)))
                    {
                        results.Add(new News
                        {
                            Date = item.CreatedTime,
                            Description = item.Message,
                            Id = item.Id,
                            Image = item.FullPicture,
                            Url = item.PermalinkUrl
                        });
                    }
                }
            }
            results = results.OrderByDescending(x => x.Date).Take(ApiConfig.FbMaxPostsDisplayed).ToList();
            return results;
        }

        private List<Event> AdaptFbEvents()
        {
            Debug.WriteLine("DataService - AdaptFbEvents()");
            var results = new List<Event>();
            if (fbFeed != null && fbEvents != null)
            {
                foreach (var item in fbFeed.Posts)
                {
                    if (item.Icon == "https://www.facebook.com/images/icons/event.gif" &&
                        item.StoryTags != null && item.StoryTags.Count > 1)
                    {
                        var newEvent = new Event();
                        var eventStoryTag = item.StoryTags.FirstOrDefault(x => x.Type == "event");
                        if (eventStoryTag != null)
                        {
                            newEvent.Id = eventStoryTag.Id;
                            newEvent.Image = item.FullPicture;
                            newEvent.Url = item.PermalinkUrl;
                            var fbEvent = fbEvents.Events.SingleOrDefault(x => x.Id == eventStoryTag.Id);
                            var fbEventPlace = fbEvent.Place;
                            if (fbEvent != null)
                            {
                                newEvent.StartDate = fbEvent.StartTime;
                                newEvent.EndDate = fbEvent.EndTime;
                                newEvent.Name = fbEvent.Name;
                                newEvent.Description = fbEvent.Description;
                                if (fbEventPlace != null)
                                {
                                    Address eventLocation = new Address
                                    {
                                        Name = fbEventPlace.Name,
                                        City = fbEventPlace.Location?.City,
                                        Country = fbEventPlace.Location?.Country,
                                        Street = fbEventPlace.Location?.Street,
                                        ZipCode = fbEventPlace.Location?.Zip
                                    };
                                    newEvent.Address = eventLocation;
                                }
                                results.Add(newEvent);
                            }
                        }
                    }
                }
            }
            results = results.OrderByDescending(x => x.StartDate).Take(ApiConfig.FbMaxEventsDisplayed).ToList();
            return results;
        }

    }
}
