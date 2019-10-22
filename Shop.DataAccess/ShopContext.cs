using Microsoft.EntityFrameworkCore;
using Shop.Domain;

namespace Shop.DataAccess
{
    public class ShopContext : DbContext
    {
        private readonly string connectionString;

        public ShopContext(string connectionString )
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        // и точно такие же другие сущности

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }



    }
}
