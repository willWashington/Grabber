using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabber.Utilities
{
    internal interface IValueStore
    {
        void Put(string key, string value);
        string Get(string key);
        void Clear();
        void Begin();
        void End();
    }
}
