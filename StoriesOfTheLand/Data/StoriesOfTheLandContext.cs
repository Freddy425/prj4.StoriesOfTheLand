using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Models;

namespace StoriesOfTheLand.Data
{
    public class StoriesOfTheLandContext : DbContext
    {
        public StoriesOfTheLandContext (DbContextOptions<StoriesOfTheLandContext> options)
            : base(options)
        {
        }

        public DbSet<StoriesOfTheLand.Models.Specimen> Specimen { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.Sponsor> Sponsor { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.Feedback> Feedback { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.Media> Media { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.Faq> Faq { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.Resource> Resource { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.UserImage> UserImage { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.FR_Resource> FR_Resource { get; set; } = default!;

        public DbSet<StoriesOfTheLand.Models.FR_Specimen> FR_Specimen { get; set; } = default!;
    }
}
