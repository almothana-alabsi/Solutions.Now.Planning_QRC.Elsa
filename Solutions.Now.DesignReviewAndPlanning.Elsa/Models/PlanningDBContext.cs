using Microsoft.EntityFrameworkCore;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models
{
    public class PlanningDBContext : DbContext
    {
        public PlanningDBContext( DbContextOptions<PlanningDBContext> options) :base(options)
        {

        }


        public DbSet<ApprovalHistory> ApprovalHistory { get; set; }
        public DbSet<WorkFlowRules> WorkFlowRules { get; set; }
        public DbSet<MainProject_Planning> MainProject_Planning { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Plan");
        }
    }
}
