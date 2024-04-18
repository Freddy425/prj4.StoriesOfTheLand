using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using Microsoft.AspNetCore.Authorization;



namespace StoriesOfTheLand.Controllers
{
    // https://stackoverflow.com/questions/41112564/asp-net-core-disable-authentication-in-development-environment
    /// <summary>
    /// This authorisation handler will bypass all requirements
    /// </summary>
    public class AllowAnonymous : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
                context.Succeed(requirement); //Simply pass all requirements

            return Task.CompletedTask;
        }
    }
    // Note that this class was generated automatically by scaffolding
    public class FeedbackController : Controller
    {
        private readonly StoriesOfTheLandContext _context;

        public FeedbackController(StoriesOfTheLandContext context)
        {
            _context = context;
        }
        // GET Feedback/Index
        [Authorize]
        public async Task<IActionResult> Index()
        {
           
            var feedbackWithSpecimen = await _context.Feedback
         .OrderByDescending(f => f.FeedbackID)
         .Select(f => new
         {
             f.FeedbackID,f.CreateDate,f.Name,f.Subject,f.SpecimenID,f.Email, f.Details,f.Status,
             SpecimenEnglishName = _context.Specimen.FirstOrDefault(s => s.SpecimenID == f.SpecimenID).EnglishName
         })
         .ToListAsync();

            return View(feedbackWithSpecimen);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            Console.WriteLine(id);
            //if the context.Feedback is null will fail
            if (_context.Feedback == null)
            {
                return Problem("Entity set 'StoriesOfTheLandContext.Feedback'  is null.");
            }
            //gets the feedback with the passed in id
            var feedback = await _context.Feedback.FindAsync(id);
            //if the passed in feeback 
            if (feedback != null)
            {
                _context.Feedback.Remove(feedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       [Authorize]
        public async Task<IActionResult> UpdateStatus(int feedbackId, int status)
        {
            var feedback = await _context.Feedback.FindAsync(feedbackId);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Status = (Status)status;
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var vc = new System.ComponentModel.DataAnnotations.ValidationContext(feedback, null, null);

            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(feedback, vc, results, true);
            if(results.Count==0)
            {
                _context.Update(feedback);
                await _context.SaveChangesAsync();
            }
            return Redirect(nameof(Index));

        }


        //// POST: Feedback/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<IActionResult> Edit(int id, [Bind("FeedbackID,Name,Email,Subject,SpecimenID,Details,Status")] Feedback feedback)
        //{
        //    if (id != feedback.FeedbackID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(feedback);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FeedbackExists(feedback.FeedbackID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(feedback);
        //}
       
        // GET: Feedback/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Feedback == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .FirstOrDefaultAsync(m => m.FeedbackID == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedback/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedback/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> Create([Bind("FeedbackID,Name,Email,Subject,SpecimenID,Details")] Feedback feedback)
        {
            // Ensures the SpecimenID is not null (but can be 0 to indicate feedback not linked)
            if (feedback.SpecimenID == null)
            {
                feedback.SpecimenID = 0;
            }

            // Checks if the model that has been submitted is valid
            if (ModelState.IsValid)
            {
                // If it is, it adds it to the database context
                _context.Add(feedback);
                // Waits for the context to confirm 
                await _context.SaveChangesAsync();

                // If the feedback object has a specimenID attached to it
                if (feedback.SpecimenID != 0)
                {
                    // Redirect to that specimen's page
                    return RedirectToAction("Details", "Specimen", new { id = feedback.SpecimenID, fA = 1 });
                }
                // Redirect back to home
                return RedirectToAction("Index", "Home", new { fA = 1 });
            }
            // If not, stays on the feedback page (showing errors where applicable)
            return View(feedback);
        }

       
        private bool FeedbackExists(int id)
        {
          return (_context.Feedback?.Any(e => e.FeedbackID == id)).GetValueOrDefault();
        }
    }
}
