using CafeEmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeEmployeeManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Cafe> Cafes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cafe>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Cafe>().Property(c => c.Description).IsRequired();
            modelBuilder.Entity<Cafe>().Property(c => c.Location).IsRequired();

            modelBuilder.Entity<Employee>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.Email).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.PhoneNumber).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.Gender).IsRequired();

            // Configure Employee and Cafe relationships
            modelBuilder.Entity<Cafe>().HasMany(x => x.Employees).WithOne(x => x.Cafe)
                .HasForeignKey(x => x.CafeId)
                .OnDelete(DeleteBehavior.Cascade);

            SeedDataStore.SeedData(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    entry.Property(nameof(BaseEntity.CreatedDate)).IsModified = false; 
                }

                entity.UpdatedDate = DateTime.UtcNow;
            }
        }


        
    }
}
