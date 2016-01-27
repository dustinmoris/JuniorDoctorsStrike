using System;

namespace JuniorDoctorsStrike.Common
{
    public class Message
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public User User { get; set; }
        public string Text { get; set; }

        public long IdValue => long.Parse(Id);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(obj, this)) return true;

            var message = obj as Message;
            return message != null && message.Id.Equals(Id);
        }

        protected bool Equals(Message other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"ID: {Id}, User: {User.Name}, Date: {Created}";
        }
    }
}