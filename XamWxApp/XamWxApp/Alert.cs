using System;
namespace XamWxApp.Models
{
    public class Alert
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string AreaDesc { get; set; }
        public DateTimeOffset Sent { get; set; }
        public DateTimeOffset Effective { get; set; }
        public DateTimeOffset Expires { get; set; }
        public string Status { get; set; }
        public string Severity { get; set; }
        public string Certainty { get; set; }
        public string Urgency { get; set; }
        public string Event { get; set; }
        public string SenderName { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public string Response { get; set; }
    }
}