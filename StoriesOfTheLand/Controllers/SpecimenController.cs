
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using DeepL;
using RestSharp;


namespace StoriesOfTheLand.Controllers
{
    public class SpecimenController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StoriesOfTheLandContext _context;

        public SpecimenController(StoriesOfTheLandContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This function will translate a specimen by performing a call to the Deepl API
        /// </summary>
        /// <param name="englishName">english name of the specimen</param>
        /// <param name="description">english description of the specimen</param>
        /// <param name="culturalSignificance">english significance of the specimen</param>
        /// <returns> a translated FR_Specimen </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<FR_Specimen> TranslateSpecimenToFrenchAsync(string englishName, string description, string culturalSignificance)
        {
            var translator = new Translator(Environment.GetEnvironmentVariable("DEEPL_KEY"));

            //call the API
            var translations = await translator.TranslateTextAsync(
                new[] { englishName, description, culturalSignificance }, null, "FR"
                );

            //Get the values from the response
            var frNameResult = translations[0].Text;
            var frDescriptionResult = translations[1].Text;
            var frSignificanceResult = translations[2].Text;

            //if there is no error we set up the object
            if (frNameResult != null && frDescriptionResult != null && frSignificanceResult != null)
            {
                return new FR_Specimen
                {
                    FR_EnglishName = frNameResult,
                    FR_CulturalSignificance = frSignificanceResult,
                    FR_SpecimenDescription = frDescriptionResult
                };
            }
            else
            {
                // Handle the case where translation failed
                throw new InvalidOperationException("Translation failed.");
            }
        }

        /// <summary>
        /// This action will delete a specimen by its id
        /// </summary>
        /// <param name="id">SpecimenID</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specimen = await _context.Specimen
                                    .Include(s => s.FR_Specimen) // Include the FR_Specimen in the query to load it
                                    .FirstOrDefaultAsync(s => s.SpecimenID == id);
            if (specimen != null)
            {
                // If there's an associated FR_Specimen, remove it
                if (specimen.FR_Specimen != null)
                {
                    _context.FR_Specimen.Remove(specimen.FR_Specimen);
                }

                _context.Specimen.Remove(specimen);
                await _context.SaveChangesAsync();

                // Redirect with a query string parameter to trigger an alert
                return RedirectToAction(nameof(Index), new { deletionSuccess = true });
            }
            return RedirectToAction(nameof(Index));
        }
        private bool SpecimenExists(int id)
        {
            return (_context.Specimen?.Any(e => e.SpecimenID == id)).GetValueOrDefault();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecimenID,LatinName,SpecimenDescription,EnglishName,CreeName,CulturalSignificance,Latitude,Longitude")] Specimen specimen)
        {
            if (id != specimen.SpecimenID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingSpecimen = await _context.Specimen
                    .Include(s => s.MediaList)
                    .Include(s => s.FR_Specimen)
                    .FirstOrDefaultAsync(s => s.SpecimenID == id);
                if (existingSpecimen == null)
                {
                    return NotFound();
                }

                existingSpecimen.LatinName = specimen.LatinName;
                existingSpecimen.SpecimenDescription = specimen.SpecimenDescription;
                existingSpecimen.EnglishName = specimen.EnglishName;
                existingSpecimen.CreeName = specimen.CreeName;
                existingSpecimen.CulturalSignificance = specimen.CulturalSignificance;
                existingSpecimen.Latitude = specimen.Latitude;
                existingSpecimen.Longitude = specimen.Longitude;

                try
                {
                    // Attempt to translate the updated specimen information to French
                    var frSpecimen = await TranslateSpecimenToFrenchAsync(specimen.EnglishName, specimen.SpecimenDescription, specimen.CulturalSignificance);

                    // Check if an FR_Specimen already exists and update it; otherwise, create a new FR_Specimen
                    if (existingSpecimen.FR_Specimen != null)
                    {
                        existingSpecimen.FR_Specimen.FR_EnglishName = frSpecimen.FR_EnglishName;
                        existingSpecimen.FR_Specimen.FR_SpecimenDescription = frSpecimen.FR_SpecimenDescription;
                        existingSpecimen.FR_Specimen.FR_CulturalSignificance = frSpecimen.FR_CulturalSignificance;
                    }
                    else
                    {
                        existingSpecimen.FR_Specimen = frSpecimen;
                    }
                }
                catch (Exception ex)
                {
                    // if the function fails we dont set up the FR_Specimen and give a message.

                    existingSpecimen.FR_Specimen = null;

                    TempData["Message"] = "Failed to translate specimen to French. Proceeding with English only.";
                }

                await _context.SaveChangesAsync();

                // Redirect to the Details view of the edited specimen
                return RedirectToAction(nameof(Index));
            }


            // get the errors and put them on a Json Tempdata
            var errors = ModelState.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );

            var response = new
            {
                Errors = errors,
                Values = new
                {
                    specimen.SpecimenID,
                    specimen.EnglishName,
                    specimen.LatinName,
                    specimen.CreeName,
                    specimen.SpecimenDescription,
                    specimen.CulturalSignificance,
                    specimen.Latitude,
                    specimen.Longitude,
                }
            };

            TempData["EditResponse"] = JsonConvert.SerializeObject(response);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// This function will return the specimen info by looking by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //Get: Specimens/Edit
        public IActionResult GetSpecimenById(int id)
        {
            var specimen = _context.Specimen.Find(id);
            if (specimen == null)
            {
                return NotFound();
            }

            _context.Entry(specimen)
            .Collection(s => s.MediaList)
            .Load();

            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // this is to not get a cyclic loop error
            };

            var result = new Dictionary<string, object>
            {
                { "specimenId", specimen.SpecimenID },
                { "englishName", specimen.EnglishName },
                { "latinName", specimen.LatinName },
                { "creeName", specimen.CreeName },
                { "specimenDescription", specimen.SpecimenDescription },
                { "culturalSignificance", specimen.CulturalSignificance },
                { "latitude", specimen.Latitude },
                { "longitude", specimen.Longitude },
            };

            if (specimen.MediaList != null)
            {

                var mediaList = specimen.MediaList.Select(media => new
                {
                    mediaId = media.Id,
                    mediaSpecId = media.SpecimenID,
                    mediaType = media.MediaType,
                    mediaPath = media.MediaPath,
                }).ToList();

                result["mediaList"] = mediaList;

            }
            else
            {
                result["mediaList"] = null;
            }



            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result);

