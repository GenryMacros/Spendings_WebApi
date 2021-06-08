using Spendings.Data.Users;
using Spendings.Data.Records;
using Spendings.Data.Categories;
using Microsoft.EntityFrameworkCore;

namespace Spendings.Data.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }
    }
}
