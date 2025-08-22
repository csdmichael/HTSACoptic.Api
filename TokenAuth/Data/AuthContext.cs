using Microsoft.EntityFrameworkCore;

namespace TokenAuth.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }
    }
}
