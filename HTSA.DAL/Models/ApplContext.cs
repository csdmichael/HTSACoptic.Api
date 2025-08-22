using Microsoft.EntityFrameworkCore;

namespace HTSA.Models
{
    public class ApplContext : DbContext
    {

        public ApplContext(DbContextOptions<ApplContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        
    }
}