using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions option) : base(option)
        {}

        public DbSet<Product> Products { get; set; }
    }
}
