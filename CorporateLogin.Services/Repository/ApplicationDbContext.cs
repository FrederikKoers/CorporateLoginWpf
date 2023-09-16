using CorporateLogin.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateLogin.Services.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
