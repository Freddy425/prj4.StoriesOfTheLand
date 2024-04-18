using AspNetCore;
using Google.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StoriesOfTheLand.Controllers;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace StoriesOfTheLand.Views.Shared.Components.Footer
{
    public class FooterViewComponent : ViewComponent
    {


        private readonly StoriesOfTheLandContext _db;

        public FooterViewComponent(StoriesOfTheLandContext context) => _db = context;
        

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var items = await GetItemsAsync();
            return View("Default", items);

        }

        public Task<List<Sponsor>> GetItemsAsync()
        {
            return _db!.Sponsor.ToListAsync();
        }
    }
}
