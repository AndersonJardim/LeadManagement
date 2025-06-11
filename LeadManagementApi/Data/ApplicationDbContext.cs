using LeadManagementApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Lead> Leads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lead>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ContactFirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Suburb).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ContactFullName).HasMaxLength(200);
                entity.Property(e => e.ContactPhoneNumber).HasMaxLength(50);
                entity.Property(e => e.ContactEmail).HasMaxLength(100);
            });
        }
    }
}