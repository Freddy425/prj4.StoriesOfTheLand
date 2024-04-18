using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StoriesOfTheLand.Controllers
{
    public class FaqController : Controller
    {
        private readonly StoriesOfTheLandContext _context;

        public FaqController(StoriesOfTheLandContext context)
        {
            _context = context;
        }

        // GET: Faqs
        public async Task<IActionResult> Index(int? id)
        {

            // create a list of faqs
            List<Faq> ListOfFaqs = new List<Faq>();

            //if _context.db is null, problem message entity set faq is null
            if(_context.Faq ==null)
            {
                Problem("Entity set 'StoriesOfTheLandContext.Faq'  is null.");
            }
            else
            {
                //_context.db set to a ListOfFaqs
                ListOfFaqs = await _context.Faq.ToListAsync();
                //order the listoffaqs by Title
                    ListOfFaqs = ListOfFaqs.OrderBy(x=>x.Title).ToList(); ;
               
               

            }
            //return view from Index
            return View(ListOfFaqs);
                         
        }

       

        // GET: Faqs/Create
        
        

        [HttpPost]
        [Authorize]
        public IActionResult Update([FromBody] Faq faq)
        {
            try
            {
                //find if Faq has an Id
                var existingFaq = _context.Faq.Find(faq.Id);
                //if Faq is null return not found
                if (existingFaq == null)
                {
                    return NotFound();
                }
                //check if the existing faq has a title an description
                existingFaq.Title = faq.Title;
                existingFaq.Description = faq.Description;
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

        // GET: Faqs/Edit/5
        

        // POST: Faqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Save(int id, [Bind("Id,Title,Description")] Faq faq)
        {
            //if modelstate has a valid entry for title and description
            if (ModelState.IsValid)
            {

                //if id is 0 or leass
                if (id <=0)
                {

                    //add faq and save changes to _context.db
                    _context.Add(faq);
                    await _context.SaveChangesAsync();
                   

                }

                else { 
                   
                    //otherwise update faq save changes to _context.db
                        _context.Update(faq);
                        await _context.SaveChangesAsync();
                    
                    
                   
                }

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
                        TempData["EditId"] = faq.Id.ToString();
                        TempData["EditTitle"] = faq.Title;
                        TempData["EditDescription"] = faq.Description;
                    }
                }

            }  

            


            //return and redirect to Index page
            return RedirectToAction(nameof(Index));
        }

        // Post: Faqs/Delete/
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            //if existing faq and id are null return not found
            if (id == null || _context.Faq == null)
            {
                return NotFound();
            }
            //set faq to instance of _context.db
            var faq = await _context.Faq
                .FirstOrDefaultAsync(m => m.Id == id);
            //if faq equals null return not found
            if (faq == null)
            {
                return NotFound();
            }
            else { 
            //else remove faq from _context.db and save changes
         
                _context.Faq.Remove(faq);
                _context.SaveChanges();
            }
            //return and redirect to Index
            return RedirectToAction(nameof(Index));
        }

        // POST: Faqs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ///ifcontext.db equals null return problem entity is null
            if (_context.Faq == null)
            {
                return Problem("Entity set 'StoriesOfTheLandContext.Faq'  is null.");
            }
            //set faqid to faq
            var faq = await _context.Faq.FindAsync(id);
            if (faq != null)
            {
                //remove faq from context.db and save changes
                _context.Faq.Remove(faq);
                _context.SaveChanges();
            }
            
            //save context.db
            await _context.SaveChangesAsync();

           //redirect to Index
            return RedirectToAction(nameof(Index));
        }
        //if faq exists return true
        private bool FaqExists(int id)
        {
          return (_context.Faq?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
