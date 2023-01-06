namespace Grabber.Models
{
    public class PolygonPayload : Payload
    {
        public string Description { get; set; }

        public PolygonPayload(DateTime createdDate, string title, string permalink, string description) : base(createdDate, title, permalink)
        {
            Description = description;
        }
    }
}
