using Microsoft.EntityFrameworkCore;
using PIMSystem.Core.Domain.Entities;

namespace PIMSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}