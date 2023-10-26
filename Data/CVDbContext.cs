using CVApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CVApp.Data
{
    public class CVDbContext : DbContext
    {
        public CVDbContext(DbContextOptions<CVDbContext> options) : base(options)
        {
        }

        public DbSet<CV> CVs { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CV>()
                .OwnsOne(c => c.Address, a =>
                {
                    a.Property(a => a.Street).IsRequired();
                    a.Property(a => a.City).IsRequired();
                    a.Property(a => a.PostalCode).IsRequired();
                    a.Property(a => a.Country).IsRequired();
                });

            modelBuilder.Entity<CV>()
                .HasMany(cv => cv.Education)
                .WithOne()
                .HasForeignKey(school => school.CVId);

            modelBuilder.Entity<CV>()
                .HasMany(cv => cv.Experience)
                .WithOne()
                .HasForeignKey(job => job.CVId);

            modelBuilder.Entity<Job>()
                .HasMany(job => job.Skills)
                .WithOne()
                .HasForeignKey(skill => skill.JobId);

            modelBuilder.Entity<Job>()
                .HasMany(job => job.Achievements)
                .WithOne()
                .HasForeignKey(achievement => achievement.JobId);

            base.OnModelCreating(modelBuilder);
        }
    }
}