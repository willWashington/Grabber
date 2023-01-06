using Grabber.Models;
using Grabber.Utilities;
using QuickType;
using Reddit;
using Reddit.Inputs.Search;
using System;
using System.ComponentModel;
using System.Net.Http.Headers;

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
        static List<string> APIs = new();
        static List<string> Tickers = new();
        static List<string> QueryStrings = new();

        static List<Payload> payloads = new List<Payload>();

        public static void Cleanup()
        {
            APIs.Clear();
            Tickers.Clear();
            QueryStrings.Clear();
            payloads.Clear();
        }

        public static void Reach(string queryString = "")
        {
            if (string.IsNullOrEmpty(queryString) && QueryStrings?.Count == 0)
            {
                Console.WriteLine("No entires to query!");
                return;
            }

            if (queryString.StartsWith("$"))
            {
                queryString = queryString.Substring(1);
                if (!Tickers.Contains(queryString))
                {                    
                    Tickers.Add(queryString.ToUpper());
                }
            }

            if (!QueryStrings.Contains(queryString))
            {
                    QueryStrings.Add(queryString);
            }

            if (APIs.Count == 0) PopulateAPIList();

            QueryAPIsAsync();
        }

        private static void PopulateAPIList()
        {
            #region Market APIs
            foreach (var ticker in Tickers)
            {
                var polygonUrl = APIStringBuilder.GetPolygonAPIString(ticker);
                APIs.Add(polygonUrl);
            }
            #endregion
        }

        private static async Task QueryAPIsAsync()
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
            try
            {
                #region Process Reddit query if any

                foreach (var query in QueryStrings)
                {
                    await QueryReddit(query);
                }

                #endregion


                foreach (var repo in APIs)
                {
                    var json = await client.GetStringAsync(repo);
                    var converted = PolygonPayloadConverter.FromJson(json);
                    if (converted.Results.Count() > 0)
                    {
                        var resultList = converted.Results.ToList();
                        foreach (Result result in resultList)
                        {
                            payloads.Add(new PolygonPayload(result.PublishedUtc.Date, result.Title, result.ArticleUrl.AbsolutePath, result.Description));
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            

            using (FasterKeyValueStore diskWriter = new FasterKeyValueStore())
            {
                diskWriter.Begin(payloads);
            };
        }

        private static async Task QueryReddit(string query)
        {
            var refreshToken = Environment.GetEnvironmentVariable("REDDIT_REFRESH_TOKEN");
            var appID = Environment.GetEnvironmentVariable("REDDIT_APP_ID");
            var secretToken = Environment.GetEnvironmentVariable("REDDIT_SECRET_TOKEN");
            var reddit = new RedditClient(appId: appID, appSecret: secretToken, refreshToken: refreshToken);

            //get last 500 results that match the query string
            var posts = await Task.Run(() => reddit.Subreddit("all").Search(new SearchGetSearchInput(query, limit: 500)));

            var count = 0;
            posts.ForEach(x =>
            {
                count++;
                payloads.Add(new RedditPayload(x.Created, x.Title, x.Permalink, x.Author, x.Spam, x.UpVotes));
            });
            Console.WriteLine($"Reddit payloads added {count}");
        }
    }
}
