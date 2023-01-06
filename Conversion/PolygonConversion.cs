namespace QuickType
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Globalization;

    public partial class PolygonPayloadConverter
    {
        [JsonProperty("results")]
        public Result[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("next_url")]
        public Uri NextUrl { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("publisher")]
        public Publisher Publisher { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("published_utc")]
        public DateTimeOffset PublishedUtc { get; set; }

        [JsonProperty("article_url")]
        public Uri ArticleUrl { get; set; }

        [JsonProperty("tickers")]
        public string[] Tickers { get; set; }

        [JsonProperty("amp_url")]
        public Uri AmpUrl { get; set; }

        [JsonProperty("image_url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("keywords", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Keywords { get; set; }
    }

    public partial class Publisher
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("homepage_url")]
        public Uri HomepageUrl { get; set; }

        [JsonProperty("logo_url")]
        public Uri LogoUrl { get; set; }

        [JsonProperty("favicon_url")]
        public Uri FaviconUrl { get; set; }
    }

    public partial class PolygonPayloadConverter
    {
        public static PolygonPayloadConverter FromJson(string json) => JsonConvert.DeserializeObject<PolygonPayloadConverter>(json, QuickType.PolygonConversion.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PolygonPayloadConverter self) => JsonConvert.SerializeObject(self, QuickType.PolygonConversion.Settings);
    }

    internal static class PolygonConversion
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}