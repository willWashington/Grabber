using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grabber.Execution
{
    static internal class Collector
    {
        public static List<string> StringsToQuery = new List<string>();

        private static Dictionary<int, string> stringsToWrite = new Dictionary<int, string>();
        //private int entryCount = DiskWriter.

        internal static void Collect()
        {
            //if the user hits a key, stop iteration for input
            if (Console.KeyAvailable)
            {
                var query = PromptUser();
                //stringsToWrite.Add()
            }
        }
        
        static string PromptUser()
        {
            Console.Clear();
            Console.WriteLine("Please enter a string to query!");
            Console.WriteLine("Don't be stupid. It'll break because I'm lazy.");
            var returnValue = "";
            returnValue = Console.ReadLine();
            if (returnValue == null) { returnValue = ""; };
            return returnValue;
        }
    }
}
