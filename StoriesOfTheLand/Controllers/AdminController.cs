using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Data;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Google.Analytics.Data.V1Beta;
using System.Collections;
using Microsoft.Identity.Client;
using System.Globalization;
using StoriesOfTheLand.Models;

namespace StoriesOfTheLand.Controllers
{
    public class AdminController : Controller
    {
        private readonly StoriesOfTheLandContext _context;

        public AdminController(StoriesOfTheLandContext context)
        {
            _context = context;
        }
        /*
        * The printAll method is used to print all the QR codes on the site
        */
        [Authorize]
        public async Task<IActionResult> printAll(int? id)
        {
            if (id.HasValue) //if there is a passed in value it will return the Specimen wiht that ID
            {
                var specimen = await _context.Specimen
                    .Include(s => s.MediaList)
                    .FirstOrDefaultAsync(s => s.SpecimenID == id);

                if (specimen == null) // if that value doesnt exist it will just return the page
                {
                    return View(); 
                }

                var singleSpecimenList = new List<Specimen> { specimen };
                return View("printAll", singleSpecimenList); // Return the printAll view with the single specimen as a list
            }
            else // else if there is no passed in value it will return all the specimens
            {
                var allSpecimens = await _context.Specimen
                    .Include(s => s.MediaList)
                    .ToListAsync();

                return View(allSpecimens); // Return the printAll view with all specimens
            }
        }
        /*
         * The Portal method is used to return a view called feedback
         */
        [Authorize]
        public async Task<IActionResult> Portal()
        {
            // Obtain all feedback objects in the database
            var feedback = await _context.Feedback.ToListAsync();

            /**
             * Overview api calls
             * Pages - Total Page Views
             * Output - User Device Usage
             * Engagement 1 - The total users sorted by date
             * Engagement 2 - The total sessions sorted by date
             */
            
            ViewBag.Pages = callAPI("pagePath", "screenPageViews");
            ViewBag.Output = callAPI("deviceCategory", "totalUsers");
            ViewBag.Engagement = callAPI("date", "totalUsers");
            ViewBag.Engagement2 = callAPI("date", "screenPageViewsPerSession");
            

            /*
             * Tech Overview api calls
             * browser - total users per browser
             * platform - Total users per platform
             * device - total users based on device model
             * screenRes - Total users screen resolution
             * operatingSystem - total users based on operating systems.
             */
            ViewBag.platform = callAPI("platform", "totalUsers");
            ViewBag.browser = callAPI("browser", "totalUsers");
            ViewBag.device = callAPI("deviceModel", "totalUsers");
            ViewBag.screenRes = callAPI("screenResolution", "totalUsers");
            ViewBag.operatingSystem = callAPI("operatingSystem", "totalUsers");

            /*
             * Real Time api calls
             * activeUsers - total active users
             * activeCities - total active cities
             * usersOver7Days - Total users in the past seven days
             */
            ViewBag.activeUsers = callRealTimeReport("country", "activeUsers");
            ViewBag.activeCities = callRealTimeReport("city", "activeUsers");
            ViewBag.Test = callRealTimeReport("minutesAgo", "eventCount");
            ViewBag.UsersOver7Days = callAPIbyDate("dayOfWeekName", "totalUsers", 7);

            return View(feedback);
        }

        [Authorize]
        public async Task<IActionResult> SubmissionIndex(bool? status)
        {
            var userImages = new List<UserImage>();

            if (_context.UserImage == null)
            {
                Problem("Entity set 'StoriesOfTheLandContext.UserImage' is null.");
            }
            else
            {
                // set userImages to a list of the model
                userImages = await _context.UserImage.ToListAsync();
                //order the userImages by ID
                userImages = userImages.OrderBy(x => x.UserImageiD).ToList();

            }

            // filter by status
            if (status != null)
            {
                userImages = userImages.Where(x => x.status.Equals(status)).ToList();
            }



            return View(userImages);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetLearnerMediaStatus(int id, bool status)
        {
            // get the userImage from the id passed in
            var userImage = await _context.UserImage.FindAsync(id);
            if (userImage == null)
            {
                return NotFound();
            }

            // set it's status
            userImage.status = status;

            // update the userImage object and save changes to the database
            _context.Update(userImage);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(SubmissionIndex));
        }

