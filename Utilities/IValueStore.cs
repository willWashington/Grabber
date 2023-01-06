using Grabber.Models;

namespace Grabber.Utilities
{
    internal interface IValueStore
    {
        void Put(string key, string value);
        string Get(string key);
        void Clear();
        void Begin(List<Payload> payload);
        void End();
    }
}
