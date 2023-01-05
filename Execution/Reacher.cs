using Grabber.Models;
using Grabber.Utilities;
using QuickType;
using Reddit;
using Reddit.Inputs.Search;
using System.Net.Http.Headers;
using static Grabber.Utilities.CustomValues;

namespace Grabber.Execution
{
    //apis:
    //https://go-apod.herokuapp.com/ - astronomy picture
    //https://binance-docs.github.io/apidocs/spot/en/#introduction - crypto
    //bored - activities
    //https://datausa.io/visualize - US data
    //https://housestockwatcher.com/api - congress member stock trades
    //https://documentation.image-charts.com/ - make a chart image
    //https://imgflip.com/api - memes
    //https://world.openfoodfacts.org/data - food info
    //https://open.fda.gov/ - FDA    
    //https://rickandmortyapi.com/ - rick and morty characters etc
    //https://developers.teleport.org/api/ - city and quality of life data
    //https://sunrisesunset.io/api/ - sunrise sunset
    //https://apilist.fun/api/wordnik - sample text
    //https://apilist.fun/api/scrapingninja - web scraper
    //https://apilist.fun/api/scrapestack - another scraper
    //https://apilist.fun/api/youtube-api - youtube    
    //https://apilist.fun/api/newseum-newsmania - trivia questions

    //https://www.reddit.com/dev/api/ - reddit
    //https://www.reddit.com/r/Wallstreetbets/top.json?limit=10&t=year
    //GET https://api.marketaux.com/v1/news/all?symbols=TSLA,AMZN,MSFT&filter_entities=true&language=en&api_token=YOUR_API_TOKEN - stock news

    //https://www.alphavantage.co/documentation/ - ticker historical info

    public static class Reacher
    {
        public static List<string> APIs = new List<string>();
        public static List<string> Tickers = new List<string>();
        public static List<string> QueryStrings = new List<string>();

        private static List<DiskStorePayload> payloads = new List<DiskStorePayload>();

        public static void Reach(string queryString = "")
        {
            if (Tickers.Count == 0) BuildCollection();
            if (APIs.Count == 0) PopulateAPIList();
        }

        private static void BuildCollection()
        {
            Tickers.Add("HardyRekshin");
            Tickers.Add("NTDOY");
            Tickers.Add("AMZN");
        }

        private static void PopulateAPIList()
        {
            #region Market APIs            
            foreach (var ticker in Tickers)
            {
                var polygonUrl = APIStringBuilder.GetPolygonAPIString(ticker);
                APIs.Add(polygonUrl);
                QueryReddit(ticker);
            }
            #endregion
        }

        public static string QueryReddit(string query)
        {
            var refreshToken = Environment.GetEnvironmentVariable("REDDIT_REFRESH_TOKEN");
            var appID = Environment.GetEnvironmentVariable("REDDIT_APP_ID");
            var secretToken = Environment.GetEnvironmentVariable("REDDIT_SECRET_TOKEN");
            var reddit = new RedditClient(appId: appID, appSecret: secretToken, refreshToken: refreshToken);
            //var test = reddit.Search(query, null);
            var posts = reddit.Subreddit("all").Search(new SearchGetSearchInput(query, limit: 500));
            posts.ForEach(x =>
            {
                Console.WriteLine($"{x.Author}, {x.Created}, {x.Title}, Spam = {x.Spam}, {x.Permalink}, Upvotes = {x.UpVotes} {Environment.NewLine}");
            });
            Console.ReadLine();
            return "";
        }

        public static async Task QueryAPIsAsync()
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            await ProcessRepositoriesAsync(client);
        }

        private static async Task ProcessRepositoriesAsync(HttpClient client)
        {
            using (FasterKeyValueStore diskWriter = new FasterKeyValueStore())
            {
                foreach (var repo in APIs)
                {
                    var json = await client.GetStringAsync(repo);
                    var payload = new DiskStorePayload(PayloadType.Polygon, json);
                    var converted = PolygonPayload.FromJson(json);
                    diskWriter.WriteToDisk(payload.Payload);
                    Console.WriteLine(payload.Payload);
                }
            };
        }
    }
}
