using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Models
{
    public class WeddingPlannerContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext> options) : base(options) { }
        
        public DbSet<User> Users {get; set;}

        public DbSet<Wedding> Weddings {get; set;}

        public DbSet<WeddingConfirmation> WeddingConfirmations {get; set;}
    }
}