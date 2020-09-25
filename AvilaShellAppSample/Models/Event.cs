using System;
namespace AvilaShellAppSample.Models
{
    public class Event
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public Address Address { get; set; }
    }
}
