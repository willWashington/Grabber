namespace Grabber.Utilities
{
    public static class APIStringBuilder
    {
        public static string GetPolygonAPIString(string ticker)
        {
            var polygonToken = Environment.GetEnvironmentVariable("POLYGON_KEY");
            var polygonUrl = "https://api.polygon.io/v2/reference/news?ticker=";
            //var date = DateTime.UtcNow.AddDays(-8).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
            polygonUrl += ticker.ToString().Trim();
            polygonUrl += $"&limit=100&apiKey={polygonToken}";
            return polygonUrl;
        }
    }
}