        public async Task<IActionResult> DeleteLearnerSubmission(int id)
        {
            // get the userImafe from the id passed in
            var userImage = await _context.UserImage.FindAsync(id);
            if (userImage == null)
            {
                return NotFound();
            }

            // remove it and save changes
            _context.UserImage.Remove(userImage);
            _context.SaveChanges();

            // delete from the file system if necessary
            if (userImage.MediaPath != null)
            {
                var filePath = userImage.MediaPath;
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction(nameof(SubmissionIndex));
        }

        static string callAPI(string dimensions, string metric)
        {
            var output = new StringBuilder();

            // Set the environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "Controllers/Quickstart-be14e92f89f9.json");

            /**
             * TODO(developer): Uncomment this variable and replace with your
             *  Google Analytics 4 property ID before running the sample.
             */
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
                output.AppendLine($"{row.DimensionValues[0].Value}, {row.MetricValues[0].Value}");
            }
            return output.ToString();
        }


        static string callAPIbyDate(string dimensions, string metric, int date)
        {
            var output = new StringBuilder();

            // Set the environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "Controllers/Quickstart-be14e92f89f9.json");

            /**
             * TODO(developer): Uncomment this variable and replace with your
             *  Google Analytics 4 property ID before running the sample.
             */
            string propertyId = "429176036";

            // Using a default constructor instructs the client to use the credentials
            // specified in GOOGLE_APPLICATION_CREDENTIALS environment variable.
            BetaAnalyticsDataClient client = BetaAnalyticsDataClient.Create();
            DateTime today = DateTime.Today;
            DateTime sevenDaysAgo = today.AddDays(-date);
            string startDate = sevenDaysAgo.ToString("yyyy-MM-dd");
            string endDate = today.ToString("yyyy-MM-dd");
            // Initialize request argument(s)
            RunReportRequest request = new RunReportRequest
            {
                Property = "properties/" + propertyId,
                Dimensions = { new Dimension { Name = dimensions }, },
                Metrics = { new Metric { Name = metric }, },
                DateRanges = { new DateRange { StartDate = startDate, EndDate = endDate }, },
            };


            var response = client.RunReport(request);
            Dictionary<string, int> dayCounts = new Dictionary<string, int>();

            // Initialize counts for all days of the week to 0
            string[] orderedDaysOfWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            foreach (string day in orderedDaysOfWeek)
            {
                dayCounts.Add(day, 0);
            }

            foreach (Row row in response.Rows)
            {
                string dayOfWeek = row.DimensionValues[0].Value;
                int count = Convert.ToInt32(row.MetricValues[0].Value);

                dayCounts[dayOfWeek] = count;
            }

            // Sort the dictionary by custom order of days of the week
            var sortedDayCounts = dayCounts.OrderBy(kv => Array.IndexOf(orderedDaysOfWeek, kv.Key));

            foreach (var kvp in sortedDayCounts)
            {
                output.AppendLine($"{kvp.Key}, {kvp.Value}");
            }

            return output.ToString();
        }

        static string callRealTimeReport(string dimensions, string metric)
        {
            var output = new StringBuilder();

            // Set the environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "Controllers/Quickstart-be14e92f89f9.json");

            /**
             * TODO(developer): Uncomment this variable and replace with your
             *  Google Analytics 4 property ID before running the sample.
             */
            string propertyId = "429176036";

            // Using a default constructor instructs the client to use the credentials
            // specified in GOOGLE_APPLICATION_CREDENTIALS environment variable.
            BetaAnalyticsDataClient client = BetaAnalyticsDataClient.Create();

            // Initialize request argument(s)
            RunRealtimeReportRequest request = new RunRealtimeReportRequest
            {
                Property = "properties/" + propertyId,
                Dimensions = { new Dimension { Name = dimensions }, },
                Metrics = { new Metric { Name = metric }, },
            };

            var response = client.RunRealtimeReport(request);
            foreach (Row row in response.Rows)
            {
                output.AppendLine($"{row.DimensionValues[0].Value}, {row.MetricValues[0].Value}");
            }
            return output.ToString();
        }
    }
}