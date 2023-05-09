using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snackis.Models;

namespace Snackis.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Snackis.Models.Post> Post { get; set; } = default!;
        public DbSet<Snackis.Models.Subject> Subject { get; set; } = default!;
    }
}