using Grabber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabber.Utilities
{
    public static class GrabberLogger
    {
        public static bool Quiet = false;

        public static void ReportPayload(RedditPayload payload)
        {
            Log("***********************************", ConsoleColor.Red);
            Log($"{payload.Title} - {payload.CreatedDate}", ConsoleColor.Yellow);
            Log("---------------");
            Log($"{payload.Author} - Spam? {payload.Spam}");
            Log($"https://reddit.com/{payload.Permalink}", ConsoleColor.Blue);
            Log("***********************************", ConsoleColor.Green);
            Console.WriteLine(Environment.NewLine);
        }

        //{payload.Title} {payload.Upvotes} {payload.Spam} {payload.Permalink}

        public static void ReportPayload(PolygonPayload payload)
        {
            Log("***********************************", ConsoleColor.Red);
            Log($"{payload.Title} - {payload.CreatedDate}", ConsoleColor.Yellow);
            Log("-----");
            Log($"{payload.Description}");
            Log($"{payload.Permalink}", ConsoleColor.Blue);
            Log("***********************************", ConsoleColor.Green);
            Console.WriteLine(Environment.NewLine);
        }

        public static void Log(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (Quiet) return;

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
