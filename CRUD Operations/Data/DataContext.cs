using CRUD_Operations.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operations.Data
{
    public class DataContext : DbContext
    {
        public DataContext ( DbContextOptions<DataContext> options ) : base ( options ) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            // Configure relationships
            modelBuilder.Entity<User> ()
                .HasOne ( u => u.Role )
                .WithMany ( r => r.Users )
                .HasForeignKey ( u => u.RoleId );
        }
    }
}