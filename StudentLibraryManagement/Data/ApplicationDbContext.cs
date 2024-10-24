using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentLibraryManagement.Models;

namespace StudentLibraryManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Books> Books { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
