using System;

namespace JuniorDoctorsStrike.Common
{
    public class Message
    {
        public DateTime Created { get; set; }
        public TimeSpan TimeSinceCreated { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
    }
}