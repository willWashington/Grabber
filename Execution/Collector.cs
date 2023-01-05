namespace Grabber.Execution
{
    static internal class Collector
    {
        public static List<string> StringsToQuery = new List<string>();
        public static readonly bool Running = false;

        private static Dictionary<int, string> stringsToWrite = new Dictionary<int, string>();
        private static bool firstRun = true;


        internal static async Task CollectAsync()
        {

            //if the user hits a key, stop iteration for input
            if (Console.KeyAvailable)
            {
                //var query = PromptUser();
            }

            if (firstRun)
            {
                firstRun = false;
                Reacher.Reach();
                await Reacher.QueryAPIsAsync();
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
