using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

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
using Microsoft.Extensions.Azure;
using AspNetCore;
using Elfie.Serialization;
using Google.Api;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace StoriesOfTheLand.Controllers
{
    public class SponsorController : Controller
    {
        private readonly StoriesOfTheLandContext _context;


        public SponsorController(StoriesOfTheLandContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the user name, user domain and email of the user from the authentication claims
        /// </summary>
        /// <param name="user">Auth Claims</param>
        /// <returns>Azure AD</returns>



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


            /**/

            return View(ListOfSponsors);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        // GET: localhost:7202/5


        // POST: localhost:7202/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Save([Bind("SponsorID,SponsorName, SponsorURL, SponsorImagePath")] Sponsor sponsor)
        {
            sponsor.SponsorImagePath = "default.png";
            //if modelstate has a valid entry for ID NAme URL Image Path
            if (ModelState.IsValid)
            {

                //if id is 0 or leass


                //add sponsor and save changes to _context.db

                _context.Add(sponsor);
                await _context.SaveChangesAsync();


            }


            else
            {
                //create a dictionary wwith kvp key and errors
                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                                   kvp => kvp.Key,
                                   kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()

                               );

                //for every key with errors
                foreach (string key in errors.Keys)
                {
                    //if they are greater than 0 set tempdata with values
                    if (errors[key].Count() > 0)
                    {
                        TempData[key + "Error"] = errors[key][0];
                        TempData["HasErrors"] = true;
                        TempData["SponsorID"] = sponsor.SponsorID.ToString();
                        TempData["SponsorName"] = sponsor.SponsorName;
                        TempData["SponsorURL"] = sponsor.SponsorURL;
                        TempData["SponsorImagePath"] = sponsor.SponsorImagePath;

                    }
                }

            }






            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> UploadImage(int id, IFormFile uploadedImage)
        {
            var sponsor = await _context.Sponsor.FindAsync(id);

            //if we cant find the image
            if (sponsor == null)
            {
                TempData["ImageError"] = "Sponsor was not found, Unable to upload";
                TempData["SponsorID"] = id.ToString();
                return RedirectToAction(nameof(Index));
            }
            //if the image is valid it will be saved and redirected
            if (uploadedImage != null && uploadedImage.Length > 0 && uploadedImage.ContentType.StartsWith("image/"))
            {
                string filePath = SaveSponsorImage(uploadedImage);
                sponsor.SponsorImagePath = filePath;

                await _context.SaveChangesAsync();

                TempData["Message"] = "Image was saved successfully";

                return RedirectToAction(nameof(Index));
            }

            //Return if errors
            TempData["ImageError"] = "An error occurred while uploading the image. Please ensure the file is an image and try again.";
            TempData["SponsorID"] = id.ToString();
            return RedirectToAction(nameof(Index));
        }
        private string SaveSponsorImage(IFormFile SponsorFile)
        {
            //creates the random fiel nad and attatches the file extentsion to the end of it
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(SponsorFile.FileName);
            //sets the path for the fileName
            var filePath = Path.Combine("wwwroot/images/", fileName);

            //saves the file to wwwroot/media
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                SponsorFile.CopyTo(stream);
            }
            //returns the filename to set it in the Media Object
            return Path.Combine(fileName);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Update([FromBody] Sponsor sponsor)
        {
            try
            {
                //find if Sponsor has an Id
                var existingSponsor = _context.Sponsor.Find(sponsor.SponsorID);
                //if Sponsor is null return not found
                if (existingSponsor == null)
                {
                    return NotFound();
                }
                //check if the existing sponsor has a title an description
                existingSponsor.SponsorName = sponsor.SponsorName;
                existingSponsor.SponsorURL = sponsor.SponsorURL;
                // saving the changes to the _context.db
                _context.SaveChanges();
                //return ok
                return Ok(); // Optionally, you can return additional data or a status
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Internal Server Error");
            }
        }
        // Post: Sponsors/Delete/1
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            //if existing sponsor and id are null return not found
            if (id == null || _context.Sponsor == null)
            {
                return NotFound();
            }
            //set sponsor to instance of _context.db
            var sponsor = await _context.Sponsor
                .FirstOrDefaultAsync(m => m.SponsorID == id);
            //if sponsor equals null return not found
            if (sponsor == null)
            {
                return NotFound();
            }
            else
            {
                //else remove sponsor from _context.db and save changes

                _context.Sponsor.Remove(sponsor);
                _context.SaveChanges();

            }

            string FileName = sponsor.SponsorImagePath;
            string Path = "wwwroot\\images\\" + FileName;
            FileInfo file = new FileInfo(Path);
            if (file.Exists)
            {
                file.Delete();
            }
            //return and redirect to Index
            return RedirectToAction(nameof(Index));
        }

        // POST: localhost:7202/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ///ifcontext.db equals null return problem entity is null
            if (_context.Sponsor == null)
            {
                return Problem("Entity set 'StoriesOfTheLandContext.Faq'  is null.");
            }
            //set sponsorid to sponsor
            var sponsor = await _context.Sponsor.FindAsync(id);
            if (sponsor != null)
            {
                //remove Sponsor from context.db and save changes
                _context.Sponsor.Remove(sponsor);
                _context.SaveChanges();
            }

            //save context.db
            await _context.SaveChangesAsync();

            //redirect to Index
            return RedirectToAction(nameof(Index));
        }
        //if Sponsor exists return true
        private bool SponsorExists(int id)
        {
            return (_context.Sponsor?.Any(e => e.SponsorID == id)).GetValueOrDefault();
        }





    }

}

