using Microsoft.EntityFrameworkCore;

namespace HTSA.Models
{
    public class CPContext : DbContext
    {
        public CPContext(DbContextOptions<CPContext> options)
        : base(options)
        { }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


    }
}