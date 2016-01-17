using System;
using System.Collections.Generic;

namespace JuniorDoctorsStrike.TwitterApi
{
    public class Tweet
    {
        public DateTime Created { get; set; }
        public TimeSpan TimeSinceCreated { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
    }
}