            return Content(jsonResult, "application/json");
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecimenID,LatinName,SpecimenDescription,EnglishName,CreeName,CulturalSignificance,SpecimenMedia,Latitude,Longitude")] Specimen specimen)
        {
            //if the model is valid
            if (ModelState.IsValid)
            {
                // Check for uniqueness of EnglishName, LatinName, and optionally CreeName if not empty
                bool isEnglishNameUnique = !_context.Specimen.Any(s => s.EnglishName == specimen.EnglishName);
                bool isLatinNameUnique = !_context.Specimen.Any(s => s.LatinName == specimen.LatinName);
                bool isCreeNameUnique = string.IsNullOrEmpty(specimen.CreeName) || !_context.Specimen.Any(s => s.CreeName == specimen.CreeName);

                if (!isEnglishNameUnique || !isLatinNameUnique || !isCreeNameUnique)
                {
                    var errors = new Dictionary<string, string[]>();
                    if (!isEnglishNameUnique)
                    {
                        errors.Add("EnglishName", new[] { "The English name must be unique." });
                    }
                    if (!isLatinNameUnique)
                    {
                        errors.Add("LatinName", new[] { "The Latin name must be unique." });
                    }
                    if (!isCreeNameUnique && !string.IsNullOrEmpty(specimen.CreeName))
                    {
                        errors.Add("CreeName", new[] { "The Cree name must be unique." });
                    }

                    var response = new
                    {
                        Errors = errors,
                        Values = new
                        {
                            specimen.EnglishName,
                            specimen.LatinName,
                            specimen.CreeName,
                            specimen.SpecimenDescription,
                            specimen.CulturalSignificance,
                            specimen.Latitude,
                            specimen.Longitude
                        }
                    };

                    TempData["CreateResponse"] = JsonConvert.SerializeObject(response);
                    //add some progress cookie stufff to response
                    return RedirectToAction(nameof(Index));
                }
                //if the names are unique we proceed to translate the specimen
                try
                {
                    // Attempt to translate the specimen's information to French
                    var frSpecimen = await TranslateSpecimenToFrenchAsync(specimen.EnglishName, specimen.SpecimenDescription, specimen.CulturalSignificance);

                    // Associate the translated specimen with the original specimen
                    specimen.FR_Specimen = frSpecimen;
                }
                catch (Exception ex)
                {
                    // if the function fails we dont set up the FR_Specimen and give a message.
                    TempData["Message"] = "Failed to translate specimen to French. Proceeding with English only.";
                }
                _context.Add(specimen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = specimen.SpecimenID });
            }
            else
            {
                var errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var response = new
                {
                    Errors = errors,
                    Values = new
                    {
                        specimen.EnglishName,
                        specimen.LatinName,
                        specimen.CreeName,
                        specimen.SpecimenDescription,
                        specimen.CulturalSignificance,
                        specimen.Latitude,
                        specimen.Longitude
                    }
                };

                TempData["CreateResponse"] = JsonConvert.SerializeObject(response);
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: Specimen/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // DELETE: Specimen/DeleteMedia/5
        // For deleting a single media object attached to a specimen
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteMedia(int id, string mediaPath)
        {
            // get the specimen with the passed-in ID
            var specimen = await _context.Specimen.Include(s => s.MediaList)
                .FirstOrDefaultAsync(s => s.SpecimenID == id);

            if (specimen == null)
            {
                return NotFound();
            }

            // get the media with the passed-in path
            var mediaToDelete = specimen.MediaList.FirstOrDefault(m => m.MediaPath == mediaPath);

            if (mediaToDelete == null)
            {
                return NotFound();
            }

            // Remove the media from the specimen's
            specimen.MediaList.Remove(mediaToDelete);

            // save changes
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Failed to delete media.");
            }
        }


