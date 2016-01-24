using System;

namespace JuniorDoctorsStrike.Common
{
    public class Message
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
    }
}