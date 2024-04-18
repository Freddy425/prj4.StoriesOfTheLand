using Google.Analytics.Data.V1Beta;
using System.Text;

namespace StoriesOfTheLand.Models
{
    public class RealAnalyticsDataClient : IAnalyticsDataClient
    {
        List<(string Dimension, string Metric)> IAnalyticsDataClient.callAPI(string dimensions, string metric)
        {
            var output = new List<(string Dimension, string Metric)>();

            // Set the environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "Controllers/Quickstart-be14e92f89f9.json");

            /**
             
TODO(developer): Uncomment this variable and replace with your
Google Analytics 4 property ID before running the sample.*/
            string propertyId = "429176036";

            // Using a default constructor instructs the client to use the credentials
            // specified in GOOGLE_APPLICATION_CREDENTIALS environment variable.
            BetaAnalyticsDataClient client = BetaAnalyticsDataClient.Create();

            // Initialize request argument(s)
            RunReportRequest request = new RunReportRequest
            {
                Property = "properties/" + propertyId,
                Dimensions = { new Dimension { Name = dimensions }, },
                Metrics = { new Metric { Name = metric }, },
                DateRanges = { new DateRange { StartDate = "2020-03-31", EndDate = "today" }, },
            };

            var response = client.RunReport(request);
            foreach (Row row in response.Rows)
            {
                var dimension = row.DimensionValues[0].Value;
                var visits = row.MetricValues[0].Value;
                output.Add((Dimension: dimension, Metric: visits));
            }
            return output;
        }

    }
}
