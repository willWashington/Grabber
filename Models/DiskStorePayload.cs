using System.Text.Json;
using static Grabber.Utilities.CustomValues;

namespace Grabber.Models
{
    public class DiskStorePayload
    {
        public Payload Payload { get; set; }
        public PayloadType Type { get; set; }
        public string JSON { get; set; }

        public DiskStorePayload(Payload payload)
        {
            Payload = payload;

            switch (Payload)
            {
                case RedditPayload redditPayload:
                    Type = PayloadType.Reddit;
                    JSON = JsonSerializer.Serialize(redditPayload);
                    break;
                case PolygonPayload polygonPayload:
                    Type = PayloadType.Polygon;
                    JSON = JsonSerializer.Serialize(polygonPayload);
                    break;
                default:
                    Type = PayloadType.Generic;
                    JSON = "";
                    break;
            }
        }
    }
}