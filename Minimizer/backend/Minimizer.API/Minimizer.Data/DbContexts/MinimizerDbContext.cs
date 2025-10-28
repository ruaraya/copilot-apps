using Microsoft.EntityFrameworkCore;
using Minimizer.Entities.Models;

namespace Minimizer.Data.DbContexts
{
    public class MinimizerDbContext : DbContext
    {
        public MinimizerDbContext(DbContextOptions<MinimizerDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasConversion(
                    v => v.ToByteArray(),
                    v => new Guid(v)
                );

            modelBuilder.Entity<Lead>()
                .Property(u => u.Id)
                .HasConversion(
                    v => v.ToByteArray(),
                    v => new Guid(v)
                );

            modelBuilder.Entity<Opportunity>()
                .Property(u => u.Id)
                .HasConversion(
                    v => v.ToByteArray(),
                    v => new Guid(v)
                );

            modelBuilder.Entity<Contact>()
                .Property(u => u.Id)
                .HasConversion(
                    v => v.ToByteArray(),
                    v => new Guid(v)
                );

            modelBuilder.Entity<AuditLog>()
                .Property(u => u.Id)
                .HasConversion(
                    v => v.ToByteArray(),
                    v => new Guid(v)
                );

        }
    }
}
