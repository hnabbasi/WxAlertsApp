using System;
namespace XamWxApp.Models
{
    public class Alert
    {
        public string Id { get; set; }
        public DateTimeOffset Sent { get; set; }
        public string Event { get; set; }
        public string Headline { get; set; }
    }
}