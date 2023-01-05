using Grabber.Execution;
using Reddit.AuthTokenRetriever;
using System.Diagnostics;
using Reddit.AuthTokenRetriever;
using System.Diagnostics;

namespace Grabber.Entry
{
    /// <summary>
    /// Grabber is a worker service that grabs anything my other services need
    /// It leverages open APIs to collect data and store it on my local machine via FASTER
    /// 
    /// Entry Point is the entry point to the application from the worker service side
    /// Collector manages collections and is responsible for knowing what it needs to collect
    /// Reacher reaches out to collect what Collector wants and sends it to Writer
    /// Writer writes the data via FASTER to disk
    /// Inspector inspects the data and cleans the system of needless information
    /// </summary>

    public static class EntryPoint
    {
        public static async void Entry()
        {
            if (!Collector.Running)
            {
                await Collector.CollectAsync();
            }
        }
    }
}
