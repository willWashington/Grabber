using Grabber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
    //iimP0TbNxwM8FVAtBjGpt_669YD9Eg
    //GET https://api.marketaux.com/v1/news/all?symbols=TSLA,AMZN,MSFT&filter_entities=true&language=en&api_token=YOUR_API_TOKEN - stock news

    //VtNkNGdLpeYafClNYTOUqFIDbcBgYhkRHEHo0CHu    

    //https://www.alphavantage.co/documentation/ - ticker historical info

    public static class Reacher
    {
        public static List<string> APIs = new List<string>();
        public static List<string> Tickers = new List<string>();
        public static List<string> QueryStrings = new List<string>();

        public static void Reach(string queryString)
        {
            if (APIs.Count == 0) PopulateAPIList();
            BuildCollection();

            var payload = new DiskStorePayload();

            using (FasterKeyValueStore diskWriter = new FasterKeyValueStore())
            {
                diskWriter.WriteToDisk(payload.Payload);
            };
        }

        private static async void BuildCollection()
        {
            Tickers.Add("NTDOY");
            PopulateAPIList();

            using HttpClient client = new HttpClient();
            await ProcessRepositoriesAsync(client);
        }

        private static void PopulateAPIList()
        {
            #region Market APIs
            var marketauxToken = "VtNkNGdLpeYafClNYTOUqFIDbcBgYhkRHEHo0CHu";
            var marketauxUrl = "https://api.marketaux.com/v1/news/all?symbols=";
            foreach (var ticker in Tickers)
            {
                //add tickers
                marketauxUrl += ticker.ToString().Trim() + ",";
            }
            marketauxUrl += $"&must_have_entities=true&filter_entities=true&language=en&api_token={marketauxToken}";
            APIs.Add(marketauxUrl);
            #endregion
        }



        public static async Task ProcessRepositoriesAsync(HttpClient client)
        {

        }


    }
}
