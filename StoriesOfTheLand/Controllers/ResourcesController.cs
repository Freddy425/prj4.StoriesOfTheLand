using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using DeepL;

namespace StoriesOfTheLand.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly StoriesOfTheLandContext _context;

        public ResourcesController(StoriesOfTheLandContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This will delete an resource image and set the image value from that resource to the default
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int id)
        {
            TempData["Message"] = "Image was not found, Unable to delete"; //This message will be replaced if there is no resource,  or in case of success
            var resource = await _context.Resource.FindAsync(id);
            if (resource == null)
            {
                TempData["Message"] = "Resource was not found, Unable to delete";
                return RedirectToAction(nameof(Index));
            }
            if (!string.IsNullOrEmpty(resource.ResourceImage) && resource.ResourceImage != "default.png")
            {

                var filePath = Path.Combine("wwwroot/", resource.ResourceImage.Replace("/", Path.DirectorySeparatorChar.ToString())); 

                // Delete the file
                System.IO.File.Delete(filePath);
                // Set the resource image to the default
                resource.ResourceImage = "images/default.png";
                TempData["Message"] = "Image was deleted successfully, image for resource is set to default";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// This function will upload an image and set it as the image value from that resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uploadedImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(int id, IFormFile uploadedImage)
        {
            var resource = await _context.Resource.FindAsync(id);

            //if we cant find the image
            if (resource == null)
            {
                TempData["ImageError"] = "Resource was not found, Unable to upload";
                TempData["ResourceID"] = id.ToString();
                return RedirectToAction(nameof(Index));
            }
            //if the image is valid it will be saved and redirected
            if (uploadedImage != null && uploadedImage.Length > 0 && uploadedImage.ContentType.StartsWith("image/"))
            {
                string filePath = SaveImageFile(uploadedImage);
                resource.ResourceImage = filePath;
                await _context.SaveChangesAsync();

                TempData["Message"] = "Image was saved successfully";

                return RedirectToAction(nameof(Index));
            }

            //Return if errors
            TempData["ImageError"] = "An error occurred while uploading the image. Please ensure the file is an image and try again.";
            TempData["ResourceID"] = id.ToString();
            return RedirectToAction(nameof(Index));
        }

        //This function saves the image by changing the file name and path
        private string SaveImageFile(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine("wwwroot/images/", fileName); // Ensure this path exists

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            // Return the path relative to wwwroot to store in the database
            return Path.Combine("images/", fileName);
        }


        // GET: Resources
        public async Task<IActionResult> Index()
        {
            var resources = await _context.Resource
                .Include(r => r.FR_Resource) // Also get the FR Resource from the database.
                .ToListAsync();
            return View(resources);
        }

        /// <summary>
        /// This function will create a new resource after validation
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResourceID,ResourceTitle,ResourceDescription,ResourceURL,ResourceImage")] Resource resource)
        {
            //sets the image as default for new created resources
            resource.ResourceImage = "images/default.png";

            //validates the resource
            if (ModelState.IsValid)
            {
                bool isResourceTitleUnique = !_context.Resource.Any(s => s.ResourceTitle == resource.ResourceTitle);
                bool isResourceURLUnique = !_context.Resource.Any(s => s.ResourceURL == resource.ResourceURL);
                //check unique values
                if (!isResourceTitleUnique || !isResourceURLUnique)
                {
                    if (!isResourceURLUnique)
                    {
                        TempData["Error_ResourceURL"] = "The URL must be unique";
                    }
                    if (!isResourceTitleUnique)
                    {
                        TempData["Error_ResourceTitle"] = "The title must be unique";
                    }

                    // Storing form values in TempData to repopulate form fields in case of an error
                    TempData["ErrorType"] = "Create";
                    TempData["ResourceTitle"] = resource.ResourceTitle;
                    TempData["ResourceDescription"] = resource.ResourceDescription;
                    TempData["ResourceURL"] = resource.ResourceURL;

                    return RedirectToAction(nameof(Index));
                }
                try
                {
                    // Attempt to translate the resource title and description to French
                    var frResource = await TranslateResourceToFrenchAsync(resource.ResourceTitle, resource.ResourceDescription);

                    // Set the FR_Resource property if translation succeeds
                    resource.FR_Resource = frResource;

                }
                catch (Exception ex)
                {
                    // if the function fails we dont set up the FR_Resource and give a message.
                    TempData["Message"] = "Failed to translate resource to French. Proceeding with English only.";
                }

                _context.Add(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //validation fails return values and error messages
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var modelStateVal = ModelState[key];
                    var errors = modelStateVal.Errors;
                    var errorMessage = errors.Select(a => a.ErrorMessage).FirstOrDefault();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        TempData[$"Error_{key}"] = errorMessage;
                    }
                }

                // Storing form values in TempData to repopulate form fields in case of an error
                TempData["ErrorType"] = "Create";
                TempData["ResourceTitle"] = resource.ResourceTitle;
                TempData["ResourceDescription"] = resource.ResourceDescription;
                TempData["ResourceURL"] = resource.ResourceURL;

                return RedirectToAction(nameof(Index));
            }
        }


        /// <summary>
        /// This function will get a english title and english description. Will proceed to call the translation API and create a new FR_Resource object
        /// </summary>
        /// <param name="title">English title of the resource</param>
        /// <param name="description">English description of the resource</param>
        /// <returns>FR_Resource object</returns>
        /// <exception cref="InvalidOperationException">Api error</exception>
        public async Task<FR_Resource> TranslateResourceToFrenchAsync(string title, string description)
        {
            var translator = new Translator(Environment.GetEnvironmentVariable("DEEPL_KEY"));

            //because the description is not mandatory we change the call depending on the value
            if (description == null)
            {
                //call the API
                var translations = await translator.TranslateTextAsync(
                    new[] { title }, null, "FR"
                    );


                //Get the values from the response
                var frTitleResult = translations[0].Text;

                //if there is no error we set up the object
                if (frTitleResult != null)
                {
                    return new FR_Resource
                    {
                        FR_ResourceTitle = frTitleResult,
                    };
                }
                else
                {
                    // Handle the case where translation failed
                    throw new InvalidOperationException("Translation failed.");
                }
            }
            else
            {
                //call the API
                var translations = await translator.TranslateTextAsync(
                    new[] { title, description }, null, "FR"
                    );


                //Get the values from the response
                var frTitleResult = translations[0].Text;
                var frDescriptionResult = translations[1].Text;

                //if there is no error we set up the object
                if (frTitleResult != null && frDescriptionResult != null)
                {
                    return new FR_Resource
                    {
                        FR_ResourceTitle = frTitleResult,
                        FR_ResourceDescription = frDescriptionResult
                    };
                }
                else
                {
                    // Handle the case where translation failed
                    throw new InvalidOperationException("Translation failed.");
                }
            }
        }

        /// <summary>
        /// This function will edit a resource after validation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResourceID,ResourceTitle,ResourceDescription,ResourceURL,ResourceImage")] Resource resource)
        {
            if (id != resource.ResourceID)
            {
                TempData["Message"] = "Resource not found. Unable to edit";
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                bool isResourceTitleUnique = !_context.Resource.Any(s => s.ResourceTitle == resource.ResourceTitle && s.ResourceID != resource.ResourceID);
                bool isResourceURLUnique = !_context.Resource.Any(s => s.ResourceURL == resource.ResourceURL && s.ResourceID != resource.ResourceID);

                if (!isResourceTitleUnique || !isResourceURLUnique)
                {
                    TempData["Error_ResourceURL"] = !isResourceURLUnique ? "The URL must be unique" : null;
                    TempData["Error_ResourceTitle"] = !isResourceTitleUnique ? "The title must be unique" : null;

                    TempData["ErrorType"] = "Edit";
                    TempData["ResourceTitle"] = resource.ResourceTitle;
                    TempData["ResourceDescription"] = resource.ResourceDescription;
                    TempData["ResourceURL"] = resource.ResourceURL;
                    TempData["ResourceImage"] = resource.ResourceImage;
                    TempData["ResourceID"] = resource.ResourceID.ToString();

                    return RedirectToAction(nameof(Index));
                }

                try
                {
                    var frResource = await TranslateResourceToFrenchAsync(resource.ResourceTitle, resource.ResourceDescription);

                    var existingFrResource = await _context.FR_Resource.FirstOrDefaultAsync(f => f.ResourceID == id);
                    if (existingFrResource != null)
                    {
                        existingFrResource.FR_ResourceTitle = frResource.FR_ResourceTitle;
                        existingFrResource.FR_ResourceDescription = frResource.FR_ResourceDescription;
                    }
                    else
                    {
                        resource.FR_Resource = frResource;
                    }
                }
                catch (Exception ex)
                {
                    //if the api fails we give the message to the user
                    TempData["Message"] = "Failed to translate resource to French. Proceeding with English only.";

                    //we need to delete the existing fr resource
                    var existingFrResource = await _context.FR_Resource.FirstOrDefaultAsync(f => f.ResourceID == id);
                    if (existingFrResource != null)
                    {
                        _context.FR_Resource.Remove(existingFrResource);
                        await _context.SaveChangesAsync();
                    }

                    // The new fr resource would be null
                    resource.FR_Resource = null;
                }

                _context.Update(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Capture and store model state errors in TempData
                foreach (var key in ModelState.Keys)
                {
                    var modelStateVal = ModelState[key];
                    var errors = modelStateVal.Errors;
                    var errorMessage = errors.Select(a => a.ErrorMessage).FirstOrDefault();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        TempData[$"Error_{key}"] = errorMessage;
                    }
                }

                TempData["ErrorType"] = "Edit";
                TempData["ResourceID"] = resource.ResourceID.ToString();
                TempData["ResourceTitle"] = resource.ResourceTitle;
                TempData["ResourceDescription"] = resource.ResourceDescription;
                TempData["ResourceURL"] = resource.ResourceURL;
                TempData["ResourceImage"] = resource.ResourceImage;

                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// This function will delete a resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Resource == null)
            {
                return Problem("Entity set 'StoriesOfTheLandContext.Resource' is null.");
            }

            // Find the resource by the given id
            var resource = await _context.Resource
                .Include(r => r.FR_Resource) // Ensure you include the FR_Resource in the query
                .FirstOrDefaultAsync(r => r.ResourceID == id);

            if (resource != null)
            {
                // If there's an associated FR_Resource, remove it
                if (resource.FR_Resource != null)
                {
                    _context.FR_Resource.Remove(resource.FR_Resource);
                }
                //delete images that are not the default.
                if (resource.ResourceImage != "images/default.png")
                {
                    var filePath = Path.Combine("wwwroot/", resource.ResourceImage.Replace("/", Path.DirectorySeparatorChar.ToString()));

                    // Delete the file
                    System.IO.File.Delete(filePath);
                }
                // Then remove the Resource
                _context.Resource.Remove(resource);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Resource was deleted successfully.";
            }
            else
            {
                TempData["Message"] = "Resource not found. Unable to delete.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ResourceExists(int id)
        {
            return (_context.Resource?.Any(e => e.ResourceID == id)).GetValueOrDefault();
        }
    }
}
