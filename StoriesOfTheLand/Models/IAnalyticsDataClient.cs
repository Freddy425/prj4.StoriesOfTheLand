namespace StoriesOfTheLand.Models
{
    public interface IAnalyticsDataClient
    {
       List<(string Dimension, string Metric)> callAPI(string dimensions, string metric);
    }
}
