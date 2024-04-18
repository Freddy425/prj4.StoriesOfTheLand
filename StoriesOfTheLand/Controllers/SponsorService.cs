using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;

namespace StoriesOfTheLand.Controllers
{
    public class SponsorService
    {
        private readonly StoriesOfTheLandContext _context;

        public SponsorService(StoriesOfTheLandContext context)
        {
            _context = context;
        }

        public async Task<List<Sponsor>> GetSponsorsAsync()
        {
            return await _context.Sponsor.ToListAsync();
        }


      
    }
}
