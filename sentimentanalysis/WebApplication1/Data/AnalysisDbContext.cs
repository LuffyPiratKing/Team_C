using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class AnalysisDbContext : DbContext
    {
        public AnalysisDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Products> products { get; set; }
        public DbSet<Review> reviews { get; set; }
    }
}