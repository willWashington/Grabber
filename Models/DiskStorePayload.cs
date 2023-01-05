using static Grabber.Utilities.CustomValues;

namespace Grabber.Models
{
    public class DiskStorePayload
    {
        public string Payload { get; set; }
        public PayloadType Type { get; set; }

        public DiskStorePayload(PayloadType type, string payload = "")
        {
            Type = type;
            Payload = payload;
        }
    }
}
