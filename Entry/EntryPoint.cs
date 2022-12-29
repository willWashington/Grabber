using Grabber.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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
        public static void Entry()
        {
            Collector.Collect();
        }
    }
}
