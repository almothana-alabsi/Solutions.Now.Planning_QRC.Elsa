using Microsoft.EntityFrameworkCore;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models
{
    public class SsoDBContext : DbContext
    {
        public SsoDBContext(DbContextOptions<SsoDBContext> options) : base(options)
        {

        }

        public DbSet<TblUsers> TblUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SSO");
        }
    }
}
