using Microsoft.EntityFrameworkCore;
using Image.WebAPI.Data;

namespace Image.WebAPI.Data
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options) : base(options)
        {
        }

        public DbSet<ImageDetail> Details { get; set; }

        // Other DbSet properties and configuration...

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration for entities

            base.OnModelCreating(modelBuilder);
        }
    }
}