        // GET: Specimens/Details/5
        //Method gets details for a specimen based in the SpecimenID and returns a 
        //corresponding veiw
        // int? fs is passed in as '1' if feedback was submitted

        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> Details(int? id, int? fA)
        {
            //checks to see if the id is null or the specimen context is null
            if (id == null || _context.Specimen == null)
            {
                // Returns not found
                return NotFound();
            }

            // checks to see if a success message is passed in
            if (fA != null)
            {
                if (fA == 1)
                {
                    ViewBag.SuccessMessage = "Thank you for submitting feedback.";
                }
            }

            //checks the database for the specimen object with the given id
            var specimen = await _context.Specimen
                .Include(s => s.MediaList)
                .Include(s => s.FR_Specimen)
                .FirstOrDefaultAsync(m => m.SpecimenID == id);

            if (specimen == null)
            {
                // Returns not found
                return NotFound();
            }

            //retrieve session data
            var specimenProgress = HttpContext.Session.GetString("SpecimenProgress");
            List<int> specimenProgressList;
            if (specimenProgress != null)
            {
                specimenProgressList = JsonConvert.DeserializeObject<List<int>>(specimenProgress) ?? new List<int>();

                // Add the current specimen ID to the list if it's not already there
                if (!specimenProgressList.Contains(specimen.SpecimenID))
                {
                    specimenProgressList.Add(specimen.SpecimenID);
                }
                
            }
            else
            {
                // Initialize an empty list if the session data is null or empty
                specimenProgressList = new List<int>
                {
                    //add specimen

                    specimen.SpecimenID
                };
                
            }
            // Save the updated list back to session
            HttpContext.Session.SetString("SpecimenProgress", JsonConvert.SerializeObject(specimenProgressList));

            var specimens = from s in _context.Specimen select s;
            List<Specimen> specimenList = specimens.ToList();

            TempData["SpecimenList"] = specimenList;

            // Render's the specimen's details.cshtml file
            return View(specimen);
        }

    



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(AddMediaViewModel mediaView, int? id)
        {
            //gets the specimen from the id in the url
            var specimen = await _context.Specimen
                .Include(s => s.MediaList)
                .Include(s => s.FR_Specimen)
                .FirstOrDefaultAsync(s => s.SpecimenID == id);

            //check to make sure the specimen exists
            if (specimen == null)
            {
                return NotFound();
            }

            //if there are no validation issues
            if (ModelState.IsValid)
            {
                //for every IFormFile in MediaFile
                for (int i = 0; i < mediaView.MediaFile.Count; i++)
                {
                    //get the file type that was entered
                    string fileType = Path.GetExtension(mediaView.MediaFile.ElementAt(i).FileName);
                    string mediaType = "";
                    //called SaveMediaFile() which saves the media into wwwroot/media
                    var mediaPath = SaveMediaFile(mediaView.MediaFile.ElementAt(i));

                    // sets the mediaType of media object based on the file extentsion
                    mediaType = (fileType == ".png" || fileType == ".jpeg") ? "Image" :
                    (fileType == ".m4a" || fileType == ".mp3") ? "Audio" :
                     "Unknown";

                    //creates a new media with the media path and mediaType
                    var media = new Media
                    {
                        MediaType = mediaType,
                        MediaPath = mediaPath,
                    };
                    //adds the media to the MediaList
                    specimen.MediaList.Add(media);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                // Redirect back to the Details view
                //return RedirectToAction(nameof(Details), new { id });
                return View("Details", specimen);
            }

            //retrieve session data
            var specimenProgress = HttpContext.Session.GetString("SpecimenProgress");
            List<int> specimenProgressList;
            if (specimenProgress != null)
            {
                specimenProgressList = JsonConvert.DeserializeObject<List<int>>(specimenProgress) ?? new List<int>();

                // Add the current specimen ID to the list if it's not already there
                if (!specimenProgressList.Contains(specimen.SpecimenID))
                {
                    specimenProgressList.Add(specimen.SpecimenID);
                }

            }
            else
            {
                // Initialize an empty list if the session data is null or empty
                specimenProgressList = new List<int>
                {
                    //add specimen

                    specimen.SpecimenID
                };

            }
            // Save the updated list back to session
            HttpContext.Session.SetString("SpecimenProgress", JsonConvert.SerializeObject(specimenProgressList));




            var specimens = from s in _context.Specimen select s;
            List<Specimen> specimenList = specimens.ToList();

            TempData["SpecimenList"] = specimenList;

            // If ModelState is not valid, return to the Details view with the current data
            ViewData["ErrorMessage"] = "There are validation errors. Please check the form.";
            return View("Details", specimen);

        }

        private string SaveMediaFile(IFormFile mediaFile)
        {
            //creates the random fiel nad and attatches the file extentsion to the end of it
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mediaFile.FileName);
            //sets the path for the fileName
            var filePath = Path.Combine("wwwroot/media", fileName);

            //saves the file to wwwroot/media
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                mediaFile.CopyTo(stream);
            }
            //returns the filename to set it in the Media Object
            return fileName;
        }

        // GET: Specimen/
        // This method corresponds with the Index.cshtml page
        // Passing in Search String enables the user to search the specimen index.
        public async Task<IActionResult> Index()
        {

            // Check to see if there are NO specimen inside of the list
            if (_context.Specimen == null)
            {
                // Return the problem object, stating that there are no specimen inside of the database
                return Problem("Entity set 'StoriesOfTheLandContext.Specimen' is null");
            }

            // Obtain the list of Specimen from the context
            var specimens = from s in _context.Specimen
                            .Include(s => s.FR_Specimen)
                            select s;

            List<Specimen> specimenList = specimens.ToList();



            //verify progress 
            // Retrieve the list of visited specimen IDs from session
            var specimenProgress = HttpContext.Session.GetString("SpecimenProgress") ?? "";

            List<int> specimenProgressList;
            if (string.IsNullOrEmpty(specimenProgress))
            {
                // Initialize an empty list if the session data is null or empty
                specimenProgressList = new List<int>();
            }
            else
            {
                // Deserialize the session data into a list of integers
                specimenProgressList = JsonConvert.DeserializeObject<List<int>>(specimenProgress);
            }

            // Check if the IDs in the list are still present in the database
            foreach (var id in specimenProgressList.ToList())
            {
                if (!_context.Specimen.Any(s => s.SpecimenID == id))
                {
                    // Remove the ID from the list if it's no longer present in the database
                    specimenProgressList.Remove(id);
                }
            }

            // Save the updated list back to session
            HttpContext.Session.SetString("SpecimenProgress", JsonConvert.SerializeObject(specimenProgressList));

            TempData["SpecimenList"] = specimenList;
            

            // Returns a View of the Specimen
            return View(await specimens.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Map()
        {
            var specimens = from s in _context.Specimen select s;
            // Returns a View of the Specimen
            return View(await specimens.Include(s => s.MediaList).ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLocation(int id, [Bind("Latitude,Longitude")] Specimen postedSpecimen)
        {

            var specimen = await _context.Specimen.FindAsync(id);

            if (specimen == null)
            {
                return NotFound();
            }

            //only updating the location

            specimen.Latitude = postedSpecimen.Latitude;
            specimen.Longitude = postedSpecimen.Longitude;

            //validation checker
            List<ValidationResult> results = new List<ValidationResult>();
            var vc = new ValidationContext(specimen, null, null);

            Validator.TryValidateObject(specimen, vc, results, true);

            if (specimen is IValidatableObject) (specimen as IValidatableObject).Validate(vc);

            if (results.Count() == 0)
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                foreach (ValidationResult result in results)
                {
                    TempData[result.MemberNames.First() + "Error"] = result.ErrorMessage;
                    //available as tempdata lat or long error on my razer
                }
                TempData["Specimenid"] = id.ToString();
                TempData["Lat"] = postedSpecimen.Latitude.ToString();
                TempData["Long"] = postedSpecimen.Longitude.ToString();
            }
            // Redirect to the Map action
            return RedirectToAction("Map");
        }

        [HttpPost, ActionName("DeleteLocation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLocation(int id, [Bind("SpecimenID,LatinName,SpecimenDescription,EnglishName,CreeName,CulturalSignificance,Latitude,Longitude")] Specimen specimen)
        {
            //var specimen = await _context.Specimen.FindAsync(id);
            if (specimen != null)
            {

                //not actually "removing" anything just setting values to null
                /*existingSpecimen.Latitude = null;
                existingSpecimen.Longitude = null;*/

                var existingSpecimen = await _context.Specimen.FindAsync(id);
                if (existingSpecimen == null)
                {
                    return NotFound();
                }
                //only updating the location
                //not actually "removing" anything just setting values to null
                existingSpecimen.Latitude = null;
                existingSpecimen.Longitude = null;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecimenExists(specimen.SpecimenID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


            }
            // Redirect to the Map view
            return RedirectToAction("Map", new { deletionSuccess = true });

        }

    }

}



