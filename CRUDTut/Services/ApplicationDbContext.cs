using CRUDTut.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDTut.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions options) : base (options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
