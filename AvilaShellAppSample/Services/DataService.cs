using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AvilaShellAppSample.Models;

namespace AvilaShellAppSample.Services
{
    public class DataService : IDataService
    {
        private readonly IFacebookGraphApiService _facebookGraphApiService;

        private FacebookEventDTO fbEvents = null;
        private FacebookFeedDTO fbFeed = null;


        public DataService()
        {
            _facebookGraphApiService = new FacebookGraphApiService();
        }

        public async Task<List<Event>> GetEvents()
        {
            var results = new List<Event>();
            await GetFbData();
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
            return results;
        }

        public async Task<List<News>> GetNews()
        {
            var results = new List<News>();
            await GetFbData();
            if (fbFeed != null)
            { 
                foreach (var item in fbFeed.Posts)
                {
                    if ((item.Icon == "https://www.facebook.com/images/icons/photo.gif" || item.Icon == "https://www.facebook.com/images/icons/video.gif")
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
            return results;
        }

        private async Task GetFbData()
        {
            if (fbFeed == null)
            {
                fbFeed = await _facebookGraphApiService.GetFeed();
                if (fbFeed == null)
                {
                    Debug.WriteLine("fbFeed null");
                }
                else
                {
                    Debug.WriteLine("fbFeed not null");
                }
            }
            if (fbEvents == null)
            {
                fbEvents = await _facebookGraphApiService.GetEvents();
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

    }
}
