using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using System.Diagnostics;
using System.Security.Claims;
using StoriesOfTheLand.Controllers;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.AspNetCore.Authorization;
using Google.Analytics.Data.V1Beta;
using System.Text;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace StoriesOfTheLand.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly StoriesOfTheLandContext _context;
        private readonly IAnalyticsDataClient _analyticsDataClient;

        public HomeController(StoriesOfTheLandContext context, IAnalyticsDataClient analyticsDataClient)
        {
            _context = context;
                
        _analyticsDataClient = analyticsDataClient;
    }

        /// <summary>
        /// Get the user name, user domain and email of the user from the authentication claims
        /// </summary>
        /// <param name="user">Auth Claims</param>
        /// <returns>Azure AD</returns>
        public static UserAzureAD GetUserOnAzureAd(ClaimsPrincipal user)
        {
            var preferredUsernameClaim = user.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"));
            if (preferredUsernameClaim != null)
            {
                return new UserAzureAD
                {
                    user_name = user.Claims.FirstOrDefault(p => p.Type.Equals("name")).Value,
                    user_email = preferredUsernameClaim.Value,
                    user_domain = string.Format(@"cpiccr\{0}", preferredUsernameClaim.Value.Split('@')[0])
                };
            }
            return null;
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(IFormFile mediaView)
        {
            // Grab all the sponsors
            var sponsor = await _context.Sponsor.ToListAsync();
            // Check if the image is null
            if (mediaView != null)
            {
                // Check File Size so that its only less than 1MB
                if (mediaView.Length > 1048576)
                {
                    // Display an error message if the file size is too large
                    TempData["LearnerImageError"] = "The file size should not exceed 1024 KB.";

                    // Redirect back to the Index page
                    return RedirectToAction(nameof(Index));
                }
                // Check File Type so that its only jpeg, jpg, or png
                if (mediaView.ContentType != "image/jpeg" && mediaView.ContentType != "image/jpg" && mediaView.ContentType != "image/png")
                {
                    // Display an error message if the file type is not supported
                    TempData["LearnerImageError"] = "File must be of type png, jpg or jpeg";

                    // Redirect back to the Index page
                    return RedirectToAction(nameof(Index));
                }
                // Create a new User Image object
                var userImage = new UserImage
                {
                    // Use Request object to obtain the RemoteIPAddress
                    IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    // Convert the media length to an integer
                    FileSize = Convert.ToInt32(mediaView.Length),
                    // Set the media path
                    MediaPath = SaveLearnerMedia(mediaView)
                };
                // Save it to the database
                _context.UserImage.Add(userImage);
                await _context.SaveChangesAsync();

                // Display a success message
                ViewData["success"] = "Your image was uploaded successfully";
            }
            // If the image is null
            if (mediaView == null)
            {
                // Display an error message
                TempData["LearnerImageError"] = "Please upload an image";
                return RedirectToAction(nameof(Index));
            }

            //call the api
            var apiResult = _analyticsDataClient.callAPI("pagePath", "screenPageViews");




            ///call the function to clean the result
            ///
            var cleanedAPI = organizedMostViewed(apiResult);


            List<Specimen> collection = new List<Specimen>();
            //call the context to get the specimen names
            foreach (var id in cleanedAPI)
            {

                var names = _context.Specimen.FirstOrDefault(e => e.SpecimenID == id);
                if (names != null)

                {
                    collection.Add(names);
                }
            }


            TempData["topTen"] = collection;

            return _context.Sponsor != null ?
              View(await _context.Sponsor.ToListAsync()) :
              Problem("Entity set 'StorisOfTheLandContext.Specimen'  is null.");
        }

        public async Task<IActionResult> Index()
        {

            List<Sponsor> ListOfSponsors = new List<Sponsor>();
            if (_context.Sponsor == null)
            {
                Problem("Entity set 'StorisOfTheLandContext.Specimen'  is null.");
            }
            else
            {
                ListOfSponsors = await _context.Sponsor.ToListAsync();

            }



            //call the api
            var apiResult = _analyticsDataClient.callAPI("pagePath", "screenPageViews");




            ///call the function to clean the result
            ///
            var cleanedAPI = organizedMostViewed(apiResult);
            

            List<Specimen> collection = new List<Specimen>();
            //call the context to get the specimen names
            foreach (var id in cleanedAPI)
            {

                var names = _context.Specimen.FirstOrDefault(e => e.SpecimenID == id);
                if (names != null)

                {
                    collection.Add(names);
                }
            }
           

            TempData["topTen"]=collection;
            //put the in a viewvag or tempdata


            return View(ListOfSponsors);

        }


        private string SaveLearnerMedia(IFormFile imageFile)
        {
            // Create a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            // Combine the directory path and file name
            var filePath = Path.Combine("wwwroot/media/submissions/", fileName);
            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }


            // Return the normalized file path
            return filePath;
        }
     

       
        public static List<int> organizedMostViewed(List<(string Dimension, string Metric)> resultAPI)
        {
            var topSpecimens = resultAPI
            .Where(pair => pair.Dimension.Contains("/Specimen/Details/"))
            .Select(pair => {
                bool isNumeric = int.TryParse(pair.Metric, out int metricValue);
                return new { pair.Dimension, MetricValue = isNumeric ? metricValue : 0 };
            })
            .OrderByDescending(pair => pair.MetricValue)
            .Take(10) // Only take the top 10
            .Select(pair => (pair.Dimension, pair.MetricValue.ToString()))
            .ToList();

            List<int> specimenList = new List<int>();

            foreach (var pair in topSpecimens)
            {
                // Extracting the number from the Dimension string
                string dimension = pair.Dimension;
                int startIndex = dimension.LastIndexOf('/') + 1;
                int length = dimension.Length - startIndex;
                string numberString = dimension.Substring(startIndex, length);

                // Parsing the number string to an integer
                if (int.TryParse(numberString, out int specimenNumber))
                {
                    // Adding the parsed number to the list of integers
                    specimenList.Add(specimenNumber);
                }
                else
                {
                    // Handling the case where parsing fails
                    Console.WriteLine($"Failed to parse number from dimension: {pair.Dimension}");
                }
            }

            return specimenList;
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // action for retrieving the media list to display on the page
        public IActionResult GetLearnerMediaList()
        {
            var userImages = new List<UserImage>();

            if (_context.UserImage == null)
            {
                Problem("Entity set 'StoriesOfTheLandContext.UserImage' is null.");
            }
            else
            {
                // set userImages to a list of the UserImage model that are approved
                userImages = _context.UserImage.Where(x => x.status.Equals(true)).ToList();
                //order the userImages by ID
                userImages = userImages.OrderBy(x => x.UserImageiD).ToList();
            }


            var jsonResult = System.Text.Json.JsonSerializer.Serialize(userImages);

            return Content(jsonResult, "application/json");
        }

    }
}
