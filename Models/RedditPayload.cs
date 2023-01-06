namespace Grabber.Models
{
    public class RedditPayload : Payload
    {
        public string Author { get; set; }
        public bool Spam { get; set; }
        public int Upvotes { get; set; }

        public RedditPayload(DateTime createdDate, string title, string permalink, string author, bool spam, int upvotes) : base(createdDate, title, permalink)
        {
            Author = author;
            Spam = spam;
            Upvotes = upvotes;
        }
    }
}
