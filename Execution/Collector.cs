namespace Grabber.Execution
{
    static internal class Collector
    {
        public static List<string> StringsToQuery = new List<string>();
        public static readonly bool Running = false;

        private static Dictionary<int, string> stringsToWrite = new Dictionary<int, string>();
        private static bool firstRun = true;


        internal static void CollectAsync()
        {
            
        }


    }
}
