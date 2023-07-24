using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class APIDbContext : IdentityDbContext<User>
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {}

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public const string ConnectStrring = @"Data Source=localhost;Initial Catalog=Temp;Integrated Security=True;Trusted_Connection=true;TrustServerCertificate=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectStrring);
        }
    }
}
