namespace Grabber.Models
{
    public class Payload
    {
        public Guid ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Permalink { get; set; }

        public Payload(DateTime createdDate, string title, string permalink)
        {
            ID = Guid.NewGuid();
            CreatedDate = createdDate;
            Title = title;
            Permalink = permalink;
        }
    }
}
